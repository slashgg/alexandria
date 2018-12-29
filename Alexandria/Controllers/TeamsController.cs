using System;
using System.Threading.Tasks;
using Alexandria.Infrastructure.Filters;
using Alexandria.Interfaces.Services;
using Alexandria.Shared.ErrorKey;
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

    [HttpGet("{teamId}")]
    [ProducesResponseType(typeof(DTO.Team.Detail), 200)]
    [ProducesResponseType(typeof(BaseError), 400)]
    public async Task<OperationResult> GetTeamDetail(Guid teamId)
    {
      var result = await this.teamService.GetTeamDetail(teamId);
      if (result.Success)
      {
        return new OperationResult<DTO.Team.Detail>(result.Data);
      }
      return new OperationResult(result.ErrorKey);
    }

    /// <summary>
    /// Disbands the specified team
    /// Required Permissions: `team::{teamId}::disband`
    /// </summary>
    /// <param name="teamId">Targeted TeanUd</param>
    /// <returns></returns>
    [HttpDelete("{teamId}")]
    [PermissionsRequired("team::{teamId}::disband")]
    [ProducesResponseType(typeof(void), 204)]
    [ProducesResponseType(typeof(BaseError), 400)]
    public async Task<OperationResult> DisbandTeam([FromRoute]Guid teamId)
    {
      var result = await this.teamService.DisbandTeam(teamId);
      if (result.Success)
      {
        return new OperationResult(204);
      }

      return new OperationResult(result.ErrorKey);
    }
  }
}
