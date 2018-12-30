using System;
using System.Threading.Tasks;
using Alexandria.Infrastructure;
using Alexandria.Infrastructure.Filters;
using Alexandria.Interfaces.Services;
using Alexandria.Shared.ErrorKey;
using Microsoft.AspNetCore.Mvc;
using Svalbard;

namespace Alexandria.Controllers.Team
{
  [Route("teams/{teamId}/memberships")]
  [ResourceSelectFilter("teamId")]
  [ApiController]
  public class TeamMembershipController : ResourceBaseController
  {
    private readonly ITeamService teamService;

    public TeamMembershipController(ITeamService teamService)
    {
      this.teamService = teamService;
    }

    /// <summary>
    /// Remove a Member from a Team
    /// </summary>
    /// <param name="teamId">GUID of the Team</param>
    /// <param name="membershipId">GUID of the Membership</param>
    /// <returns></returns>
    [HttpDelete("{membershipId}")]
    [PermissionsRequired("team::{teamId}::member--remove")]
    [ProducesResponseType(typeof(void), 204)]
    [ProducesResponseType(typeof(BaseError), 400)]
    [ProducesResponseType(typeof(void), 401)]
    public async Task<OperationResult> RemoveMember([FromRoute]Guid membershipId)
    {
      var result = await this.teamService.RemoveMember(membershipId, "Remove");
      if (result.Success)
      {
        return new NoContentResult();
      }

      return new OperationResult(result.ErrorKey);
    }
  }
}
