using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alexandria.Orchestration.Services.Admin;
using Alexandria.Shared.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Svalbard;

namespace Alexandria.Admin.Controllers.Competitions
{
  [Route("competitions")]
  [ApiController]
  public class CompetitionsController : AdminController
  {
    private readonly AdminCompetitionService adminCompetitionService;

    public CompetitionsController(AdminCompetitionService adminCompetitionService)
    {
      this.adminCompetitionService = adminCompetitionService;
    }

    [ProducesResponseType(typeof(IList<DTO.Competition.Info>), 200)]
    [ProducesResponseType(401)]
    [HttpGet("accessible")]
    public async Task<OperationResult<IList<DTO.Competition.Info>>> GetCompetitionsForUser()
    {
      var result = await this.adminCompetitionService.GetCompetitionsAvailableToUser(HttpContext.GetUserId().Value);
      return new OperationResult<IList<DTO.Competition.Info>>(result.Data);
    }
  }
}
