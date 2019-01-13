using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alexandria.Interfaces.Processing;
using Alexandria.Shared.Enums;
using Alexandria.Shared.Extensions;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Alexandria.ExternalServices.Mailer
{
  public class SendGridMailer : IMailer, IContactBook
  {
    ISendGridClient client;

    public SendGridMailer(string key)
    {
      client = new SendGridClient(key);
    }

    public async Task SendEmail(TransactionalEmail template, string email, object data)
    {
      var message = new SendGridMessage();
      var templateId = this.GetTemplateId(template);

      message.SetupNoReplyMessage(templateId, email, data);
      var result = await this.client.SendEmailAsync(message);

      return;
    }

    public async Task CreateContacts(IList<DTO.Marketing.Contact> contacts)
    {
      var data = JsonConvert.SerializeObject(contacts);
      var result = await this.client.RequestAsync(
        SendGridClient.Method.POST, 
        data, 
        urlPath: "contactdb/recipients"
      );
      return;
    }

    public async Task UpdateContacts(IList<DTO.Marketing.Contact> contacts)
    {
      var data = JsonConvert.SerializeObject(contacts);
      var result = await this.client.RequestAsync(
        SendGridClient.Method.PATCH,
        requestBody: data,
        urlPath: "contactdb/recipients"
      );
      return;
    }

    private string GetTemplateId(TransactionalEmail email)
    {
      switch (email)
      {
        case TransactionalEmail.TeamInvite:
          return Shared.Configuration.SendGridTemplates.SendInvite;
        default:
          throw new Exception("Invalid Email Template");
      }
    }
  }
}
