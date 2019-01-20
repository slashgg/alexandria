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
    /// Search Competitions by parameters
    /// </summary>
    /// <param name="lookup">Lookup Object in Query</param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(IList<DTO.Competition.Detail>), 200)]
    public async Task<OperationResult<IList<DTO.Competition.Detail>>> Lookup([FromQuery] DTO.Competition.Lookup lookup)
    {
      var result = await this.competitionService.SearchCompetitions(lookup);
      return new OperationResult<IList<DTO.Competition.Detail>>(result.Data);
    }

    /// <summary>
    /// Get all active Competitions
    /// </summary>
    /// <returns></returns>
    [HttpGet("active")]
    [ProducesResponseType(typeof(IList<DTO.Competition.Detail>), 204)]
    [ProducesResponseType(typeof(void), 400)]
    public async Task<OperationResult<IList<DTO.Competition.Detail>>> GetActiveCompetitions()
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

      return new OperationResult<DTO.Competition.Detail>(result.Error);
    }

    /// <summary>
    /// Get competition by slug
    /// </summary>
    /// <param name="competitionSlug">Slug of the competiiton</param>
    /// <returns></returns>
    [HttpGet("by-slug/{competitionSlug}")]
    [ProducesResponseType(typeof(DTO.Competition.Detail), 204)]
    [ProducesResponseType(typeof(BaseError), 400)]
    public async Task<OperationResult<DTO.Competition.Detail>> GetCompetitionDetailBySlug([FromRoute] string competitionSlug)
    {
      var result = await this.competitionService.GetCompetitionDetail(competitionSlug);
      if (result.Success)
      {
        
        return new OperationResult<DTO.Competition.Detail>(result.Data);
      }

      return new OperationResult<DTO.Competition.Detail>(result.Error);
    }
  }
}
