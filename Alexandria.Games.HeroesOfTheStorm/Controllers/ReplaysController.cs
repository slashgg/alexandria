using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alexandria.Interfaces.Services;
using Alexandria.Shared.ErrorKey;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Svalbard;

namespace Alexandria.Games.HeroesOfTheStorm.Controllers
{
  [Route("heroes-of-the-storm/replays")]
  [ApiController]
  public class HeroesOfTheStormReplaysController : ControllerBase
  {
    private readonly string REPLAY_BUCKET = "slashgg.heroes-of-the-storm.replays";
    private readonly IFileService fileService;
    public HeroesOfTheStormReplaysController(IFileService fileService)
    {
      this.fileService = fileService;
    }

    /// <summary>
    /// Creates an upload URL for a replay file
    /// </summary>
    /// <param name="matchId"></param>
    /// <returns></returns>
    //[Authorize]
    [HttpGet("url")]
    [ProducesResponseType(typeof(Alexandria.DTO.Asset.PresignedURLResponse), 200)]
    [ProducesResponseType(typeof(void), 400)]
    [ProducesResponseType(typeof(BaseError), 400)]
    [ProducesResponseType(typeof(void), 401)]
    public async Task<OperationResult<Alexandria.DTO.Asset.PresignedURLResponse>> GetPresignedURL([FromQuery] Guid matchId)
    {
      var extension = "StormReplay";

      var result = await fileService.CreatePresignedUrl(REPLAY_BUCKET, $"{matchId}/{Guid.NewGuid().ToString("N")}.{extension}", "application/octet-stream");

      if (result.Success)
      {
        return new OperationResult<Alexandria.DTO.Asset.PresignedURLResponse>(result.Data);
      }

      return new OperationResult<Alexandria.DTO.Asset.PresignedURLResponse>(result.Error);
    }
  }
}
