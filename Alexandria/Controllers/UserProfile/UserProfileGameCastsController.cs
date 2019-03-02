using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alexandria.DTO.Casting;
using Alexandria.Interfaces.Services;
using Alexandria.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Svalbard;

namespace Alexandria.Controllers.UserProfile
{
  [Route("user-profiles/game-casts")]
  [ApiController]
  public class UserProfileGameCastsController : ControllerBase
  {
    private readonly IGameCastingService gameCastingService;

    public UserProfileGameCastsController(IGameCastingService gameCastingService)
    {
      this.gameCastingService = gameCastingService;
    }

    /// <summary>
    /// Gets the future schedule of the logged in caster
    /// </summary>
    /// <returns></returns>
    [ProducesResponseType(typeof(IList<DTO.Casting.Cast>), 200)]
    [ProducesResponseType(401)]
    [Authorize]
    [HttpGet]
    public async Task<OperationResult<IList<DTO.Casting.Cast>>> GetScheduledCasts()
    {
      var userId = this.HttpContext.GetUserId();
      if (!userId.HasValue || userId == Guid.Empty)
      {
        return new OperationResult<IList<DTO.Casting.Cast>>(401);
      }

      var result = await this.gameCastingService.GetScheduledCastsForUser(userId.Value);
      if (result.Success)
      {
        return new OperationResult<IList<Cast>>(result.Data);
      }

      return new OperationResult<IList<Cast>>(result.Error);
    }
  }
}
