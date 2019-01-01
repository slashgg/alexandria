using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alexandria.Interfaces.Processing;
using Alexandria.Shared.Enums;
using Alexandria.Shared.Extensions;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Alexandria.ExternalServices.Mailer
{
  public class SendGridMailer : IMailer
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
