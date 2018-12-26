using System.Net.Mail;

namespace Alexandria.Shared.Utils
{
  public static class EmailHelper
  {
    public static bool IsEmail(string email)
    {
      try
      {
        new MailAddress(email);
        return true;
      } catch
      {
        return false;
      }
    }
  }
}
