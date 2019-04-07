using System;
using System.Threading.Tasks;
using Alexandria.Games.SuperSmashBros.DTO.MatchSeries;
using Alexandria.Games.SuperSmashBros.Orchestration.Services;
using Alexandria.Infrastructure.Filters;
using Alexandria.Shared.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Svalbard;

namespace Alexandria.Games.SuperSmashBros.Controllers.MatchSeries
{
  [Route("super-smash-bros/match-series/{seriesId}/reporting")]
  [ResourceSelectFilter("seriesId")]
  [ApiExplorerSettings(GroupName = "Super Smash Bros - Match Reporting")]
  [ApiController]
  public class SuperSmashBrosMatchSeriesReportingController : ResourceBaseController
  {
    private readonly SuperSmashBrosMatchService superSmashBrosMatchService;

    public SuperSmashBrosMatchSeriesReportingController(SuperSmashBrosMatchService superSmashBrosMatchService)
    {
      this.superSmashBrosMatchService = superSmashBrosMatchService;
    }

    [HttpGet("meta")]
    [ProducesResponseType(typeof(DTO.MatchSeries.SuperSmashBrosMatchReportMetaData), 200)]
    public async Task<OperationResult<DTO.MatchSeries.SuperSmashBrosMatchReportMetaData>> GetMetaData()
    {
      if (this.resourceId == Guid.Empty)
      {
        return new OperationResult<SuperSmashBrosMatchReportMetaData>(404);
      }

      var result = await this.superSmashBrosMatchService.GetMatchResultMetaData(this.resourceId);
      return new OperationResult<SuperSmashBrosMatchReportMetaData>(result);
    }
  }
}
