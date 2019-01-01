using System.Threading.Tasks;
using Alexandria.Shared.Enums;

namespace Alexandria.Interfaces.Processing
{
  public interface IMailer
  {
    Task SendEmail(TransactionalEmail template, string email, object data);
  }
}
