using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alexandria.ExternalServices.HOTSLogs;
using Alexandria.Games.HeroesOfTheStorm.EF.Context;
using Alexandria.Games.HeroesOfTheStorm.Orchestration.BackgroundServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Svalbard.Extensions;

[assembly: HostingStartup(typeof(Alexandria.Games.HeroesOfTheStorm.HeroesOfTheStormInjector))]

namespace Alexandria.Games.HeroesOfTheStorm
{
  public class HeroesOfTheStormInjector : IHostingStartup
  {
    public void Configure(IWebHostBuilder builder)
    {
      builder.ConfigureAppConfiguration((builderCtx, config) =>
      {
        var prod = builderCtx.HostingEnvironment.IsProduction();
        var secrets = new List<string>();

        if (prod)
        {
          secrets.Add("HeroesOfTheStormQueues");
        }
        config.AddJsonFile($"Alexandria.Games.HeroesOfTheStorm/appsettings.{builderCtx.HostingEnvironment.EnvironmentName}.json", optional: true);
        if (secrets.Any())
        {
          config.AddAWSSecrets(options =>
          {
            options.Region = "us-east-1";
            options.Secrets = secrets.ToArray();
          });
        }
      });
      //builder.UseStartup<HeroesOfTheStormStartup>();
      builder.ConfigureServices((builderCtx, services) =>
      {
        services.AddDbContext<HeroesOfTheStormContext>(options =>
        {
          var connectionString = builderCtx.Configuration.GetConnectionString("Alexandria");
          options.UseSqlServer(connectionString, (efBuilder) =>
          {
            efBuilder.MigrationsAssembly(typeof(HeroesOfTheStormContext).Assembly.FullName);
            efBuilder.MigrationsHistoryTable("_EF_heroes_of_the_storm_migrations", "heroesofthestorm");
          });
        });

        services.Configure<Configuration.Queue>(builderCtx.Configuration.GetSection("HeroesOfTheStormQueues"));
        services.AddSingleton<HOTSLogsClient>();
        services.AddHostedService<HeroesOfTheStormCronWorker>();
        services.AddHostedService<HOTSLogsMMRPullBackgroundService>();
      });
    }
  }
}
