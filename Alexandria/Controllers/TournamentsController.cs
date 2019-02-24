using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alexandria.Consumer.Shared.Infrastructure.Filters;
using Alexandria.Interfaces.Services;
using Alexandria.Shared.Enums;
using Alexandria.Shared.ErrorKey;
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

    /// <summary>
    /// Retrieves the played rounds of the tournament. NON RECURSIVE
    /// </summary>
    /// <param name="tournamentId"></param>
    /// <returns></returns>
    [HttpGet("{tournamentId}/rounds")]
    [ProducesResponseType(typeof(IList<DTO.Tournament.RoundDetail>), 200)]
    public async Task<OperationResult<IList<DTO.Tournament.RoundDetail>>> GetTournamentRounds(Guid tournamentId)
    {
      var result = await this.tournamentService.GetTournamentRounds(tournamentId);
      return new OperationResult<IList<DTO.Tournament.RoundDetail>>(result.Data);
    }

    /// <summary>
    /// Gets the schedule of the tournament. NON RECURSIVE
    /// </summary>
    /// <param name="tournamentId"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Gets the Round Robin Schedule for the Tournament
    /// </summary>
    /// <param name="type">round-robin</param>
    /// <param name="tournamentId"></param>
    /// <returns></returns>
    [HttpGet("{tournamentId}/standings")]
    [QueryStringConstraint("type", true, "round-robin")]
    [ProducesResponseType(typeof(DTO.Tournament.Standing<DTO.Tournament.RoundRobinRecord>), 200)]
    [ProducesResponseType(typeof(BaseError), 400)]
    [ProducesResponseType(typeof(BaseError), 404)]
    public async Task<OperationResult<DTO.Tournament.Standing<DTO.Tournament.RoundRobinRecord>>> GetRoundRobinResult([FromQuery] TournamentType type, [FromRoute] Guid tournamentId)
    {
      var result = await this.tournamentService.GetTournamentTable(tournamentId);
      if (result.Success)
      {
        return new OperationResult<DTO.Tournament.Standing<DTO.Tournament.RoundRobinRecord>>(result.Data);
      }

      return new OperationResult<DTO.Tournament.Standing<DTO.Tournament.RoundRobinRecord>>(result.Error);
    }
  }
}
