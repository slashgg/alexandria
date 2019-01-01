using System;
using System.Collections.Generic;
using System.Text;
using Alexandria.Shared.Enums;

namespace Alexandria.DTO.EMail
{
  public class Message<T> : Message
  {
    public T Data { get; set; }

    public Message(string recipient, string emailTemplate, TransactionalEmail type, T data) : base(recipient, emailTemplate, type)
    {
      this.Data = data;
    }
  }

  public class Message
  {
    public string Recipient { get; set; }
    public string EmailTemplate { get; set; }
    public TransactionalEmail TransactionalType { get; set; }

    public Message(string recipient, string emailTemplate, TransactionalEmail type)
    {
      this.Recipient = recipient;
      this.EmailTemplate = emailTemplate;
      this.TransactionalType = type;
    }
  }
}
