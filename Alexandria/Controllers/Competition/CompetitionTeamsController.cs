using System;
using System.Threading.Tasks;
using Alexandria.Interfaces.Services;
using Alexandria.Shared.ErrorKey;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Svalbard;

namespace Alexandria.Controllers.Competition
{
  [ApiExplorerSettings(GroupName = "Competition - Teams")]
  [Route("competitions/{competitionId}/teams")]
  [ApiController]
  [Authorize]
  public class CompetitionTeamsController : CompetitionBaseController
  {
    private readonly ITeamService teamService;

    public CompetitionTeamsController(ITeamService teamService)
    {
      this.teamService = teamService;
    }
    /// <summary>
    /// Create a Team for a Competition
    /// </summary>
    /// <param name="payload">Team Payload</param>
    [HttpPost]
    [ProducesResponseType(typeof(void), 201)]
    [ProducesResponseType(typeof(BaseError), 400)]
    public async Task<OperationResult> CreateTeam([FromBody] DTO.Team.Create payload)
    {
      var result = await this.teamService.CreateTeam(this.CompetitionId, payload);
      if (result.Success)
      {
        return Ok();
      }
      return new OperationResult(400, result.ErrorKey);
    }
  }
}
