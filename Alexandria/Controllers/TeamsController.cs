using System;
using System.Threading.Tasks;
using Alexandria.Infrastructure.Filters;
using Alexandria.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Svalbard;

namespace Alexandria.Controllers
{
  [Route("teams")]
  [ApiController]
  public class TeamsController : ControllerBase
  {
    private readonly ITeamService teamService;

    public TeamsController(ITeamService teamService)
    {
      this.teamService = teamService;
    }

    /// <summary>
    /// Disbands the specified team
    /// Required Permissions: `team::{teamId}::disband`
    /// </summary>
    /// <param name="teamId">Targeted TeanUd</param>
    /// <returns></returns>
    [HttpDelete("{teamId}")]
    [PermissionsRequired("team::{teamId}::disband")]
    public async Task<OperationResult> DisbandTeam(Guid teamId)
    {
      var result = await this.teamService.DisbandTeam(teamId);
      if (result.Success)
      {
        return new OperationResult(200);
      }

      return new OperationResult(result.ErrorKey);
    }
  }
}
