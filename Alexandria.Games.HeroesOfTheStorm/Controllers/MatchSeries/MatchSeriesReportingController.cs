using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alexandria.Games.HeroesOfTheStorm.Infrastructure;
using Alexandria.Games.HeroesOfTheStorm.Orchestration.Services;
using Alexandria.Infrastructure.Filters;
using Alexandria.Orchestration.Utils;
using Alexandria.Shared.ErrorKey;
using Alexandria.Shared.Extensions;
using Alexandria.Shared.Infrastructure;
using Microsoft.AspNetCore.Authorization;
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
    private readonly MatchSeriesUtils matchSeriesUtils;

    public HeroesOfTheStormMatchSeriesReportingController(HeroesOfTheStormMatchService matchService, MatchSeriesUtils matchSeriesUtils)
    {
      this.matchService = matchService;
      this.matchSeriesUtils = matchSeriesUtils;
    }

    [ServiceFilter(typeof(HeroesOfTheStormSaveChangesFilter))]
    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(void), 401)]
    [ProducesResponseType(typeof(void), 403)]
    [ProducesResponseType(typeof(void), 204)]
    public async Task<OperationResult> ReportMatch([FromBody] DTO.MatchSeries.MatchSeriesResultReportingRequest payload)
    {
      var userId = this.HttpContext.GetUserId();
      if (!userId.HasValue || userId.Value == Guid.Empty)
      {
        return new OperationResult(401);
      }

      if (!await this.matchSeriesUtils.CanReport(this.resourceId, userId.Value))
      {
        return new OperationResult(403);
      }

      var result = await this.matchService.ReportMatchSeries(this.resourceId, payload);
      if (result.Success)
      {
        return new OperationResult(204);
      }

      return new OperationResult(result.Error);
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
