using System.Collections.Generic;
using Alexandria.EF.Context;
using Alexandria.ExternalServices.BackgroundWorker;
using Alexandria.Infrastructure.Filters;
using Alexandria.Interfaces;
using Alexandria.Interfaces.Processing;
using Alexandria.Interfaces.Services;
using Alexandria.Orchestration.Mapper;
using Alexandria.Orchestration.Services;
using Alexandria.Orchestration.Utils;
using Amazon;
using Amazon.SQS;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json;
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
      .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
      //.AddJsonOptions(options =>
      //{
      //  options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
      //  options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
      //});

      JsonConvert.DefaultSettings = () =>
      {
        return new JsonSerializerSettings
        {
          Converters = new List<JsonConverter> { new Newtonsoft.Json.Converters.StringEnumConverter() }
        };
      };

      services.AddDefaultAWSOptions(new Amazon.Extensions.NETCore.Setup.AWSOptions { Region = RegionEndpoint.USEast1 });
      services.AddHttpContextAccessor();
      services.AddSvalbard();
      services.Configure<Shared.Configuration.Queue>(Configuration.GetSection("Queues"));
      services.AddScoped<AlexandriaContext>();
      services.AddScoped<SaveChangesFilter>();
      services.AddAWSService<IAmazonSQS>();
      services.AddScoped<IBackgroundWorker, SQSBackgroundWorker>();
      services.AddScoped<IUserUtils, UserUtils>();
      services.AddScoped<IAuthorizationService, AuthorizationService>();
      services.AddScoped<IUserProfileService, UserProfileService>();
      services.AddScoped<ITeamService, TeamService>();
      services.AddScoped<ITournamentService, TournamentService>();
      services.AddScoped<ICompetitionService, CompetitionService>();

      var connectionString = Configuration.GetConnectionString("Alexandria");
      services.AddDbContext<AlexandriaContext>(options =>
      {
        options.UseSqlServer(connectionString, (builder) =>
        {
          builder.MigrationsAssembly(typeof(AlexandriaContext).Assembly.FullName);
        });
      });

      IdentityModelEventSource.ShowPII = true;
      services.AddAuthentication("Bearer")
              .AddIdentityServerAuthentication(options =>
              {
                options.Authority = "https://passport.slash.gg";
                options.RequireHttpsMetadata = Production;
                options.ApiName = "Alexandria";
              });

      services.AddSwaggerDocument(options =>
      {
        options.Title = "Alexandria";
        options.Version = "1.0.0";
        options.PostProcess = settings =>
        {
          settings.Schemes.Clear();
          settings.Schemes.Add(NSwag.SwaggerSchema.Https);
        };
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
      app.UseSwaggerUi3();
      app.UseAuthentication();
      app.UseMvc();
    }
  }
}
