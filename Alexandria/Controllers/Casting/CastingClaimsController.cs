using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alexandria.Consumer.Shared.Infrastructure.Filters;
using Alexandria.DTO.Casting;
using Alexandria.Interfaces.Services;
using Alexandria.Orchestration.Utils;
using Alexandria.Shared.ErrorKey;
using Alexandria.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Svalbard;

namespace Alexandria.Controllers.Casting
{
  [Route("casting/claims")]
  [ApiController]
  public class CastingClaimsController : ControllerBase
  {
    private readonly IGameCastingService gameCastingService;
    private readonly CastingUtils castingUtils;

    public CastingClaimsController(IGameCastingService gameCastingService, CastingUtils castingUtils)
    {
      this.gameCastingService = gameCastingService;
      this.castingUtils = castingUtils;
    }

    /// <summary>
    /// Gets all claims the user currently has on matches
    /// </summary>
    /// <returns></returns>
    [ProducesResponseType(typeof(IList<DTO.Casting.CastableMatchSeries>), 200)]
    [ProducesResponseType(401)]
    [HttpGet]
    [Authorize]
    public async Task<OperationResult<IList<DTO.Casting.CastableMatchSeries>>> GetClaimedMatches()
    {
      var userId = this.HttpContext.GetUserId();
      if (!userId.HasValue || userId == Guid.Empty)
      {
        return new OperationResult<IList<DTO.Casting.CastableMatchSeries>>();
      }

      var result = await this.gameCastingService.GetClaimedMatches(userId.Value);

      return new OperationResult<IList<CastableMatchSeries>>(result.Data);
    }

    /// <summary>
    /// Creates a claim for a match if required
    /// </summary>
    /// <param name="payload"></param>
    /// <returns></returns>
    [ProducesResponseType(201)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(typeof(Svalbard.Error), 404)]
    [ProducesResponseType(typeof(Svalbard.Error), 409)]
    [HttpPost]
    [Authorize]
    public async Task<OperationResult> ClaimMatchSeries([FromBody] DTO.Casting.ClaimMatchSeriesRequest payload)
    {
      var userId = this.HttpContext.GetUserId();
      if (!userId.HasValue || userId == Guid.Empty)
      {
        return new OperationResult();
      }

      if (!await this.castingUtils.CanCast(userId.Value, payload.MatchSeriesId))
      {
        return new OperationResult(403);
      }

      var result = await this.gameCastingService.ClaimGame(userId.Value, payload.MatchSeriesId);
      if (result.Success)
      {
        return new OperationResult(201);
      }

      return new OperationResult(result.Error);
    }

    /// <summary>
    /// Deletes an existing claim
    /// </summary>
    /// <param name="claimId"></param>
    /// <returns></returns>
    [ProducesResponseType(204)]
    [ProducesResponseType(401)]
    [ProducesResponseType(typeof(Svalbard.Error), 404)]
    [HttpDelete("{claimId}")]
    [PermissionsRequired("match-series-casting-claim::{claimId}::delete")]
    public async Task<OperationResult> RetractClaim([FromRoute] Guid claimId)
    {
      var userId = this.HttpContext.GetUserId();
      if (!userId.HasValue || userId == Guid.Empty)
      {
        return new OperationResult();
      }

      var result = await this.gameCastingService.RemoveClaim(claimId);
      if (result.Success)
      {
        return new OperationResult();
      }

      return new OperationResult(result.Error);
    }
  }
}
