using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Alexandria.Interfaces.Processing;
using Alexandria.Orchestration.BackgroundServices.Crons;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Alexandria.Orchestration.BackgroundServices
{
  public class CronService : BackgroundService
  {
    private string queue;
    private IBackgroundWorker backgroundWorker;
    private IServiceProvider serviceProvider;

    public CronService(IOptions<Shared.Configuration.Queue> queues, IBackgroundWorker backgroundWorker, IServiceProvider provider)
    {
      this.queue = queues.Value.Cron;
      this.backgroundWorker = backgroundWorker;
      this.serviceProvider = provider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      while (!stoppingToken.IsCancellationRequested)
      {
        var messages = await this.backgroundWorker.ReceiveMessages<DTO.Util.Cron>(this.queue, 1, 300);
        if (messages != null && messages.Any())
        {
          foreach (var message in messages)
          {
            try
            {
              var type = message.Data.CronType;
              switch (type)
              {
                case Shared.Enums.Cron.ExternalUserName:
                  var externalUserNameCron = new ExternalUserNameCron(this.serviceProvider);
                  await externalUserNameCron.Work();
                  break;
                default:
                  break;
              }
            } catch (Exception ex)
            {
              Sentry.SentrySdk.CaptureException(ex);
            } finally
            {
              await this.backgroundWorker.AcknowledgeMessage(this.queue, message.Receipt);
            }
          }
        }
        Thread.Sleep(300 * 1000);
      }
    }
  }
}
