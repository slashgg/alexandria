using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alexandria.Interfaces.Processing;
using Amazon.SQS;
using Amazon.SQS.Model;
using Newtonsoft.Json;

namespace Alexandria.ExternalServices.BackgroundWorker
{
  public class SQSBackgroundWorker : IBackgroundWorker
  {
    private readonly IAmazonSQS sqsClient;
    public SQSBackgroundWorker(IAmazonSQS sqsClient)
    {
      this.sqsClient = sqsClient;
    }

    public async Task<bool> SendMessage(string queue, object message, int? delaySeconds = null)
    {
      var request = new SendMessageRequest();
      request.QueueUrl = queue;
      request.MessageBody = JsonConvert.SerializeObject(message);
      if (delaySeconds.HasValue)
      {
        request.DelaySeconds = delaySeconds.Value;
      }

      var response = await this.sqsClient.SendMessageAsync(request);

      return response.MessageId != null;
    }
  }
}
