using Alexandria.EF.Context;
using Alexandria.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Svalbard.Services;
using System;
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

    public async Task<ServiceResult<DTO.UserProfile.Detail>> GetUserProfileDetail(Guid userId)
    {
      var result = new ServiceResult<DTO.UserProfile.Detail>();
      var user = await this.context.UserProfiles.Include(u => u.TeamMemberships)
                                                .ThenInclude(m => m.Team)
                                                .Include(u => u.TeamMemberships)
                                                .ThenInclude(m => m.TeamRole)
                                                .FirstOrDefaultAsync(u => u.Id == userId);
      if (user == null)
      {
        result.ErrorKey = Shared.ErrorKey.UserProfile.UserNotFound;
        return result;
      }

      result.Data = AutoMapper.Mapper.Map<DTO.UserProfile.Detail>(user);
      result.Succeed();
      return result;
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

      var pendingInvites = await context.TeamInvites.Where(i => string.Equals(i.Email, account.Email, System.StringComparison.CurrentCultureIgnoreCase)).ToListAsync();
      if (pendingInvites.Any())
      {
        foreach (var invite in pendingInvites)
        {
          invite.UserProfileId = account.Id;
        }

        context.TeamInvites.UpdateRange(pendingInvites);
      }

      result.Succeed();
      return result;
    }
  }
}
