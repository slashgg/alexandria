using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Alexandria.DTO.UserProfile;
using Alexandria.EF.Context;
using Alexandria.EF.Models;
using Alexandria.Interfaces.Processing;
using Alexandria.Interfaces.Services;
using Alexandria.Shared.Enums;
using Alexandria.Shared.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Svalbard.Services;
using Svalbard.Utils;

namespace Alexandria.Orchestration.Services
{
  public class UserProfileService : IUserProfileService
  {
    private readonly EF.Context.AlexandriaContext context;
    private readonly IAuthorizationService authorizationService;
    private readonly IPassportClient passportClient;
    private readonly IBackgroundWorker backgroundWorker;
    private readonly Shared.Configuration.Queue queues;

    public UserProfileService(AlexandriaContext context, IAuthorizationService authorizationService, IPassportClient passportClient, IBackgroundWorker backgroundWorker, IOptions<Shared.Configuration.Queue> queues)
    {
      this.context = context;
      this.authorizationService = authorizationService;
      this.passportClient = passportClient;
      this.backgroundWorker = backgroundWorker;
      this.queues = queues.Value ?? throw new NoNullAllowedException("Queue Options can't be null");
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
      await this.backgroundWorker.SendMessage(this.queues.Contact, new DTO.Marketing.ContactSync(account.Id, true), 5);

      return result;
    }

    public async Task<ServiceResult<string>> UpdateAvatar(Guid value, string presignedUrl)
    {
      var result = new ServiceResult<string>();
      if (string.IsNullOrEmpty(presignedUrl))
      {
        result.Error = Shared.ErrorKey.Asset.PresignedUrlInvalid;
        return result;
      }

      var parts = presignedUrl.Split('?');
      if (parts.Length != 2)
      {
        result.Error = Shared.ErrorKey.Asset.PresignedUrlInvalid;
        return result;
      }

      var oldAvatar = await DangerouslyUpdateProfileAvatarUrl(parts.First(), value);

      result.Succeed(oldAvatar);
      return result;
    }

    public async Task<ServiceResult> ResendEmailVerification(Guid userId)
    {
      var user = await context.UserProfiles.FindAsync(userId);
      if (user == null)
      {
        return new ServiceResult(Shared.ErrorKey.UserProfile.UserNotFound);
      }

      return await passportClient.ResendEmailVerification(user.Email);
    }

    public async Task<ServiceResult> UpdateSettings(Guid userId, UpdateSettings updateDto)
    {
      var result = new ServiceResult();

      if (updateDto == null || string.IsNullOrEmpty(updateDto.Email) || string.IsNullOrEmpty(updateDto.Username))
      {
        result.Error = Shared.ErrorKey.UserProfile.InvalidUserSettings;
        return result;
      }

      if (updateDto.DateOfBirth.HasValue)
      {
        var userAgeInDays = (DateTimeOffset.UtcNow - updateDto.DateOfBirth.Value).TotalDays;
        if (userAgeInDays > Shared.Configuration.UserProfileValidations.MaxAgeInDays)
        {
          result.Error = Shared.ErrorKey.UserProfile.UserTooOld;
          return result;
        }
        else if (userAgeInDays < Shared.Configuration.UserProfileValidations.MinAgeInDays)
        {
          result.Error = Shared.ErrorKey.UserProfile.UserTooYoung;
          return result;
        }
      }

      if (!await context.UserProfiles.AnyAsync(up => up.Id.Equals(userId)))
      {
        result.Error = Shared.ErrorKey.UserProfile.UserNotFound;
        return result;
      }

      if (await context.UserProfiles.AnyAsync(up => !up.Id.Equals(userId) && up.Email.Equals(updateDto.Email, StringComparison.InvariantCultureIgnoreCase)))
      {
        result.Error = Shared.ErrorKey.UserProfile.ProfileExists;
        return result;
      }

      var passportResult = await passportClient.UpdateProfile(userId, AutoMapper.Mapper.Map<UpdatePassportUser>(updateDto));
      if (!passportResult.Success)
      {
        return passportResult;
      }


      await DangerouslyUpdateProfileSettings(updateDto, userId);
      await this.backgroundWorker.SendMessage(this.queues.Contact, new DTO.Marketing.ContactSync(userId), 5);

      result.Succeed();
      return result;
    }

    public async Task<ServiceResult<IList<DTO.UserProfile.FavoriteCompetition>>> GetFavoriteCompetitions(Guid userId)
    {
      var result = new ServiceResult<IList<DTO.UserProfile.FavoriteCompetition>>();
      var favorites = await context.FavoriteCompetitions.Include(fc => fc.Competition).Where(f => f.UserProfileId == userId).ToListAsync();
      var favoriteDTOs = favorites.Select(AutoMapper.Mapper.Map<DTO.UserProfile.FavoriteCompetition>).ToList();
      result.Succeed(favoriteDTOs);
      return result;
    }

    public async Task<ServiceResult> AddFavoriteCompetition(Guid userId, Guid competitionId)
    {
      var result = new ServiceResult();
      var userExists = await context.UserProfiles.AnyAsync(u => u.Id == userId);
      if (!userExists)
      {
        result.Error = Shared.ErrorKey.UserProfile.UserNotFound;
        return result;
      }

      var competitionExists = await context.Competitions.AnyAsync(c => c.Id == competitionId);
      if (!competitionExists)
      {
        result.Error = Shared.ErrorKey.Competition.NotFound;
        return result;
      }

      var favoriteCompetitionExists = await context.FavoriteCompetitions.AnyAsync(fc => fc.CompetitionId == competitionId && fc.UserProfileId == userId);
      if (favoriteCompetitionExists)
      {
        result.Error = Shared.ErrorKey.FavoriteCompetition.AlreadyExists;
        return result;
      }

      await this.DangerouslyCreateFavoriteCompetition(userId, competitionId);
      result.Succeed();
      return result;
    }

    public async Task<ServiceResult> DeleteFavoriteCompetition(Guid favoriteId)
    {
      var result = new ServiceResult();
      var favoriteExists = await this.context.FavoriteCompetitions.AnyAsync(fc => fc.Id == favoriteId);
      if (!favoriteExists)
      {
        result.Error = Shared.ErrorKey.FavoriteCompetition.NotFound;
        return result;
      }

      await DangerouslyDeleteFavoriteCompetition(favoriteId);
      result.Succeed();
      return result;
    }

    private async Task DangerouslyUpdateProfileSettings(UpdateSettings updateDto, Guid userId)
    {
      var profile = await context.UserProfiles.FindAsync(userId);
      profile.Birthday = updateDto.DateOfBirth;
      profile.Email = updateDto.Email;
      profile.UserName = updateDto.Username + $"#{MurmurHash2.ComputeHash(updateDto.Email) % 100000}";
      profile.DisplayName = updateDto.Username;
    }

    private async Task<string> DangerouslyUpdateProfileAvatarUrl(string url, Guid profileId)
    {
      var profile = await context.UserProfiles.FindAsync(profileId);
      if (profile == null)
      {
        return null;
      }

      var oldAvatar = profile.AvatarURL;
      profile.AvatarURL = url;

      context.Entry(profile).State = EntityState.Modified;

      return oldAvatar;
    }

    private async Task<ExternalAccount> DangerouslyCreateExternalConnection(CreateConnection dto, Guid profileId)
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
          var invitePermission = new Permission(userData.Id, AuthorizationHelper.GenerateARN(typeof(EF.Models.TeamInvite), invite.Id.ToString(), Shared.Permissions.TeamInvite.All));
          context.Permissions.Add(invitePermission);
        }

        context.TeamInvites.UpdateRange(pendingInvites);
      }
      return userAccount;
    }

    private async Task DangerouslyCreateFavoriteCompetition(Guid userId, Guid competitionId)
    {
      var favorite = new EF.Models.FavoriteCompetition(userId, competitionId);
      await authorizationService.AddPermission(userId, AuthorizationHelper.GenerateARN(typeof(EF.Models.FavoriteCompetition), favorite.Id.ToString(), Shared.Permissions.FavoriteCompetition.All));
      context.FavoriteCompetitions.Add(favorite);
    }

    private async Task DangerouslyDeleteFavoriteCompetition(Guid favoriteId)
    {
      var favorite = await context.FavoriteCompetitions.FirstOrDefaultAsync(fc => fc.Id == favoriteId);
      await authorizationService.RemovePermission(favorite.UserProfileId, AuthorizationHelper.GenerateARN(typeof(EF.Models.FavoriteCompetition), favorite.Id.ToString(), Shared.Permissions.FavoriteCompetition.All));
      context.FavoriteCompetitions.Remove(favorite);
    }
  }
}
