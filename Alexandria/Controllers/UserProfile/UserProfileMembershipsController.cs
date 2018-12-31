using System;
using System.Threading.Tasks;
using Alexandria.Infrastructure.Filters;
using Alexandria.Interfaces.Services;
using Alexandria.Shared.ErrorKey;
using Microsoft.AspNetCore.Mvc;
using Svalbard;

namespace Alexandria.Controllers.UserProfile
{
  [Route("user-profile/memberships")]
  [ApiController]
  public class UserProfileMembershipsController : ControllerBase
  {
    private readonly ITeamService teamService;

    public UserProfileMembershipsController(ITeamService teamService)
    {
      this.teamService = teamService;
    }

    [HttpDelete("{membershipId}")]
    [PermissionsRequired("team-membership::{membershipId}::handle")]
    [ProducesResponseType(typeof(void), 204)]
    [ProducesResponseType(typeof(BaseError), 400)]
    [ProducesResponseType(typeof(void), 401)]
    [ProducesResponseType(typeof(BaseError), 404)]
    [ProducesResponseType(typeof(BaseError), 422)]
    [ProducesResponseType(typeof(BaseError), 423)]
    public async Task<OperationResult> RemoveMembership([FromRoute] Guid membershipId)
    {
      var result = await this.teamService.RemoveMember(membershipId, "Left");

      if (result.Success)
      {
        return new OperationResult(204);
      }

      return new OperationResult(result.Error);
    }
  }
}
