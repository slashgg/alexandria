using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alexandria.Shared.Infrastructure;
using Alexandria.Infrastructure.Filters;
using Alexandria.Interfaces.Services;
using Alexandria.Shared.ErrorKey;
using Microsoft.AspNetCore.Mvc;
using Svalbard;

namespace Alexandria.Controllers.Team
{
  [Route("teams/{teamId}/tournaments")]
  [ResourceSelectFilter("teamId")]
  [ApiController]
  public class TeamTournamentsController : ResourceBaseController
  {
    private readonly ITournamentService tournamentService;

    public TeamTournamentsController(ITournamentService tournamentService)
    {
      this.tournamentService = tournamentService;
    }

    /// <summary>
    /// Get all matches in a tournament for the team
    /// </summary>
    /// <param name="tournamentId"></param>
    /// <returns></returns>
    [HttpGet("{tournamentId}/matches")]
    [ProducesResponseType(typeof(IList<DTO.Tournament.MatchSeries>), 200)]
    [ProducesResponseType(typeof(BaseError), 404)]
    public async Task<OperationResult<IList<DTO.Tournament.MatchSeries>>> GetMatches(Guid tournamentId)
    {
      if (this.resourceId == Guid.Empty)
      {
        return new OperationResult<IList<DTO.Tournament.MatchSeries>>(404);
      }

      var result = await this.tournamentService.GetMatchesForTeamInTournament(this.resourceId, tournamentId);
      if (result.Success)
      {
        return new OperationResult<IList<DTO.Tournament.MatchSeries>>(result.Data);
      }

      return new OperationResult<IList<DTO.Tournament.MatchSeries>>(result.Error);
    }

  }
}
