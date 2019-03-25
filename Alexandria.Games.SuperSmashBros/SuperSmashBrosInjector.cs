using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Alexandria.Games.SuperSmashBros.Configuration;
using Alexandria.Games.SuperSmashBros.EF.Context;
using Alexandria.Games.SuperSmashBros.Orchestration.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Svalbard.Extensions;

[assembly: HostingStartup(typeof(Alexandria.Games.SuperSmashBros.SuperSmashBrosInjector))]
namespace Alexandria.Games.SuperSmashBros
{
  public class SuperSmashBrosInjector : IHostingStartup
  {
    public void Configure(IWebHostBuilder builder)
    {
      builder.ConfigureAppConfiguration((builderCtx, config) =>
      {
        var prod = builderCtx.HostingEnvironment.IsProduction();
        var secrets = new List<string>();

        if (prod)
        {
        }
        var envConfigPath = Directory.GetParent(builderCtx.HostingEnvironment.ContentRootPath).FullName;
        var envConfigFile = Path.Combine(envConfigPath, "Alexandria.Games.SuperSmashBros", $"appsettings.{builderCtx.HostingEnvironment.EnvironmentName}.json");


        config.AddJsonFile(envConfigFile, optional: true);
        if (secrets.Any())
        {
          config.AddAWSSecrets(options =>
          {
            options.Region = "us-east-1";
            options.Secrets = secrets.ToArray();
          });
        }
      });

      builder.ConfigureServices((builderCtx, services) =>
      {
        services.AddDbContext<SuperSmashBrosContext>(options =>
        {
          var connectionString = builderCtx.Configuration.GetConnectionString("Alexandria");
          options.UseSqlServer(connectionString, (efBuilder) =>
          {
            efBuilder.MigrationsAssembly(typeof(SuperSmashBrosContext).Assembly.FullName);
            efBuilder.MigrationsHistoryTable("_EF_super_smash_bros_migrations", Constants.Schema);
          });
        });

        services.AddScoped<SuperSmashBrosMatchService>();

        var backgroundServiceEnabled = builderCtx.Configuration.GetSection("BackgroundServices").GetValue<bool>("Enabled");
      });
    }
  }
}
