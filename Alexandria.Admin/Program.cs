using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Svalbard.Extensions;

namespace Alexandria.Admin
{
  public class Program
  {
    public static void Main(string[] args)
    {
      CreateWebHostBuilder(args).Build().Run();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
              .ConfigureAppConfiguration((hosting, config) =>
              {
                var secrets = new List<string>
                {
                  "SendGrid",
                  "Slack",
                };

                if (hosting.HostingEnvironment.IsProduction())
                {
                  secrets.Add("ConnectionStrings");
                  secrets.Add("Queues");
                }

                var basePath = hosting.HostingEnvironment.ContentRootPath;
                config.SetBasePath(basePath);
                config.AddJsonFile("appsettings.json");
                config.AddJsonFile($"appsettings.{hosting.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: false);
                config.AddAWSSecrets(options =>
                {
                  options.Region = "us-east-1";
                  options.Secrets = secrets.ToArray();
                });
              })
             .UseStartup<Startup>();
  }
}
