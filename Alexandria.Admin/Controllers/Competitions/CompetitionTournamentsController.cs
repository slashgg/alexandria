using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alexandria.Orchestration.Services.Admin;
using Microsoft.AspNetCore.Mvc;
using Svalbard;

namespace Alexandria.Admin.Controllers.Competitions
{
  [Route("competitions/{competitionId}/tournaments")]
  [ApiController]
  public class CompetitionTournamentsController : AdminController
  {
    private readonly AdminCompetitionService adminCompetitionService;

    public CompetitionTournamentsController(AdminCompetitionService adminCompetitionService)
    {
      this.adminCompetitionService = adminCompetitionService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IList<DTO.Competition.Tournament>), 200)]
    [ProducesResponseType(401)]
    public async Task<OperationResult<IList<DTO.Competition.Tournament>>> GetTournaments([FromRoute] Guid competitionId, [FromQuery] DTO.Util.Pagination pagination)
    {
      var result = await adminCompetitionService.GetTournaments(competitionId, pagination.ZeroIndexPage, pagination.Limit);
      return new OperationResult<IList<DTO.Competition.Tournament>>(result);
    }
  }
}
