using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alexandria.Interfaces.Services;
using Alexandria.Shared.ErrorKey;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Svalbard;

namespace Alexandria.Controllers
{
  [Route("tournaments")]
  [ApiController]
  public class TournamentsController : ControllerBase
  {
    private readonly ITournamentService tournamentService;
    public TournamentsController(ITournamentService tournamentService)
    {
      this.tournamentService = tournamentService;
    }

    /// <summary>
    /// Get the Tournament Detail including 4 levels of recursive child-tournaments
    /// </summary>
    /// <param name="tournamentId"></param>
    /// <returns></returns>
    [HttpGet("{tournamentId}")]
    [ProducesResponseType(typeof(DTO.Tournament.Detail), 200)]
    [ProducesResponseType(typeof(BaseError), 404)]
    public async Task<OperationResult<DTO.Tournament.Detail>> GetTournamentDetail(Guid tournamentId)
    {
      var result = await this.tournamentService.GetTournamentDetail(tournamentId);
      if (result.Success)
      {
        return new OperationResult<DTO.Tournament.Detail>(result.Data);
      }

      return new OperationResult<DTO.Tournament.Detail>(result.Error);
    }

    [HttpGet("{tournamentId}/rounds")]
    [ProducesResponseType(typeof(IList<DTO.Tournament.RoundDetail>), 200)]
    public async Task<OperationResult<IList<DTO.Tournament.RoundDetail>>> GetTournamentRounds(Guid tournamentId)
    {
      var result = await this.tournamentService.GetTournamentRounds(tournamentId);
      return new OperationResult<IList<DTO.Tournament.RoundDetail>>(result.Data);
    }

    [HttpGet("{tournamentId}/schedule")]
    [ProducesResponseType(typeof(DTO.Tournament.Schedule), 200)]
    [ProducesResponseType(typeof(BaseError), 404)]
    public async Task<OperationResult<DTO.Tournament.Schedule>> GetTournamentSchedule(Guid tournamentId)
    {
      var result = await tournamentService.GetSchedule(tournamentId);
      if (result.Success)
      {
        return new OperationResult<DTO.Tournament.Schedule>(result.Data);
      }

      return new OperationResult<DTO.Tournament.Schedule>(result.Error);
    }
  }
}
