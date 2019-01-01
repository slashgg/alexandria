using System;
using System.Collections.Generic;
using System.Text;
using SendGrid.Helpers.Mail;

namespace Alexandria.Shared.Extensions
{
  public static class SendGridExtension
  {
    public static void SetupNoReplyMessage<T>(this SendGridMessage message, string templateId, string to, T model)
    {
      var from = new EmailAddress("no-reply@e.slash.gg", "slashgg");
      var personalization = new Personalization
      {
        TemplateData = model,
        Tos = new List<EmailAddress>()
      };

      personalization.Tos.Add(new EmailAddress(to));

      message.SetFrom(from);
      message.SetTemplateId(templateId);
      message.Personalizations = message.Personalizations ?? new List<Personalization>();

      message.Personalizations.Add(personalization);
    }
  }
}
