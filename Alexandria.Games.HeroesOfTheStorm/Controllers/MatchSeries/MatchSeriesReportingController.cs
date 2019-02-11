using System;
using System.Threading.Tasks;
using Alexandria.Games.HeroesOfTheStorm.Orchestration.Services;
using Alexandria.Infrastructure.Filters;
using Alexandria.Shared.ErrorKey;
using Alexandria.Shared.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Svalbard;

namespace Alexandria.Games.HeroesOfTheStorm.Controllers.MatchSeries
{
  [Route("heroes-of-the-storm/match-series/{matchId}/reporting")]
  [ResourceSelectFilter("matchId")]
  [ApiExplorerSettings(GroupName = "Heroes of the Storm - Match Reporting")]
  [ApiController]
  public class HeroesOfTheStormMatchSeriesReportingController : ResourceBaseController
  {
    private readonly HeroesOfTheStormMatchService matchService;

    public HeroesOfTheStormMatchSeriesReportingController(HeroesOfTheStormMatchService matchService)
    {
      this.matchService = matchService;
    }

    [HttpPost]
    public async Task<OperationResult> ReportMatch()
    {
      return Ok();
    }

    [HttpGet("meta")]
    [ProducesResponseType(typeof(DTO.MatchSeries.HeroesOfTheStormMatchReportMetaData), 200)]
    [ProducesResponseType(typeof(void), 404)]
    [ProducesResponseType(typeof(BaseError), 404)]
    public async Task<OperationResult<DTO.MatchSeries.HeroesOfTheStormMatchReportMetaData>> GetMatchSeriesResultMetaData()
    {
      if (this.resourceId == Guid.Empty)
      {
        return new OperationResult<DTO.MatchSeries.HeroesOfTheStormMatchReportMetaData>(404);
      }

      var result = await this.matchService.GetMatchResultMetaData(this.resourceId);
      if (result.Success)
      {
        return new OperationResult<DTO.MatchSeries.HeroesOfTheStormMatchReportMetaData>(result.Data);
      }

      return new OperationResult<DTO.MatchSeries.HeroesOfTheStormMatchReportMetaData>(result.Error);
    }
  }
}
