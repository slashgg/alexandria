using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alexandria.Infrastructure;
using Alexandria.Infrastructure.Filters;
using Alexandria.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Svalbard;

namespace Alexandria.Controllers.Team
{
  [Route("teams/{teamId}/match-series")]
  [ResourceSelectFilter("teamId")]
  [ApiController]
  public class TeamMatchSeriesController : ResourceBaseController
  {
    private readonly IMatchService matchService;

    public TeamMatchSeriesController(IMatchService matchService)
    {
      this.matchService = matchService;
    }

    /// <summary>
    /// Returns all pending matches for this team
    /// </summary>
    /// <returns></returns>
    [HttpGet("pending")]
    [ProducesResponseType(typeof(IList<DTO.MatchSeries.Detail>), 200)]
    [ProducesResponseType(typeof(void), 404)]
    public async Task<OperationResult<IList<DTO.MatchSeries.Detail>>> GetPendingMatches()
    {
      if (this.resourceId == Guid.Empty)
      {
        return new OperationResult<IList<DTO.MatchSeries.Detail>>(404);
      }

      var result = await this.matchService.GetPendingMatchesForTeam(this.resourceId);

      return new OperationResult<IList<DTO.MatchSeries.Detail>>(result.Data);
    }
  }
}
