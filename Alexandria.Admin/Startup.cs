using System.Collections.Generic;
using Alexandria.Consumer.Shared.AutoMapper;
using Alexandria.Consumer.Shared.Infrastructure.Authorization;
using Alexandria.Consumer.Shared.Infrastructure.Filters;
using Alexandria.EF.Context;
using Alexandria.Interfaces.Services;
using Alexandria.Orchestration.Services;
using Amazon;
using Amazon.S3;
using Amazon.SQS;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json;
using NSwag.SwaggerGeneration.Processors.Security;
using Svalbard;

namespace Alexandria.Admin
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

      services.AddMvc(options => { options.Filters.Add<SaveChangesFilter>(); })
        .AddJsonOptions(options =>
        {
          options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
          options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
        })
       .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

      JsonConvert.DefaultSettings = () => new JsonSerializerSettings
      {
        Converters = new List<JsonConverter> { new Newtonsoft.Json.Converters.StringEnumConverter() },
      };

      services.AddDefaultAWSOptions(new Amazon.Extensions.NETCore.Setup.AWSOptions { Region = RegionEndpoint.USEast1 });
      services.AddSvalbard();
      services.Configure<Shared.Configuration.Queue>(Configuration.GetSection("Queues"));
      services.Configure<Shared.Configuration.SendGridConfig>(Configuration.GetSection("SendGrid"));
      services.Configure<Shared.Configuration.PassportClientConfiguration>(Configuration.GetSection("Passport"));
      services.Configure<Shared.Configuration.Slack>(Configuration.GetSection("Slack"));
      services.AddScoped<AlexandriaContext>();
      services.AddScoped<SaveChangesFilter>();
      services.AddAWSService<IAmazonSQS>();
      services.AddAWSService<IAmazonS3>();
      services.AddScoped<IAuthorizationService, AuthorizationService>();

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
        options.DefaultPolicy = AuthorizationPolicies.Admin;
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
        options.Title = "Alexandria.Admin";
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
            { "@slashgg/alexandria.admin", "Alexandria" }
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
