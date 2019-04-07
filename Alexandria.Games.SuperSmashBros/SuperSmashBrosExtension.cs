using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Alexandria.Games.SuperSmashBros.Configuration;
using Alexandria.Games.SuperSmashBros.EF.Context;
using Alexandria.Games.SuperSmashBros.Orchestration.BackgroundServices;
using Alexandria.Games.SuperSmashBros.Orchestration.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Svalbard.Extensions;

namespace Alexandria.Games.SuperSmashBros
{
  public static class SuperSmashBrosExtension
  {
    public static void AddSuperSmashBros(this IServiceCollection services, IConfiguration configuration)
    {
      var connectionString = configuration.GetConnectionString("Alexandria");
      var backgroundServicesEnabled = configuration.GetSection("BackgroundServices").GetValue<bool>("Enabled");

      services.AddDbContext<SuperSmashBrosContext>(options =>
      {
        options.UseSqlServer(connectionString, (efBuilder) =>
        {
          efBuilder.MigrationsAssembly(typeof(SuperSmashBrosContext).Assembly.FullName);
          efBuilder.MigrationsHistoryTable("_EF_super_smash_bros_migrations", Constants.Schema);
        });
      });
      services.Configure<Configuration.Queue>(configuration.GetSection("SuperSmashBrosQueues"));
      services.AddScoped<SuperSmashBrosMatchService>();

      if (backgroundServicesEnabled)
      {
        services.AddHostedService<MatchResultJobWorker>();
      }
    }
  }
}
