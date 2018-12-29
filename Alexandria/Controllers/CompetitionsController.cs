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
  [Route("competitions")]
  [ApiController]
  public class CompetitionsController : ControllerBase
  {
    private readonly ICompetitionService competitionService;
    public CompetitionsController(ICompetitionService competitionService)
    {
      this.competitionService = competitionService;
    }

    /// <summary>
    /// Get all active Competitions
    /// </summary>
    /// <returns></returns>
    [HttpGet("active")]
    [ProducesResponseType(typeof(IList<DTO.Competition.Detail>), 204)]
    [ProducesResponseType(typeof(void), 400)]
    public async Task<OperationResult<IList<DTO.Competition.Detail>>> GetrActiveCompetitions()
    {
      var result = await this.competitionService.GetActiveCompetitions();
      if (result.Success)
      {
        return new OperationResult<IList<DTO.Competition.Detail>>(result.Data);
      }

      return new OperationResult<IList<DTO.Competition.Detail>>(400);
    }

    /// <summary>
    /// Get Competition by Id
    /// </summary>
    /// <param name="competitionId">GUID of the competition</param>
    /// <returns></returns>
    [HttpGet("{competitionId}")]
    [ProducesResponseType(typeof(DTO.Competition.Detail), 204)]
    [ProducesResponseType(typeof(BaseError), 400)]
    public async Task<OperationResult<DTO.Competition.Detail>> GetCompetitionDetail([FromRoute] Guid competitionId)
    {
      var result = await this.competitionService.GetCompetitionDetail(competitionId);
      if (result.Success)
      {
        return new OperationResult<DTO.Competition.Detail>(result.Data);
      }

      return new OperationResult<DTO.Competition.Detail>(result.ErrorKey);
    }

    /// <summary>
    /// Get competition by name
    /// This will automatically add the leading slash ('/')
    /// </summary>
    /// <param name="competitionName">Name without the leading slash (ex: `heroes-open` will become `/heroes-open`)</param>
    /// <returns></returns>
    [HttpGet("by-name/{competitionName}")]
    [ProducesResponseType(typeof(DTO.Competition.Detail), 204)]
    [ProducesResponseType(typeof(BaseError), 400)]
    public async Task<OperationResult<DTO.Competition.Detail>> GetCompetitionDetailByName([FromRoute] string competitionName)
    {
      var result = await this.competitionService.GetCompetitionByName(competitionName);
      if (result.Success)
      {
        return new OperationResult<DTO.Competition.Detail>(result.Data);
      }

      return new OperationResult<DTO.Competition.Detail>(result.ErrorKey);
    }
  }
}
