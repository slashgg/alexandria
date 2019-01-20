using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alexandria.Infrastructure;
using Alexandria.Infrastructure.Filters;
using Alexandria.Interfaces.Services;
using Alexandria.Shared.ErrorKey;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Svalbard;

namespace Alexandria.Controllers.Team
{
  [Route("teams/{teamId}/invites")]
  [ResourceSelectFilter("teamId")]
  [ApiController]
  public class TeamInvitesController : ResourceBaseController
  {
    private readonly ITeamService teamService;

    public TeamInvitesController(ITeamService teamService)
    {
      this.teamService = teamService;
    }

    /// <summary>
    /// Get Invites for the given team
    /// Permissions Required: `team::{teamId}::invite--send`
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [PermissionsRequired("team::{teamId}::invite--send")]
    [ProducesResponseType(typeof(IList<DTO.Team.Invite>), 200)]
    [ProducesResponseType(typeof(void), 401)]
    public async Task<OperationResult<IList<DTO.Team.Invite>>> GetTeamInvites()
    {
      var result = await this.teamService.GetTeamInvites(this.resourceId);
      return new OperationResult<IList<DTO.Team.Invite>>(result.Data);
    }

    /// <summary>
    /// Send Invite to a User via Email or UserName
    /// </summary>
    /// <param name="teamId">GUID of the Team</param>
    /// <param name="payload">The qualifying Email or UserName ({displayName}#{number})</param>
    /// <returns></returns>
    [HttpPost]
    [PermissionsRequired("team::{teamId}::invite--send")]
    [ProducesResponseType(typeof(void), 201)]
    [ProducesResponseType(typeof(BaseError), 400)]
    [ProducesResponseType(typeof(void), 401)]
    [ProducesResponseType(typeof(BaseError), 409)]
    [ProducesResponseType(typeof(BaseError), 422)]
    public async Task<Svalbard.OperationResult> SendInvite([FromBody] DTO.Team.InviteRequest payload)
    {
      var result = await this.teamService.InviteMember(this.resourceId, payload.Invitee);
      if (result.Success)
      {
        return new Svalbard.OperationResult(201);
      }

      return new Svalbard.OperationResult(result.Error);
    }

    /// <summary>
    /// Resends an existing invite
    /// Required Permissions: `team::{teamId}::invite--send`
    /// </summary>
    /// <param name="teamId">GUID of the Team</param>
    /// <param name="inviteId">GUID of the Invite</param>
    /// <returns></returns>
    [HttpPost("{inviteId}")]
    [PermissionsRequired("team::{teamId}::invite--send")]
    [ProducesResponseType(typeof(void), 201)]
    [ProducesResponseType(typeof(BaseError), 400)]
    [ProducesResponseType(typeof(void), 401)]
    [ProducesResponseType(typeof(BaseError), 404)]
    public async Task<Svalbard.OperationResult> ResendInvite([FromRoute] Guid inviteId)
    {
      var result = await this.teamService.ResendInvite(inviteId);
      if (result.Success)
      {
        return new Svalbard.OperationResult(204);
      }
      return new Svalbard.OperationResult(result.Error);
    }

    /// <summary>
    /// Revokes an Invite
    /// Required permissions: `team::{teamId}::invite--revoke`
    /// </summary>
    /// <param name="teamId">GUID of the Team</param>
    /// <param name="inviteId">GUID of the invite</param>
    /// <returns></returns>
    [HttpDelete("{inviteId}")]
    [PermissionsRequired("team::{teamId}::invite--revoke")]
    [ProducesResponseType(typeof(void), 204)]
    [ProducesResponseType(typeof(BaseError), 400)]
    [ProducesResponseType(typeof(void), 401)]
    [ProducesResponseType(typeof(void), 404)]
    public async Task<Svalbard.OperationResult> RevokeInvite([FromRoute] Guid inviteId)
    {
      var result = await this.teamService.RevokeInvite(inviteId);
      if (result.Success)
      {
        return new Svalbard.OperationResult(204);
      }
      return new Svalbard.OperationResult(result.Error);
    }
  }
}
