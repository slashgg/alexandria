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

namespace Alexandria.Controllers.Tournament
{
  [Route("tournaments/{tournamentId}/participations")]
  [ResourceSelectFilter("tournamentId")]
  [ApiController]
  public class TournamentParticipationsController : ResourceBaseController
  {
    private readonly ITournamentService tournamentService;

    public TournamentParticipationsController(ITournamentService tournamentService)
    {
      this.tournamentService = tournamentService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<DTO.Tournament.TeamParticipation>), 200)]
    public async Task<OperationResult<List<DTO.Tournament.TeamParticipation>>> GetTournamentParticipations()
    {
      if (this.resourceId != Guid.Empty)
      {
        var result = await tournamentService.GetTeamParticipations(this.resourceId);
        if (result.Success)
        {
          return new OperationResult<List<DTO.Tournament.TeamParticipation>>(result.Data);
        }
        return new OperationResult<List<DTO.Tournament.TeamParticipation>>(result.Error);
      } else
      {
        var result = await this.tournamentService.GetTeamParticipations(this.Slug);
        if (result.Success)
        {
          return new OperationResult<List<DTO.Tournament.TeamParticipation>>(result.Data);
        }
        return new OperationResult<List<DTO.Tournament.TeamParticipation>>(result.Error);
      }
    }
  }
}
