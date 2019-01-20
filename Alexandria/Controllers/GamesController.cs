using System.Collections.Generic;
using System.Threading.Tasks;
using Alexandria.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Svalbard;

namespace Alexandria.Controllers
{
  [Route("games")]
  [ApiController]
  public class GamesController : ControllerBase
  {
    private readonly ICompetitionService competitionService;

    public GamesController(ICompetitionService competitionService)
    {
      this.competitionService = competitionService;
    }

    /// <summary>
    /// Get all Games that have an active competition
    /// </summary>
    /// <returns></returns>
    [HttpGet("supported")]
    [ProducesResponseType(typeof(IList<DTO.Game.Detail>), 200)]
    public async Task<OperationResult<IList<DTO.Game.Detail>>> GetSupportedGames()
    {
      var result = await this.competitionService.GetSupportedGames();
      return new OperationResult<IList<DTO.Game.Detail>>(result.Data);
    }
  }
}
