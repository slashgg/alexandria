using Svalbard.Services;
using System.Threading.Tasks;

namespace Alexandria.Interfaces.Services
{
  public interface IUserProfileService
  {
    Task<ServiceResult> CreateAccount(Alexandria.DTO.UserProfile.Create account);
  }
}
