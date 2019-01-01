using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alexandria.DTO.Util;

namespace Alexandria.Interfaces.Processing
{
  public interface IBackgroundWorker
  {
    Task<bool> SendMessage(string queue, object message, int? delaySeconds = null);
    Task<IList<BackgroundMessage<T>>> ReceiveMessages<T>(string queue, int maxMessages = 1, int visibilityTimeout = 30);
    Task<bool> AcknowledgeMessage(string queue, string receipt);
    Task<bool> AcknowledgeMessage(string queue, IList<string> receipts);
  }
}
