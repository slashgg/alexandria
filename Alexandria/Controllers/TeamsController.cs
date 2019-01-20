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

    /// <summary>
    /// Get Team Details
    /// </summary>
    /// <param name="teamId">GUID of the team</param>
    /// <returns></returns>
    [HttpGet("{teamId}")]
    [ProducesResponseType(typeof(DTO.Team.Detail), 200)]
    [ProducesResponseType(typeof(BaseError), 400)]
    [ProducesResponseType(typeof(BaseError), 404)]
    public async Task<Svalbard.OperationResult> GetTeamDetail(Guid teamId)
    {
      var result = await this.teamService.GetTeamDetail(teamId);
      if (result.Success)
      {
        return new OperationResult<DTO.Team.Detail>(result.Data);
      }
      return new Svalbard.OperationResult(result.Error);
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
    [ProducesResponseType(typeof(BaseError), 404)]
    public async Task<Svalbard.OperationResult> DisbandTeam([FromRoute]Guid teamId)
    {
      var result = await this.teamService.DisbandTeam(teamId);
      if (result.Success)
      {
        return new Svalbard.OperationResult(204);
      }

      return new Svalbard.OperationResult(result.Error);
    }
  }
}
