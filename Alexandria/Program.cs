using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Svalbard.Extensions;

namespace Alexandria
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
              var prod = hosting.HostingEnvironment.IsProduction();
              var secrets = new List<string>();
              secrets.Add("SendGrid");
              if (hosting.HostingEnvironment.IsProduction())
              {
                secrets.Add("ConnectionStrings");
                secrets.Add("Queues");
              }
              config.SetBasePath(hosting.HostingEnvironment.ContentRootPath);
              config.AddJsonFile("appsettings.json");
              config.AddJsonFile($"appsettings.{hosting.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: false);
              config.AddAWSSecrets(options =>
              {
                options.Region = "us-east-1";
                options.Secrets = secrets.ToArray();
              });
            })
            .UseSentry(opts =>
            {
              var version = typeof(Program).Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
              opts.Release = version;
            })
            .UseStartup<Startup>();
  }
}
