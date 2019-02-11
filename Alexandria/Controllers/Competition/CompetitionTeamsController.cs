using System.Threading.Tasks;
using Alexandria.Infrastructure.Filters;
using Alexandria.Interfaces.Services;
using Alexandria.Shared.ErrorKey;
using Alexandria.Shared.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Alexandria.Controllers.Competition
{
  [ApiExplorerSettings(GroupName = "Competition - Teams")]
  [Route("competitions/{competitionId}/teams")]
  [ResourceSelectFilter("competitionId")]
  [ApiController]
  [Authorize]
  public class CompetitionTeamsController : ResourceBaseController
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
    /// <param name="competitionId">GUID of the competition</param>
    [HttpPost]
    [ProducesResponseType(typeof(void), 201)]
    [ProducesResponseType(typeof(BaseError), 400)]
    [ProducesResponseType(typeof(BaseError), 404)]
    [ProducesResponseType(typeof(BaseError), 409)]
    public async Task<Svalbard.OperationResult> CreateTeam([FromBody] DTO.Team.Create payload)
    {
      var result = await this.teamService.CreateTeam(this.resourceId, payload);
      if (result.Success)
      {
        return Ok();
      }

      return new Svalbard.OperationResult(result.Error);
    }
  }
}
