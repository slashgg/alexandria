using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alexandria.Infrastructure.Filters;
using Alexandria.Interfaces.Services;
using Alexandria.Orchestration.Utils;
using Alexandria.Shared.ErrorKey;
using Alexandria.Shared.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Svalbard;

namespace Alexandria.Controllers.MatchSeries
{
  [Route("match-series/{matchSeriesId}/reporting")]
  [ResourceSelectFilter("matchSeriesId")]
  [ApiController]
  public class MatchSeriesReportingController : ResourceBaseController
  {
    private readonly IMatchService matchService;
    private readonly MatchSeriesUtils matchSeriesUtils;

    public MatchSeriesReportingController(IMatchService matchService, MatchSeriesUtils matchSeriesUtils)
    {
      this.matchService = matchService;
      this.matchSeriesUtils = matchSeriesUtils;
    }

    [HttpGet("meta")]
    [ProducesResponseType(typeof(DTO.MatchSeries.MatchReportMetaData), 200)]
    [ProducesResponseType(typeof(void), 301)]
    [ProducesResponseType(typeof(void), 404)]
    [ProducesResponseType(typeof(Svalbard.Error), 404)]
    public async Task<IActionResult> GetMatchReportingMeta()
    {
      if (this.resourceId == Guid.Empty)
      {
        return new OperationResult(404);
      }

      var redirectTo = await this.matchSeriesUtils.GetMatchSeriesMetaDataEndpointURL(this.resourceId);
      if (redirectTo != null)
      {
        return Redirect(redirectTo);
      }

      var result = await this.matchService.GetResultSubmitMetaData(this.resourceId);
      if (result.Success)
      {
        return Ok(result.Data);
      }

      return NotFound(result.Error);
    }
  }
}
