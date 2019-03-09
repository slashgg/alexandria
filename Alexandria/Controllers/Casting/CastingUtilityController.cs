using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Alexandria.Interfaces.Services;
using Alexandria.Shared.ErrorKey;
using Alexandria.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Svalbard;

namespace Alexandria.Controllers.Casting
{
  [Route("casting")]
  [ApiController]
  public class CastingUtilityController : ControllerBase
  {
    private readonly IGameCastingService gameCastingService;

    public CastingUtilityController(IGameCastingService gameCastingService)
    {
      this.gameCastingService = gameCastingService;
    }

    /// <summary>
    /// Gets the MetaData for the Logged In User
    /// </summary>
    /// <returns></returns>
    [ProducesResponseType(typeof(DTO.Casting.CasterMetaData), 200)]
    [ProducesResponseType(typeof(void), 401)]
    [ProducesResponseType(typeof(Svalbard.Error), 404)]
    [HttpGet("caster-meta-data")]
    [Authorize]
    public async Task<OperationResult<DTO.Casting.CasterMetaData>> GetCasterMetaData()
    {
      var userId = this.HttpContext.GetUserId();
      if (!userId.HasValue || userId == Guid.Empty)
      {
        return new OperationResult<DTO.Casting.CasterMetaData>();
      }

      var result = await this.gameCastingService.GetCasterMetaData(userId.Value);
      if (result.Success)
      {
        return new OperationResult<DTO.Casting.CasterMetaData>(result.Data);
      }

      return new OperationResult<DTO.Casting.CasterMetaData>(result.Error);
    }

    /// <summary>
    /// Gets all the available competitions for the user
    /// </summary>
    /// <returns></returns>
    [ProducesResponseType(typeof(IList<DTO.Competition.Info>), 200)]
    [ProducesResponseType(401)]
    [HttpGet("competitions-available")]
    [Authorize]
    public async Task<OperationResult<IList<DTO.Competition.Info>>> GetAvailableCompetitions()
    {
      var userId = this.HttpContext.GetUserId();
      if (!userId.HasValue || userId == Guid.Empty)
      {
        return new OperationResult<IList<DTO.Competition.Info>>(401);
      }

      var result = await this.gameCastingService.GetCastableCompetitions(userId.Value);
      return new OperationResult<IList<DTO.Competition.Info>>(result.Data);
    }

  }
}
