using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Alexandria.Interfaces.Processing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Alexandria.Orchestration.BackgroundServices
{
  public class TransactionalService : BackgroundService
  {
    public string queue;
    public IBackgroundWorker backgroundWorker;

    public TransactionalService(IOptions<Shared.Configuration.Queue> queues, IBackgroundWorker backgroundWorker)
    {
      this.queue = queues.Value.Email;
      this.backgroundWorker = backgroundWorker;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      while (!stoppingToken.IsCancellationRequested)
      {
        var messages = await this.backgroundWorker.ReceiveMessages<DTO.EMail.Message<object>>(this.queue, 1, 15);
        if (messages != null && messages.Any())
        {
          foreach (var message in messages)
          {
            await this.HandleEmail(message.Data);
          }
        }
      }
    }

    public async Task HandleEmail(DTO.EMail.Message<object> email)
    {
      
    }
  }
}
