using System;
using System.Threading.Tasks;

namespace Alexandria.Interfaces
{
  public interface IUserUtils
  {
    Task<string> GetEmail(Guid userId);
    Task<string> GetEmailFromUserName(string userName);
    Task<Guid?> GetIdFromName(string userName);
    Task<Guid?> GetIdFromEmail(string email);
    bool GetIdFromEmail(string email, out Guid userId);
    bool GetEmail(Guid userId, out string email);
    Task GenerateExternalUserName(Guid userProfileId, Guid gameId);
  }
}
