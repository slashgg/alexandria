using Alexandria.EF.Context;
using Alexandria.EF.Models;
using Alexandria.Interfaces.Services;
using Alexandria.Shared.Utils;
using Microsoft.EntityFrameworkCore;
using Svalbard.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alexandria.Orchestration.Services
{
  public class UserProfileService : IUserProfileService
  {
    private readonly EF.Context.AlexandriaContext context;
    private readonly IAuthorizationService authorizationService;

    public UserProfileService(AlexandriaContext context, IAuthorizationService authorizationService)
    {
      this.context = context;
      this.authorizationService = authorizationService;
    }

    public async Task<ServiceResult<DTO.UserProfile.Detail>> GetUserProfileDetail(Guid userId)
    {
      var result = new ServiceResult<DTO.UserProfile.Detail>();
      var user = await this.context.UserProfiles.Include(u => u.TeamMemberships)
                                                .ThenInclude(m => m.Team)
                                                .ThenInclude(t => t.Competition)
                                                .Include(u => u.TeamMemberships)
                                                .ThenInclude(m => m.TeamRole)
                                                .FirstOrDefaultAsync(u => u.Id == userId);
      if (user == null)
      {
        result.Error = Shared.ErrorKey.UserProfile.UserNotFound;
        return result;
      }

      result.Data = AutoMapper.Mapper.Map<DTO.UserProfile.Detail>(user);
      result.Succeed();
      return result;
    }

    public async Task<ServiceResult<IList<DTO.UserProfile.TeamInvite>>> GetTeamInvites(Guid userId)
    {
      var result = new ServiceResult<IList<DTO.UserProfile.TeamInvite>>();

      var invites = await this.context.TeamInvites.Include(i => i.Team)
                                                  .ThenInclude(t => t.Competition)
                                                  .Include(i => i.UserProfile)
                                                  .Where(i => i.UserProfileId == userId)
                                                  .ToListAsync();

      var inviteDTOs = invites.Select(AutoMapper.Mapper.Map<DTO.UserProfile.TeamInvite>).ToList();

      result.Succeed(inviteDTOs);
      return result;
    }

    public async Task<ServiceResult<IList<string>>> GetPermissions(Guid userId)
    {
      var result = new ServiceResult<IList<string>>();
      var permissions = await this.context.Permissions.Where(p => p.UserProfileId == userId).Select(p => p.ARN).ToListAsync();

      result.Succeed(permissions);
      return result;
    }

    public async Task<ServiceResult> CreateAccount(DTO.UserProfile.Create account)
    {
      var result = new ServiceResult();
      if (context.UserProfiles.Any(u => u.Email == account.Email || u.DisplayName == account.DisplayName || u.Id == account.Id))
      {
        result.Error = Shared.ErrorKey.UserProfile.ProfileExists;
        return result;
      }

      await this.DangerouslyCreateUserProfile(account);

      result.Succeed();
      return result;
    }

    private async Task<UserProfile> DangerouslyCreateUserProfile(DTO.UserProfile.Create userData)
    {
      var userAccount = new EF.Models.UserProfile(userData.Id, userData.DisplayName, userData.Email);
      await context.UserProfiles.AddAsync(userAccount);

      var pendingInvites = await context.TeamInvites.Where(i => string.Equals(i.Email, userData.Email, System.StringComparison.CurrentCultureIgnoreCase)).ToListAsync();
      if (pendingInvites.Any())
      {
        foreach (var invite in pendingInvites)
        {
          invite.UserProfileId = userData.Id;
          await this.authorizationService.AddPermission(userData.Id, AuthorizationHelper.GenerateARN(typeof(TeamInvite), invite.Id.ToString(), Shared.Permissions.TeamInvite.All));
        }

        context.TeamInvites.UpdateRange(pendingInvites);
      }
      return userAccount;
    }
  }
}
