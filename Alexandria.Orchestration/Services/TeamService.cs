﻿
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Alexandria.EF.Context;
using Alexandria.EF.Models;
using Alexandria.ExternalServices.Slack;
using Alexandria.Interfaces;
using Alexandria.Interfaces.Processing;
using Alexandria.Interfaces.Services;
using Alexandria.Shared.Enums;
using Alexandria.Shared.Extensions;
using Alexandria.Shared.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Svalbard.Services;

namespace Alexandria.Orchestration.Services
{
  public class TeamService : ITeamService
  {
    private readonly HttpContext httpContext;
    private readonly AlexandriaContext context;
    private readonly IUserUtils userUtils;
    private readonly IAuthorizationService authorizationService;
    private readonly IBackgroundWorker backgroundWorker;
    private readonly Shared.Configuration.Queue queues;
    private readonly IProfanityValidator profanityValidator;
    private readonly SlackClient slackClient;
    private readonly ICacheBreaker cacheBreaker;

    public TeamService(IHttpContextAccessor httpContext, 
                       AlexandriaContext context, 
                       IUserUtils userUtils, 
                       IAuthorizationService authorizationService, 
                       IBackgroundWorker backgroundWorker, 
                       IOptions<Shared.Configuration.Queue> queues, 
                       IProfanityValidator profanityValidator,
                       SlackClient slackClient,
                       ICacheBreaker cacheBreaker)
    {
      this.httpContext = httpContext.HttpContext;
      this.context = context;
      this.userUtils = userUtils;
      this.authorizationService = authorizationService;
      this.backgroundWorker = backgroundWorker;
      this.queues = queues.Value ?? throw new NoNullAllowedException("Queue Options can't be null");
      this.profanityValidator = profanityValidator;
      this.slackClient = slackClient;
      this.cacheBreaker = cacheBreaker;
    }

    public async Task<ServiceResult<DTO.Team.Detail>> GetTeamDetail(Guid teamId)
    {
      var result = new ServiceResult<DTO.Team.Detail>();
      var team = await this.context.Teams
                                   .Include(t => t.TeamMemberships)
                                   .ThenInclude(m => m.TeamRole)
                                   .Include(t => t.TeamMemberships)
                                   .ThenInclude(m => m.UserProfile)
                                   .Include(t => t.Competition)
                                   .FirstOrDefaultAsync(t => t.Id == teamId);

      if (team == null)
      {
        result.Error = Shared.ErrorKey.Team.TeamNotFound;
        return result;
      }

      result.Data = AutoMapper.Mapper.Map<DTO.Team.Detail>(team);
      result.Succeed();

      return result;
    }

    public async Task<ServiceResult<IList<DTO.Team.Invite>>> GetTeamInvites(Guid teamId)
    {
      var result = new ServiceResult<IList<DTO.Team.Invite>>();

      var invites = await this.context.TeamInvites
                                   .Include(i => i.UserProfile)
                                   .Where(i => i.TeamId == teamId)
                                   .ToListAsync();

      var inviteDTOs = invites.Select(AutoMapper.Mapper.Map<DTO.Team.Invite>).ToList();
      result.Succeed(inviteDTOs);

      return result;
    }

    public async Task<ServiceResult> CreateTeam(Guid competitionId, DTO.Team.Create teamData)
    {
      var id = this.httpContext.GetUserId();
      if (id == null)
      {
        throw new NoNullAllowedException("UserId can't be null");
      }

      return await CreateTeam(competitionId, teamData, id.Value);
    }
    
    public async Task<ServiceResult> CreateTeam(Guid competitionId, DTO.Team.Create teamData, Guid userId)
    {
      var result = new ServiceResult();


      var competition = await this.context.Competitions.FirstOrDefaultAsync(t => t.Id == competitionId);
      if (competition == null)
      {
        result.Error = Shared.ErrorKey.Competition.NotFound;
        return result;
      }

      // Check if name is taken
      if (context.Teams.Any(t => string.Equals(teamData.Name, t.Name, StringComparison.CurrentCultureIgnoreCase) && t.CompetitionId == competitionId && t.TeamState != TeamState.Disbanded))
      {
        result.Error = Shared.ErrorKey.Team.NameTaken;
        return result;
      }

      // Check User Existence
      var user = await context.UserProfiles.Include(u => u.TeamMemberships).ThenInclude(m => m.Team).FirstOrDefaultAsync(u => u.Id == userId);
      if (user == null)
      {
        result.Error = Shared.ErrorKey.UserProfile.UserNotFound;
      }

      // Check if user is already in team for this competition
      if (user.HasTeam(competitionId))
      {
        result.Error = Shared.ErrorKey.UserProfile.AlreadyInCompetitionTeam;
        return result;
      }

      var profanityCheck = await this.profanityValidator.Check(teamData.Name);
      if (profanityCheck.Severity == ProfanityFilterSeverity.Blacklist)
      {
        this.slackClient.SendMessage($"[SEVERITY 1] User {user.UserName} ({userId}) tried to create a Team called {teamData.Name} in Competition {competition.Name} ({competition.Id}), we blocked this action", Shared.ExternalResources.Slack.ProfanityFilterChannel);

        result.Error = Shared.ErrorKey.ProfanityFilter.BlacklistedWord;
        return result;
      }
      else if (profanityCheck.Severity == ProfanityFilterSeverity.Suspicious)
      {
        this.slackClient.SendMessage($"[SEVERITY 2] User {user.UserName} ({userId}) created a Team called {teamData.Name} in Competition {competition.Name} ({competition.Id})", Shared.ExternalResources.Slack.ProfanityFilterChannel);
      }


      var team = await this.DangerouslyCreateTeam(competitionId, teamData, user.Id);

      this.context.Teams.Add(team);
      foreach (var invite in team.TeamInvites.ToList())
      {
        var message = new DTO.EMail.Message<DTO.EMail.TeamInvite>(invite.Email, TransactionalEmail.TeamInvite, new DTO.EMail.TeamInvite(invite.Id, competition.Id, competition.Name, competition.Slug, team.Id, team.Name, team.Slug));
        await this.backgroundWorker.SendMessage(this.queues.Email, message);
      }

      this.cacheBreaker.Break(Shared.Cache.Competition.Detail(competition.Id));
      result.Succeed();

      return result;
    }

    public async Task<ServiceResult> InviteMember(Guid teamId, string invitee)
    {
      var result = new ServiceResult();
      var team = await this.context.Teams
                             .Include(t => t.TeamInvites)
                             .ThenInclude(t => t.UserProfile)
                             .Include(t => t.TeamMemberships)
                             .ThenInclude(t => t.UserProfile)
                             .Include(t => t.Competition)
                             .FirstOrDefaultAsync(t => t.Id == teamId);

      // Check if Team Exists
      if (team == null)
      {
        result.Error = Shared.ErrorKey.Team.TeamNotFound;
        return result;
      }

      var invitedUser = await this.context.UserProfiles.FirstOrDefaultAsync(u => u.UserName == invitee || u.Email == invitee);

      // Check if User is already a member
      if (invitedUser != null && team.HasMember(invitedUser.Id))
      {
        result.Error = Shared.ErrorKey.Team.AlreadyInTeam;
        return result;
      }

      // Check if there is already an invite for the user
      if (team.HasInvite(invitee) || (invitedUser != null && team.HasInvite(invitedUser.Id)))
      {
        result.Error = Shared.ErrorKey.Invite.AlreadyInvited;
        return result;
      }

      var competition = team.Competition;
      if (competition == null)
      {
        result.Error = Shared.ErrorKey.Competition.NotFound;
        return result;
      }

      if (!team.HasOpenSpots())
      {
        result.Error = Shared.ErrorKey.Competition.MaxTeamSize;
        return result;
      }

      var invite = await this.DangerouslyCreateInvite(teamId, invitee);
      if (invite == null)
      {
        result.Error = Shared.ErrorKey.Invite.InvalidRecipient;
        return result;
      }

      team.TeamInvites.Add(invite);

      this.context.Teams.Update(team);
      var message = new DTO.EMail.Message<DTO.EMail.TeamInvite>(invite.Email, TransactionalEmail.TeamInvite, new DTO.EMail.TeamInvite(invite.Id, team.Competition.Id, team.Competition.Name, team.Competition.Slug, team.Id, team.Name, team.Slug));
      await this.backgroundWorker.SendMessage(this.queues.Email, message);

      result.Succeed();
      return result;
    }

    public async Task<ServiceResult> ResendInvite(Guid inviteId)
    {
      var result = new ServiceResult();
      var invite = await this.context.TeamInvites.FirstOrDefaultAsync(i => i.Id == inviteId);
      if (invite == null)
      {
        result.Error = Shared.ErrorKey.Invite.NotFound;
        return result;
      }

      var team = await this.context.Teams.Include(t => t.Competition).FirstOrDefaultAsync(t => t.Id == invite.TeamId);
      var message = new DTO.EMail.Message<DTO.EMail.TeamInvite>(invite.Email, TransactionalEmail.TeamInvite, new DTO.EMail.TeamInvite(invite.Id, team.Competition.Id, team.Competition.Name, team.Competition.Slug, team.Id, team.Name, team.Slug));
      await this.backgroundWorker.SendMessage(this.queues.Email, message);

      // Do Send Stuff
      result.Succeed();
      return result;
    }

    public async Task<ServiceResult> RevokeInvite(Guid inviteId)
    {
      var result = new ServiceResult();
      var invite = await this.context.TeamInvites.FirstOrDefaultAsync(i => i.Id == inviteId);
      if (invite == null)
      {
        result.Error = Shared.ErrorKey.Invite.NotFound;
        return result;
      }

      await this.DangerouslyWithdrawInvite(invite);
      this.context.TeamInvites.Update(invite);

      result.Succeed();
      return result;
    }

    public async Task<ServiceResult> DisbandTeam(Guid teamId)
    {
      var result = new ServiceResult();

      var team = await this.context.Teams.Include(t => t.TeamMemberships)
                                         .ThenInclude(m => m.TeamRole)
                                         .FirstOrDefaultAsync(t => t.Id == teamId);

      if (team == null)
      {
        result.Error = Shared.ErrorKey.Team.TeamNotFound;
        return result;
      }

      team = await this.DangerouslyDisbandTeam(team);
      this.context.Teams.Update(team);

      result.Succeed();
      this.cacheBreaker.Break(Shared.Cache.Competition.Detail(team.CompetitionId));

      return result;
    }

    public async Task<ServiceResult> RemoveMember(Guid membershipId, string notes, bool forceRemove = false)
    {
      var result = new ServiceResult();

      var membership = await this.context.TeamMemberships.Include(m => m.Team).ThenInclude(t => t.TeamMemberships).ThenInclude(m => m.TeamRole).FirstOrDefaultAsync(m => m.Id == membershipId);
      if (membership == null)
      {
        result.Error = Shared.ErrorKey.TeamMembership.NotFound;
        return result;
      }
      var team = membership.Team;

      if (!forceRemove)
      {
        if (team.TeamMemberships.Count == 1)
        {
          result.Error = Shared.ErrorKey.TeamMembership.LastMember;
          return result;
        }

        if (membership.TeamRole.RemoveProtection)
        {
          result.Error = Shared.ErrorKey.TeamMembership.Protected;
          return result;
        }
      }

      var userId = membership.UserProfileId;
      await this.DangerouslyRemoveTeamMembership(team, userId, notes);

      this.context.Teams.Update(team);

      result.Succeed();

      return result;
    }

    public async Task<ServiceResult> AcceptInvite(Guid inviteId)
    {
      var result = new ServiceResult();

      var invite = await this.context.TeamInvites.Include(i => i.Team)
                                                 .ThenInclude(t => t.TeamMemberships)
                                                 .Include(i => i.UserProfile)
                                                 .ThenInclude(u => u.TeamMemberships)
                                                 .Include(i => i.Team)
                                                 .ThenInclude(t => t.Competition)
                                                 .FirstOrDefaultAsync(i => i.Id == inviteId);
      // Check if User is null;
      if (invite == null)
      {
        result.Error = Shared.ErrorKey.Invite.NotFound;
        return result;
      }

      // Check if the invite state is valid;
      if (!invite.CanBeRedeemed())
      {
        result.Error = Shared.ErrorKey.Invite.AlreadyUsed;
        return result;
      }

      var team = invite.Team;
      var user = invite.UserProfile;

      // If no direct user is found, lets see if somebody exists
      if (user == null)
      {
        user = await this.context.UserProfiles.Include(u => u.TeamMemberships).FirstOrDefaultAsync(u => u.Email == invite.Email);
      }

      // if no user is found, short circuit. 
      if (user == null)
      {
        result.Error = Shared.ErrorKey.Invite.InvalidUser;
        return result;
      }

      // Check if user is not in differemt team
      if (user.HasTeam(team.CompetitionId))
      {
        result.Error = Shared.ErrorKey.UserProfile.AlreadyInCompetitionTeam;
        return result;
      }

      // Check if Team exists
      if (team == null)
      {
        result.Error = Shared.ErrorKey.Team.TeamNotFound;
        return result;
      }

      // User os alreadu member of this team
      if (team.HasMember(user.Id))
      {
        result.Error = Shared.ErrorKey.Team.AlreadyInTeam;
        return result;
      }

      // Team is already disbanded
      if (team.TeamState == TeamState.Disbanded)
      {
        result.Error = Shared.ErrorKey.Team.Disbanded;
        return result;
      }

      var competition = invite.Team.Competition;
      if (competition == null)
      {
        result.Error = Shared.ErrorKey.Competition.NotFound;
        return result;
      }

      if (!team.HasOpenSpots())
      {
        result.Error = Shared.ErrorKey.Competition.MaxTeamSize;
        return result;
      }

      // Find the default role for a given competitoin
      var role = await this.context.TeamRoles.FirstOrDefaultAsync(r => r.Id == this.context.Competitions.FirstOrDefault(c => c.Id == team.CompetitionId).DefaultRoleId);
      if (role == null)
      {
        result.Error = Shared.ErrorKey.Competition.NoDefaultRoleSet;
        return result;
      }

      await this.DangerouslyCreateTeamMembership(team, user.Id, role.Id, "Invite Accepted");
      await this.DangerouslyAcceptTeamInvite(invite);

      this.context.Teams.Update(team);
      this.context.TeamInvites.Update(invite);

      result.Succeed();

      return result;
    }

    public async Task<ServiceResult> DeclineInvite(Guid inviteId)
    {
      var result = new ServiceResult();

      var invite = await this.context.TeamInvites.FirstOrDefaultAsync(i => i.Id == inviteId);
      // Check if User is null;
      if (invite == null)
      {
        result.Error = Shared.ErrorKey.Invite.NotFound;
        return result;
      }

      // Check if the invite state is valid;
      if (!invite.CanBeRedeemed())
      {
        result.Error = Shared.ErrorKey.Invite.AlreadyUsed;
        return result;
      }

      await this.DangerouslyDeclineTeamInvite(invite);

      result.Succeed();

      return result;
    }
    
    public async Task<ServiceResult> UpdateTeamLogo(Guid teamId, string logoURL)
    {
      var result = new ServiceResult();
      var team = await this.context.Teams.FirstOrDefaultAsync(t => t.Id == teamId);
      if (team == null)
      {
        result.Error = Shared.ErrorKey.Team.TeamNotFound;
        return result;
      }

      await DangerouslyUpdateTeamLogo(teamId, logoURL);

      result.Succeed();


      return result;
    }

    public async Task<ServiceResult<List<DTO.Team.Role>>> GetAvailableRoles(Guid teamId)
    {
      var result = new ServiceResult<List<DTO.Team.Role>>();

      var team = await this.context.Teams.Include(t => t.Competition)
        .ThenInclude(c => c.TeamRoles)
        .ThenInclude(tr => tr.Competition)
        .FirstOrDefaultAsync(t => t.Id == teamId);

      if (team == null)
      {
        result.Error = Shared.ErrorKey.Team.TeamNotFound;
        return result;
      }

      var roles = team.Competition.TeamRoles;
      var rolesDTO = roles.Select(AutoMapper.Mapper.Map<DTO.Team.Role>).ToList();

      result.Succeed(rolesDTO);
      return result;
    }

    private async Task<TeamInvite> DangerouslyCreateInvite(Guid teamId, string invitee)
    {
      var invite = new TeamInvite(teamId);

      if (EmailHelper.IsEmail(invitee))
      {
        invite.Email = invitee;
        if (userUtils.GetIdFromEmail(invitee, out var userId))
        {
          var user = await this.context.UserProfiles.Include(u => u.TeamInvites).FirstOrDefaultAsync(u => u.Id == userId);
          if (user.TeamInvites.Any(i => i.TeamId == teamId && i.State == InviteState.Pending))
          {
            return null;
          }

          invite.UserProfileId = userId;
          await this.authorizationService.AddPermission(userId, AuthorizationHelper.GenerateARN(typeof(TeamInvite), invite.Id.ToString(), Shared.Permissions.TeamInvite.All));

        }
      } else
      {
        var user = await this.context.UserProfiles.Include(u => u.TeamInvites).FirstOrDefaultAsync(u => u.UserName == invitee);
        if (user == null)
        {
          return null;
        }

        if (user.TeamInvites.Any(i => i.TeamId == teamId && i.State == InviteState.Pending))
        {
          return null;
        }

        invite.UserProfileId = user.Id;
        invite.Email = user.Email;
        await this.authorizationService.AddPermission(user.Id, AuthorizationHelper.GenerateARN(typeof(TeamInvite), invite.Id.ToString(), Shared.Permissions.TeamInvite.All));
      }

      return invite;
    }

    private async Task<Team> DangerouslyCreateTeam(Guid competitionId, DTO.Team.Create teamData, Guid ownerId)
    {
      var team = new Team(competitionId, teamData.Name, teamData.Abbreviation);
      if (teamData.Invites.Any())
      {
        foreach (var invitee in teamData.Invites)
        {
          var invite = await this.DangerouslyCreateInvite(team.Id, invitee);
          if (invite != null)
          {
            team.TeamInvites.Add(invite);
          }
        }
      }

      var competition = await this.context.Competitions.Include(c => c.TeamOwnerRole).FirstOrDefaultAsync(c => c.Id == competitionId);
      var creatorRole = competition?.TeamOwnerRole;
      if (creatorRole == null)
      {
        throw new NoNullAllowedException("Role cannot be null");
      }
      await this.backgroundWorker.SendMessage(this.queues.Contact, new DTO.Marketing.ContactSync(ownerId), 5);
      await this.DangerouslyCreateTeamMembership(team, ownerId, creatorRole.Id, "Created Team");

      return team;
    }

    private async Task<TeamMembership> DangerouslyCreateTeamMembership(Team team, Guid userId, Guid roleId, string notes)
    {
      var role = await context.TeamRoles.FirstOrDefaultAsync(tr => tr.Id == roleId);
      if (role == null)
      {
        throw new NoNullAllowedException("Role cannot be null");
      }

      var membership = team.AddMember(userId, roleId, notes);
      if (membership != null)
      {
        var permissions = role.Permissions.Select(p => AuthorizationHelper.GenerateARN(typeof(Team), team.Id.ToString(), p)).ToList();
        var membershipPermission = AuthorizationHelper.GenerateARN(typeof(TeamMembership), membership.Id.ToString(), Shared.Permissions.TeamMembership.All);

        await this.authorizationService.AddPermission(userId, membershipPermission);
        await this.authorizationService.AddPermission(userId, permissions);
        await this.backgroundWorker.SendMessage(this.queues.Contact, new DTO.Marketing.ContactSync(userId), 5);
      }

      var gameId = team.Competition?.GameId;
      if (!gameId.HasValue)
      {
        gameId = (await context.Competitions.Include(c => c.Game).FirstOrDefaultAsync(c => c.Id.Equals(team.CompetitionId)))?.GameId;
      }

      if (gameId.HasValue)
      {
        await this.userUtils.GenerateExternalUserName(userId, gameId.Value);
      }

      return membership;
    }

    private async Task<Team> DangerouslyDisbandTeam(Team team)
    {
      team.TeamState = TeamState.Disbanded;
      foreach (var member in team.TeamMemberships.ToList())
      {
        var membership = await this.DangerouslyRemoveTeamMembership(team, member.UserProfileId, "Disbanded");
        this.context.TeamMemberships.Remove(membership);
      }

      foreach (var invite in team.TeamInvites.Where(i => i.State == InviteState.Pending).ToList())
      {
        await this.DangerouslyWithdrawInvite(invite);
      }

      return team;
    }

    private async Task<TeamMembership> DangerouslyRemoveTeamMembership(Team team, Guid userId, string notes)
    {
      var membership = team.TeamMemberships.FirstOrDefault(m => m.UserProfileId == userId);
      team.RemoveMember(userId, notes);
      if (membership.TeamRole.Permissions.Any())
      {
        await this.authorizationService.RemovePermission(userId, membership.TeamRole.Permissions);
      }

      context.TeamMemberships.Remove(membership);
      var membershipPermission = AuthorizationHelper.GenerateARN(typeof(TeamMembership), membership.Id.ToString(), Shared.Permissions.TeamMembership.All);
      await this.authorizationService.RemovePermission(userId, membershipPermission);
      await this.backgroundWorker.SendMessage(this.queues.Contact, new DTO.Marketing.ContactSync(userId), 5);

      return membership;
    }

    private async Task<TeamInvite> DangerouslyAcceptTeamInvite(TeamInvite invite)
    {
      invite.Mark(InviteState.Accepted);
      if (invite.UserProfileId != null && invite.UserProfileId != Guid.Empty && invite.UserProfileId.HasValue)
      {
        await this.authorizationService.RemovePermission(invite.UserProfileId.Value, AuthorizationHelper.GenerateARN(typeof(TeamInvite), invite.Id.ToString(), Shared.Permissions.TeamInvite.All));
      }
      return invite;
    }

    private async Task<TeamInvite> DangerouslyDeclineTeamInvite(TeamInvite invite)
    {
      invite.Mark(InviteState.Declined);
      if (invite.UserProfileId != null && invite.UserProfileId != Guid.Empty && invite.UserProfileId.HasValue)
      {
        await this.authorizationService.RemovePermission(invite.UserProfileId.Value, AuthorizationHelper.GenerateARN(typeof(TeamInvite), invite.Id.ToString(), Shared.Permissions.TeamInvite.All));
      }
      return invite;
    }

    private async Task<TeamInvite> DangerouslyWithdrawInvite(TeamInvite invite)
    {
      invite.Mark(InviteState.Withdrawn);
      if (invite.UserProfileId != null && invite.UserProfileId != Guid.Empty && invite.UserProfileId.HasValue)
      {
        await this.authorizationService.RemovePermission(invite.UserProfileId.Value, AuthorizationHelper.GenerateARN(typeof(TeamInvite), invite.Id.ToString(), Shared.Permissions.TeamInvite.All));
      }
      return invite;
    }

    private async Task DangerouslyUpdateTeamLogo(Guid teamId, string logoURL)
    {
      var team = await context.Teams.FirstOrDefaultAsync(t => t.Id == teamId);
      if (team == null)
      {
        return;
      }

      team.LogoURL = logoURL;
    }
  }
}
