using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alexandria.Infrastructure;
using Alexandria.Infrastructure.Filters;
using Alexandria.Interfaces.Services;
using Alexandria.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Svalbard;

namespace Alexandria.Controllers.Competition
{
  [Route("competitions/{competitionId}/tournaments")]
  [ResourceSelectFilter("competitionId")]
  [ApiController]
  public class CompetitionTournamentsController : ResourceBaseController
  {
    private readonly ICompetitionService competitionService;
    private readonly ITournamentService tournamentService;


    public CompetitionTournamentsController(ICompetitionService competitionService, ITournamentService tournamentService)
    {
      this.competitionService = competitionService;
      this.tournamentService = tournamentService;
    }

    /// <summary>
    /// Get the tournaments for this competition
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<DTO.Competition.Tournament>), 200)]
    public async Task<OperationResult<IList<DTO.Competition.Tournament>>> GetTournamentsForCompetition()
    {
      if (this.resourceId != Guid.Empty)
      {
        var result = await this.competitionService.GetTournaments(this.resourceId);
        return new OperationResult<IList<DTO.Competition.Tournament>>(result.Data);
      } else
      {
        var result = await this.competitionService.GetTournaments(this.Slug);
        return new OperationResult<IList<DTO.Competition.Tournament>>(result.Data);
      }

    }

    /// <summary>
    /// Get the currently tournaments open for application for this competition
    /// </summary>
    /// <returns></returns>
    [HttpGet("appplications")]
    [ProducesResponseType(typeof(List<DTO.Competition.TournamentApplication>), 200)]
    public async Task<OperationResult<IList<DTO.Competition.TournamentApplication>>> GetOpenApplications()
    {
      if (this.resourceId != Guid.Empty)
      {
        var result = await this.tournamentService.GetOpenTournamentApplications(this.resourceId);
        return new OperationResult<IList<DTO.Competition.TournamentApplication>>(result.Data);
      } else
      {
        var result = await this.tournamentService.GetOpenTournamentApplications(this.Slug);
        return new OperationResult<IList<DTO.Competition.TournamentApplication>>(result.Data);
      }
    }

  }
}
