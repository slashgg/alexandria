using System;
using System.Collections.Generic;
using System.Text;
using Alexandria.Shared.Enums;

namespace Alexandria.DTO.EMail
{
  public class Message<T> : Message
  {
    public T Data { get; set; }

    public Message(string recipient, TransactionalEmail type, T data) : base(recipient, type)
    {
      this.Data = data;
    }
  }

  public class Message
  {
    public string Recipient { get; set; }
    public TransactionalEmail TransactionalType { get; set; }

    public Message(string recipient, TransactionalEmail type)
    {
      this.Recipient = recipient;
      this.TransactionalType = type;
    }
  }
}
