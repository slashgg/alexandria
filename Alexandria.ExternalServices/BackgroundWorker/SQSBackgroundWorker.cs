using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alexandria.DTO.Util;
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

    public async Task<IList<BackgroundMessage<T>>> ReceiveMessages<T>(string queue, int maxMessages = 1, int visibilityTimeout = 30)
    {
      var request = new ReceiveMessageRequest();
      request.QueueUrl = queue;
      request.MaxNumberOfMessages = maxMessages;
      request.VisibilityTimeout = visibilityTimeout;

      var result = await this.sqsClient.ReceiveMessageAsync(request);
      if (!result.Messages.Any())
      {
        return null;
      }

      var messages = result.Messages.Select(sqsMessage =>
      {
        var data = JsonConvert.DeserializeObject<T>(sqsMessage.Body);
        var message = new BackgroundMessage<T>(sqsMessage.ReceiptHandle, data);
        return message;
      }).ToList();

      return messages;
    }

    public async Task<bool> AcknowledgeMessage(string queue, string receipt)
    {
      var request = new DeleteMessageRequest(queue, receipt);
      var result = await this.sqsClient.DeleteMessageAsync(request);

      return result.HttpStatusCode == System.Net.HttpStatusCode.OK;
    }

    public async Task<bool> AcknowledgeMessage(string queue, IList<string> receipts)
    {
      var batch = receipts.Select(receipt => new DeleteMessageBatchRequestEntry(Guid.NewGuid().ToString(), receipt)).ToList();

      var request = new DeleteMessageBatchRequest(queue, batch);
      var result = await this.sqsClient.DeleteMessageBatchAsync(request);

      return !result.Failed.Any();
    }
  }
}
