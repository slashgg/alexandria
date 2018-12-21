using Alexandria.EF.Context;
using Alexandria.Interfaces.Services;
using Svalbard.Services;
using System.Linq;
using System.Threading.Tasks;

namespace Alexandria.Orchestration.Services
{
  public class UserProfileService : IUserProfileService
  {
    private readonly EF.Context.AlexandriaContext context;

    public UserProfileService(AlexandriaContext context)
    {
      this.context = context;
    }

    public async Task<ServiceResult> CreateAccount(DTO.UserProfile.Create account)
    {
      var result = new ServiceResult();
      if (context.UserProfiles.Any(u => u.Email == account.Email || u.DisplayName == account.DisplayName || u.Id == account.Id))
      {
        result.ErrorKey = Shared.ErrorKey.UserProfile.ProfileExists;
        return result;
      }

      var userAccount = new EF.Models.UserProfile(account.Id, account.DisplayName, account.Email);

      await context.UserProfiles.AddAsync(userAccount);

      result.Succeed();
      return result;
    }
  }
}
