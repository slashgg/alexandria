using System;
using System.Threading.Tasks;
using Alexandria.Consumer.Shared.Infrastructure.Filters;
using Alexandria.Interfaces.Services;
using Alexandria.Shared.ErrorKey;
using Microsoft.AspNetCore.Mvc;

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
    [ProducesResponseType(typeof(Svalbard.Error), 400)]
    [ProducesResponseType(typeof(void), 401)]
    [ProducesResponseType(typeof(Svalbard.Error), 404)]
    [ProducesResponseType(typeof(Svalbard.Error), 422)]
    [ProducesResponseType(typeof(Svalbard.Error), 423)]
    public async Task<Svalbard.OperationResult> RemoveMembership([FromRoute] Guid membershipId)
    {
      var result = await this.teamService.RemoveMember(membershipId, "Left");

      if (result.Success)
      {
        return new Svalbard.OperationResult(204);
      }

      return new Svalbard.OperationResult(result.Error);
    }
  }
}
