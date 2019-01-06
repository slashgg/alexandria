using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alexandria.DTO.UserProfile;
using Alexandria.EF.Context;
using Alexandria.EF.Models;
using Alexandria.Interfaces.Services;
using Alexandria.Shared.Enums;
using Alexandria.Shared.Utils;
using Microsoft.EntityFrameworkCore;
using Svalbard.Services;

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
      var user = await context.UserProfiles.Include(u => u.TeamMemberships)
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

      var invites = await context.TeamInvites.Include(i => i.Team)
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
      var permissions = await context.Permissions.Where(p => p.UserProfileId == userId).Select(p => p.ARN).ToListAsync();

      result.Succeed(permissions);
      return result;
    }

    public async Task<ServiceResult<List<ConnectionDetail>>> GetConnections(Guid userId)
    {
      var result = new ServiceResult<List<ConnectionDetail>>();

      var connections = await context.ExternalAccount.Where(ea => ea.UserProfileId.Equals(userId)).ToListAsync();
      var connectionDtos = connections.Select(AutoMapper.Mapper.Map<ConnectionDetail>).ToList();
      result.Succeed(connectionDtos);

      return result;
    }

    public async Task<ServiceResult<ConnectionDetail>> GetConnection(Guid userId, ExternalProvider provider)
    {
      var result = new ServiceResult<ConnectionDetail>();

      var connection = await context.ExternalAccount.FirstOrDefaultAsync(ea => ea.UserProfileId.Equals(userId) && ea.Provider.Equals(provider));
      if (connection == null)
      {
        result.Error = Shared.ErrorKey.UserProfile.ExternalAccountNotFound;
        return result;
      }

      result.Succeed(AutoMapper.Mapper.Map<ConnectionDetail>(connection));
      return result;
    }

    public async Task<ServiceResult> CreateConnection(CreateConnection createDto)
    {
      var result = new ServiceResult();

      Guid profileId;
      if (!Guid.TryParse(createDto.UserId, out profileId) || !context.UserProfiles.Any(p => p.Id.Equals(profileId)))
      {
        result.Error = Shared.ErrorKey.UserProfile.UserNotFound;
      }

      if (string.IsNullOrEmpty(createDto.ExternalId))
      {
        result.Error = Shared.ErrorKey.UserProfile.InvalidExternalId;
      }
      else if (string.IsNullOrEmpty(createDto.Name))
      {
        result.Error = Shared.ErrorKey.UserProfile.InvalidExternalName;
      }

      if (result.Error != null)
      {
        return result;
      }

      // Look for a matching external account
      if (context.ExternalAccount.Any(ea => ea.Provider == createDto.Provider && ea.ExternalId.Equals(createDto.ExternalId)))
      {
        result.Error = Shared.ErrorKey.UserProfile.ExternalAccountExists;

        return result;
      }

      await DangerouslyCreateExternalConnection(createDto, profileId);

      result.Succeed();
      return result;
    }

    public async Task<ServiceResult> DeleteConnection(string connectionId)
    {
      var result = new ServiceResult();
      Guid id;
      if (Guid.TryParse(connectionId, out id))
      {
        var connection = await context.ExternalAccount.FindAsync(id);
        if (connection != null)
        {
          await DangerouslyDeleteConnection(connection);
        }
      }

      result.Succeed();
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

      await DangerouslyCreateUserProfile(account);

      result.Succeed();
      return result;
    }

    public async Task<ExternalAccount> DangerouslyCreateExternalConnection(CreateConnection dto, Guid profileId)
    {
      var externalAccount = new ExternalAccount(dto.Provider, dto.Name, dto.ExternalId, profileId);

      await context.AddAsync(externalAccount);
      await authorizationService.AddPermission(profileId, AuthorizationHelper.GenerateARN(typeof(ExternalAccount), externalAccount.Id.ToString(), Shared.Permissions.ExternalAccount.Delete));

      return externalAccount;
    }

    private async Task DangerouslyDeleteConnection(ExternalAccount connection)
    {
      await authorizationService.RemovePermission(connection.UserProfileId, AuthorizationHelper.GenerateARN(typeof(ExternalAccount), connection.Id.ToString(), Shared.Permissions.ExternalAccount.Delete));
      context.ExternalAccount.Remove(connection);
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
          await authorizationService.AddPermission(userData.Id, AuthorizationHelper.GenerateARN(typeof(EF.Models.TeamInvite), invite.Id.ToString(), Shared.Permissions.TeamInvite.All));
        }

        context.TeamInvites.UpdateRange(pendingInvites);
      }
      return userAccount;
    }
  }
}
