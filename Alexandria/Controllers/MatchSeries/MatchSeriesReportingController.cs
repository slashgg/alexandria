using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alexandria.Consumer.Shared.Infrastructure.Filters;
using Alexandria.Infrastructure.Filters;
using Alexandria.Interfaces.Services;
using Alexandria.Orchestration.Utils;
using Alexandria.Shared.ErrorKey;
using Alexandria.Shared.Extensions;
using Alexandria.Shared.Infrastructure;
using Microsoft.AspNetCore.Authorization;
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

    /// <summary>
    /// Get Meta Information for Match Reporting
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Generic Match Reporting Endpoint
    /// </summary>
    /// <param name="payload"></param>
    /// <returns></returns>
    [Authorize]
    [QueryStringConstraint("type", true, "versus")]
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    public async Task<OperationResult> ReportMatch([FromBody] DTO.MatchSeries.MatchSeriesResultReport payload)
    {
      if (this.resourceId == Guid.Empty)
      {
        return new OperationResult(404);
      }

      var userId = this.HttpContext.GetUserId();
      if (!userId.HasValue)
      {
        return new OperationResult(401);
      }

      //if (!await this.matchSeriesUtils.CanReport(this.resourceId, userId.Value))
      //{
      //  return new OperationResult(403);
      //}

      var result = await this.matchService.ReportMatchSeriesResult(this.resourceId, payload.MatchResults);

      return new OperationResult(result);
    }
  }
}
