using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Alexandria.EF.Context;
using Alexandria.EF.Models;
using Alexandria.Interfaces;
using Alexandria.Interfaces.Services;
using Alexandria.Shared.Enums;
using Alexandria.Shared.Extensions;
using Alexandria.Shared.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Svalbard.Services;

namespace Alexandria.Orchestration.Services
{
  public class TeamService : ITeamService
  {
    private readonly HttpContext httpContext;
    private readonly AlexandriaContext context;
    private readonly IUserUtils userUtils;
    private readonly IAuthorizationService authorizationService;

    public TeamService(IHttpContextAccessor httpContext, AlexandriaContext context, IUserUtils userUtils, IAuthorizationService authorizationService)
    {
      this.httpContext = httpContext.HttpContext;
      this.context = context;
      this.userUtils = userUtils;
      this.authorizationService = authorizationService;
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

      // Check if name is taken
      if (context.Teams.Any(t => string.Equals(teamData.Name, t.Name, StringComparison.CurrentCultureIgnoreCase) && t.CompetitionId == competitionId && t.TeamState != TeamState.Disbanded))
      {
        result.ErrorKey = Shared.ErrorKey.Team.NameTaken;
        return result;
      }

      // Check User Existence
      var user = await context.UserProfiles.Include(u => u.TeamMemberships).ThenInclude(m => m.Team).FirstOrDefaultAsync(u => u.Id == userId);
      if (user == null)
      {
        result.ErrorKey = Shared.ErrorKey.UserProfile.UserNotFound;
      }

      // Check if user is already in team for this competition
      if (user.HasTeam(competitionId))
      {
        result.ErrorKey = Shared.ErrorKey.Team.AlreadyInTeam;
        return result;
      }

      var team = await this.DangerouslyCreateTeam(competitionId, teamData, user.Id);

      this.context.Teams.Add(team);
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
        result.ErrorKey = Shared.ErrorKey.Team.TeamNotFound;
        return result;
      }

      team = await this.DangerouslyDisbandTeam(team);
      this.context.Teams.Update(team);

      result.Succeed();
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
        }
        return invite;
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

        return invite;
      }
    }

    private async Task<Team> DangerouslyCreateTeam(Guid competitionId, DTO.Team.Create teamData, Guid ownerId)
    {
      var team = new Team(competitionId, teamData.Name);
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

      var creatorRole = await this.context.TeamRoles.FirstOrDefaultAsync(r => r.CompetitionId == competitionId && r.Permissions.Count == 1 && r.Permissions.Contains("*"));
      if (creatorRole == null)
      {
        throw new NoNullAllowedException("Role cannot be null");
      }

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
        await this.authorizationService.AddPermission(userId, permissions);
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

      return membership;
    }
  }
}
