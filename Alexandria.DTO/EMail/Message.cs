using System;
using System.Collections.Generic;
using System.Text;

namespace Alexandria.DTO.EMail
{
  public class Message<T> : Message
  {
    public T Data { get; set; }

    public Message(string recipient, string emailTemplate, T data) : base(recipient, emailTemplate)
    {
      this.Data = data;
    }
  }

  public class Message
  {
    public string Recipient { get; set; }
    public string EmailTemplate { get; set; }

    public Message(string recipient, string emailTemplate)
    {
      this.Recipient = recipient;
      this.EmailTemplate = emailTemplate;
    }
  }
}
