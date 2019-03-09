using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alexandria.DTO.Competition;
using Alexandria.Interfaces.Services;
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
    private readonly ICompetitionService competitionService;

    public CompetitionsController(AdminCompetitionService adminCompetitionService, ICompetitionService competitionService)
    {
      this.adminCompetitionService = adminCompetitionService;
      this.competitionService = competitionService;
    }

    [ProducesResponseType(typeof(IList<DTO.Competition.Info>), 200)]
    [ProducesResponseType(401)]
    [HttpGet("accessible")]
    public async Task<OperationResult<IList<DTO.Competition.Info>>> GetCompetitionsForUser()
    {
      var result = await this.adminCompetitionService.GetCompetitionsAvailableToUser(HttpContext.GetUserId().Value);
      return new OperationResult<IList<DTO.Competition.Info>>(result);
    }

    [ProducesResponseType(typeof(IList<DTO.Competition.CompetitionLevel>), 200)]
    [ProducesResponseType(401)]
    [HttpGet("levels")]
    public async Task<OperationResult<IList<DTO.Competition.CompetitionLevel>>> GetCompetitionLevels()
    {
      var result = await this.competitionService.GetCompetitionLevels();
      return new OperationResult<IList<CompetitionLevel>>(result);
    }

    [ProducesResponseType(201)]
    [ProducesResponseType(401)]
    [HttpPost]
    public async Task<OperationResult> CreateCompetition([FromBody] DTO.Admin.Competition.CreateData payload)
    {
      var result = await this.adminCompetitionService.CreateCompetition(payload);
      return new OperationResult(result);
    }
  }
}
