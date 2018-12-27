using System;
using System.Threading.Tasks;
using Alexandria.Infrastructure;
using Alexandria.Infrastructure.Filters;
using Alexandria.Interfaces.Services;
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
    
    [HttpDelete("{membershipId}")]
    [PermissionsRequired("team::{teamId}::member--remove")]
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
