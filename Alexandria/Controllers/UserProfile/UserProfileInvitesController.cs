using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alexandria.Infrastructure.Filters;
using Alexandria.Interfaces.Services;
using Alexandria.Shared.ErrorKey;
using Alexandria.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Svalbard;

namespace Alexandria.Controllers.UserProfile
{
  [Route("user-profile/invites")]
  [ApiController]
  public class UserProfileInvitesController : ControllerBase
  {
    private readonly IUserProfileService userProfileService;

    public UserProfileInvitesController(IUserProfileService userProfileService)
    {
      this.userProfileService = userProfileService;
    }

    /// <summary>
    /// Get the Invites for the logged in user
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(List<DTO.UserProfile.TeamInvite>), 200)]
    [ProducesResponseType(typeof(void), 401)]
    public async Task<OperationResult<IList<DTO.UserProfile.TeamInvite>>> GetInvites()
    {
      var userId = HttpContext.GetUserId();
      if (userId == null)
      {
        return new OperationResult<IList<DTO.UserProfile.TeamInvite>>(404);
      }

      IList<string> hello = new List<string>();

      var result = await this.userProfileService.GetTeamInvites(userId.Value);

      return new OperationResult<IList<DTO.UserProfile.TeamInvite>>(result.Data);
    }

    /// <summary>
    /// Decline the targeted invite
    /// Reqyured Permissions: `invite::{inviteId}::handle`
    /// </summary>
    /// <param name="inviteId">GUID of the Invite</param>
    /// <returns></returns>
    [HttpDelete("{inviteId}")]
    [PermissionsRequired("invite::{inviteId}::handle")]
    [ProducesResponseType(typeof(void), 204)]
    [ProducesResponseType(typeof(BaseError), 400)]
    [ProducesResponseType(typeof(void), 401)]
    public async Task<OperationResult> DeclineInvite([FromRoute] Guid inviteId)
    {
      return new OperationResult(204);
    }

    /// <summary>
    /// Accept the targeted invite
    /// Reqyured Permissions: `invite::{inviteId}::handle`
    /// </summary>
    /// <param name="inviteId">GUID of the targeted invite</param>
    /// <returns></returns>
    [HttpPut("{inviteId}")]
    [PermissionsRequired("invite::{inviteId}::handle")]
    [ProducesResponseType(typeof(void), 204)]
    [ProducesResponseType(typeof(BaseError), 400)]
    [ProducesResponseType(typeof(void), 401)]
    public async Task<OperationResult> AcceptInvite([FromRoute] Guid inviteId)
    {
      return new OperationResult(204);
    }
  }
}
