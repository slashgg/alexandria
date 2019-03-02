using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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
  [Route("casting/match-series")]
  [ApiController]
  public class MatchSeriesCastingsController : ControllerBase
  {
    private readonly IGameCastingService gameCastingService;
    private readonly CastingUtils castingUtils;
    public MatchSeriesCastingsController(IGameCastingService gameCastingService, CastingUtils castingUtils)
    {
      this.gameCastingService = gameCastingService;
      this.castingUtils = castingUtils;
    }

    /// <summary>
    /// Gets the list of available matches to cast per competition
    /// </summary>
    /// <param name="competitionId"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(IList<DTO.Casting.CastableMatchSeries>), 200)]
    [HttpGet]
    [Authorize]
    public async Task<OperationResult<IList<DTO.Casting.CastableMatchSeries>>> GetCastableMatcHSeries(
      [FromQuery] Guid competitionId)
    {
      var result = await this.gameCastingService.GetCastableMatchSeries(competitionId);
      if (result.Success)
      {
        return new OperationResult<IList<DTO.Casting.CastableMatchSeries>>(result.Data);
      }

      return new OperationResult<IList<DTO.Casting.CastableMatchSeries>>(result.Error);
    }

    /// <summary>
    /// Creates a new cast
    /// </summary>
    /// <param name="payload"></param>
    /// <returns></returns>
    [ProducesResponseType(201)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(typeof(BaseError), 409)]
    [HttpPost]
    [Authorize]
    public async Task<OperationResult> CreateGameCast([FromBody] DTO.Casting.CreateCastData payload)
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

      var result = await this.gameCastingService.CreateGameCast(userId.Value, payload);
      if (result.Success)
      {
        return new OperationResult(201);
      }

      return new OperationResult(result.Error);
    }

    /// <summary>
    /// Updates an existing claim
    /// </summary>
    /// <param name="castId"></param>
    /// <param name="payload"></param>
    /// <returns></returns>
    [ProducesResponseType(204)]
    [ProducesResponseType(401)]
    [ProducesResponseType(typeof(BaseError), 404)]
    [HttpPut("{castId}")]
    [PermissionsRequired("match-series-casting::{castId}::update")]
    [Authorize]
    public async Task<OperationResult> UpdateGameCast([FromRoute] Guid castId,
      [FromBody] DTO.Casting.UpdateGameCast payload)
    {
      var result = await this.gameCastingService.UpdateGameCast(castId, payload);
      if (result.Success)
      {
        return new OperationResult(204);
      }

      return new OperationResult(result.Error);
    }
  }
}
