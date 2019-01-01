using System;
using System.Collections.Generic;
using System.Text;

namespace Alexandria.DTO.Util
{
  public class BackgroundMessage<T> : BackgroundMessage
  {
    public T Data { get; set; }

    public BackgroundMessage() { }

    public BackgroundMessage(string receipt, T data) : base(receipt)
    {
      this.Data = data;
    }
  }

  public class BackgroundMessage
  {
    public string Receipt { get; set; }

    public BackgroundMessage() { }
    public BackgroundMessage(string receipt)
    {
      this.Receipt = receipt;
    }
  }
}
