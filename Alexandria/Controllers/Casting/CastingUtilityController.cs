using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alexandria.DTO.Competition;
using Alexandria.Interfaces.Services;
using Alexandria.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

    [HttpGet("competitions-available")]
    [Authorize]
    public async Task<OperationResult<IList<DTO.Competition.Info>>> GetAvailableCompetitions()
    {
      var userId = this.HttpContext.GetUserId();
      if (!userId.HasValue || userId == Guid.Empty)
      {
        return new OperationResult<IList<Info>>(401);
      }

      var result = await this.gameCastingService.GetCastableCompetitions(userId.Value);
      return new OperationResult<IList<Info>>(result.Data);
    }
  }
}
