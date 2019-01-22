using System.Collections.Generic;
using Alexandria.EF.Context;
using Alexandria.ExternalServices.BackgroundWorker;
using Alexandria.ExternalServices.Mailer;
using Alexandria.ExternalServices.Slack;
using Alexandria.Infrastructure.Authorization;
using Alexandria.Infrastructure.Filters;
using Alexandria.Interfaces;
using Alexandria.Interfaces.Processing;
using Alexandria.Interfaces.Services;
using Alexandria.Orchestration.BackgroundServices;
using Alexandria.Orchestration.Mapper;
using Alexandria.Orchestration.Services;
using Alexandria.Orchestration.Utils;
using Amazon;
using Amazon.S3;
using Amazon.SQS;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json;
using NSwag.SwaggerGeneration.Processors.Security;
using Svalbard;

namespace Alexandria
{
  public class Startup
  {
    public Startup(IHostingEnvironment env, IConfiguration configuration)
    {
      Configuration = configuration;
      Production = env.IsProduction();
    }

    public IConfiguration Configuration { get; }
    public bool Production { get; set; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {

      AutoMapperConfig.Initialize();
      services.AddMvc(options =>
      {
        options.Filters.Add<SaveChangesFilter>();
      })
      .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
      .AddJsonOptions(options =>
      {
        options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
      });

      JsonConvert.DefaultSettings = () =>
      {
        return new JsonSerializerSettings
        {
          Converters = new List<JsonConverter> { new Newtonsoft.Json.Converters.StringEnumConverter() }
        };
      };

      services.AddMemoryCache();
      services.AddDefaultAWSOptions(new Amazon.Extensions.NETCore.Setup.AWSOptions { Region = RegionEndpoint.USEast1 });
      services.AddHttpContextAccessor();
      services.AddSvalbard();
      services.Configure<Shared.Configuration.Queue>(Configuration.GetSection("Queues"));
      services.Configure<Shared.Configuration.SendGridConfig>(Configuration.GetSection("SendGrid"));
      services.Configure<Shared.Configuration.PassportClientConfiguration>(Configuration.GetSection("Passport"));
      services.Configure<Shared.Configuration.Slack>(Configuration.GetSection("Slack"));
      services.AddScoped<AlexandriaContext>();
      services.AddScoped<SaveChangesFilter>();
      services.AddAWSService<IAmazonSQS>();
      services.AddAWSService<IAmazonS3>();
      services.AddSingleton<IBackgroundWorker, SQSBackgroundWorker>();
      services.AddScoped<IUserUtils, UserUtils>();
      services.AddScoped<IAuthorizationService, AuthorizationService>();
      services.AddScoped<IUserProfileService, UserProfileService>();
      services.AddScoped<ITeamService, TeamService>();
      services.AddScoped<ITournamentService, TournamentService>();
      services.AddScoped<ICompetitionService, CompetitionService>();
      services.AddScoped<IFileService, FileService>();
      services.AddScoped<IPassportClient, PassportClient>();
      services.AddScoped<IProfanityValidator, ProfanityValidator>();
      services.AddScoped<SlackClient>();

      services.AddSingleton<IMimeMappingService>(provider =>
      {
        var staticProvider = new FileExtensionContentTypeProvider();
        return new MimeMappingService(staticProvider.Mappings);
      });

      services.AddSingleton<IMailer, SendGridMailer>(provider =>
      {
        var accessor = provider.GetRequiredService<IOptions<Shared.Configuration.SendGridConfig>>();
        return new SendGridMailer(accessor.Value.ApiKey);
      });

      services.AddSingleton<IContactBook, SendGridMailer>(provider =>
      {
        var accessor = provider.GetRequiredService<IOptions<Shared.Configuration.SendGridConfig>>();
        return new SendGridMailer(accessor.Value.ApiKey);
      });

      services.AddHostedService<TransactionalService>();
      services.AddHostedService<ContactSyncBackgroundService>();

      var connectionString = Configuration.GetConnectionString("Alexandria");
      services.AddDbContext<AlexandriaContext>(options =>
      {
        options.UseSqlServer(connectionString, (builder) =>
        {
          builder.MigrationsAssembly(typeof(AlexandriaContext).Assembly.FullName);
        });
      });

      services.AddAuthorization(options =>
      {
        options.DefaultPolicy = AuthorizationPolicies.Default;
        options.AddPolicy("Backchannel", AuthorizationPolicies.Backchannel);
      });

      var passportHost = Production ? "https://passport.slash.gg" : "http://localhost:52215";

      IdentityModelEventSource.ShowPII = true;
      services.AddAuthentication("Bearer")
              .AddIdentityServerAuthentication(options =>
              {
                options.Authority = passportHost;
                options.RequireHttpsMetadata = Production;
                options.ApiName = "Alexandria";
              });

      services.AddSwaggerDocument(options =>
      {
        options.Title = "Alexandria";
        options.Version = "1.0.0";
        options.PostProcess = settings =>
        {
          if (Production)
          {
            settings.Schemes.Clear();
            settings.Schemes.Add(NSwag.SwaggerSchema.Https);
          }
        };

        options.OperationProcessors.Add(new OperationSecurityScopeProcessor("oauth2"));

        options.DocumentProcessors.Add(new SecurityDefinitionAppender("oauth2", new NSwag.SwaggerSecurityScheme
        {
          Type = NSwag.SwaggerSecuritySchemeType.OAuth2,
          OpenIdConnectUrl = $"{passportHost}/.well-known/openid-configuration",
          TokenUrl = $"{passportHost}/connect/token",
          AuthorizationUrl = $"{passportHost}/connect/authorize",
          Flow = NSwag.SwaggerOAuth2Flow.Implicit,
          Scheme = "Bearer",
          Name = "Authorization",
          In = NSwag.SwaggerSecurityApiKeyLocation.Header,
          Scopes = new Dictionary<string, string>
          {
            { "@slashgg/alexandria.full_access", "Alexandria" }
          }
        }));
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseHttpsRedirection();
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseCors(options =>
      {
        options.AllowAnyHeader();
        options.AllowAnyMethod();
        options.AllowAnyOrigin();
      });

      app.UseSwagger();
      app.UseSwaggerUi3(configuration =>
      {
        configuration.OAuth2Client = new NSwag.AspNetCore.OAuth2ClientSettings
        {
          AppName = "Alexandria",
          ClientId = "slashgg-alexandria-swagger",
        };

        configuration.OAuth2Client.AdditionalQueryStringParameters.Add("nonce", System.Guid.NewGuid().ToString("N"));
      });
      app.UseAuthentication();
      app.UseMvc();
    }
  }
}
