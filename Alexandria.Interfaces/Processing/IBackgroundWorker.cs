using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Alexandria.Interfaces.Processing
{
  public interface IBackgroundWorker
  {
    Task<bool> SendMessage(string queue, object message, int? delaySeconds = null);
  }
}
