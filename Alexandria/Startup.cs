using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alexandria.EF.Context;
using Alexandria.Infrastructure.Filters;
using Alexandria.Interfaces.Services;
using Alexandria.Orchestration.Mapper;
using Alexandria.Orchestration.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
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

      services.AddHttpContextAccessor();

      services.AddSvalbard();
      services.AddScoped<AlexandriaContext>();
      services.AddScoped<SaveChangesFilter>();
      services.AddScoped<IUserProfileService, UserProfileService>();
      services.AddScoped<ITeamService, TeamService>();

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
                //options.Authority = Production ? "https//passport.slash.gg" : "http://localhost:5000";
                options.RequireHttpsMetadata = Production;
                options.ApiName = "Alexandria";
              });

      services.AddSwaggerDocument();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseSwagger();
        app.UseSwaggerUi3();
        app.UseDeveloperExceptionPage();
      }
      else
      {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseAuthentication();
      app.UseMvc();
      
    }
  }
}
