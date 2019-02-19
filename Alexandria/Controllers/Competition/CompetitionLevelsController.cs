using System.Collections.Generic;
using System.Threading.Tasks;
using Alexandria.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Svalbard;

namespace Alexandria.Controllers.Competition
{
  [Route("competition-levels")]
  [ApiController]
  public class CompetitionLevelsController : ControllerBase
  {
    private readonly ICompetitionService competitionService;

    public CompetitionLevelsController(ICompetitionService competitionService)
    {
      this.competitionService = competitionService;
    }

    /// <summary>
    /// Get all Competition Levels
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(IList<DTO.Competition.CompetitionLevel>), 200)]
    public async Task<OperationResult<IList<DTO.Competition.CompetitionLevel>>> GetLevels()
    {
      var result = await this.competitionService.GetCompetitionLevels();
      return new OperationResult<IList<DTO.Competition.CompetitionLevel>>(result.Data);
    }
  }
}
