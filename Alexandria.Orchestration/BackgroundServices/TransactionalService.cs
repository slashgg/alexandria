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
    private string queue;
    private IBackgroundWorker backgroundWorker;
    private IMailer mailer;

    public TransactionalService(IOptions<Shared.Configuration.Queue> queues, IBackgroundWorker backgroundWorker, IMailer mailClient)
    {
      this.queue = queues.Value.Email;
      this.backgroundWorker = backgroundWorker;
      this.mailer = mailClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      while (!stoppingToken.IsCancellationRequested)
      {
        var messages = await this.backgroundWorker.ReceiveMessages<DTO.EMail.Message<object>>(this.queue, 10, 15);
        if (messages != null && messages.Any())
        {
          foreach (var message in messages)
          {
            await this.HandleEmail(message.Data);
            await this.backgroundWorker.AcknowledgeMessage(this.queue, message.Receipt);
          }
        }

        // 1 minute batching
        Thread.Sleep(60 * 1000);
      }
    }

    public async Task HandleEmail(DTO.EMail.Message<object> email)
    {
      await this.mailer.SendEmail(email.TransactionalType, email.Recipient, email.Data);
    }
  }
}
