using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alexandria.Infrastructure;
using Alexandria.Infrastructure.Filters;
using Alexandria.Interfaces.Services;
using Alexandria.Shared.ErrorKey;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Svalbard;

namespace Alexandria.Controllers.Team
{
  [Route("teams/{teamId}/tournament-applications")]
  [ResourceSelectFilter("teamId")]
  [ApiController]
  public class TeamTournamentApplicationsController : ResourceBaseController
  {
    private readonly ITournamentService tournamentService;

    public TeamTournamentApplicationsController(ITournamentService tournamentService)
    {
      this.tournamentService = tournamentService;
    }

    /// <summary>
    /// Get a Teams Tournament Application
    /// Required Permissions: `team::{teamId}::tournament--join`
    /// </summary>
    /// <param name="tournamentId">Guid of the Tournament</param>
    /// <returns></returns>
    [HttpGet("{tournamentId}")]
    [PermissionsRequired("team::{teamId}::tournament--join")]
    [ProducesResponseType(typeof(DTO.Tournament.TournamentApplication), 200)]
    [ProducesResponseType(typeof(void), 401)]
    [ProducesResponseType(typeof(BaseError), 404)]
    public async Task<OperationResult<DTO.Tournament.TournamentApplication>> GetTournamentApplication([FromRoute] Guid tournamentId)
    {
      var result = await this.tournamentService.GetTeamApplication(tournamentId, this.resourceId);
      if (result.Success)
      {
        return new OperationResult<DTO.Tournament.TournamentApplication>(result.Data);
      }
      return new OperationResult<DTO.Tournament.TournamentApplication>(result.Error);
    }

    /// <summary>
    /// Get a Teams Tournament Application
    /// Required Permissions: `team::{teamId}::tournament--join`
    /// </summary>
    /// <param name="tournamentSlug">Slug of the tournament</param>
    /// <returns></returns>
    [HttpGet("by-slug/{tournamentSlug}")]
    [PermissionsRequired("team::{teamId}::tournament--join")]
    [ProducesResponseType(typeof(DTO.Tournament.TournamentApplication), 200)]
    [ProducesResponseType(typeof(void), 401)]
    [ProducesResponseType(typeof(BaseError), 404)]
    public async Task<OperationResult<DTO.Tournament.TournamentApplication>> GetTournamentApplication([FromRoute] string tournamentSlug)
    {
      var result = await this.tournamentService.GetTeamApplication(tournamentSlug, this.resourceId);
      if (result.Success)
      {
        return new OperationResult<DTO.Tournament.TournamentApplication>(result.Data);
      }
      return new OperationResult<DTO.Tournament.TournamentApplication>(result.Error);
    }

    /// <summary>
    /// Apply to a Tournament
    /// Required Permissions: `team::{teamId}::tournament--join`
    /// </summary>
    /// <param name="payload">Creation Object for Team Application</param>
    /// <returns></returns>
    [HttpPost]
    [PermissionsRequired("team::{teamId}::tournament--join")]
    [ProducesResponseType(typeof(void), 201)]
    [ProducesResponseType(typeof(BaseError), 400)]
    [ProducesResponseType(typeof(void), 401)]
    [ProducesResponseType(typeof(BaseError), 404)]
    [ProducesResponseType(typeof(BaseError), 423)]
    public async Task<OperationResult> ApplyToTournament([FromBody] DTO.Tournament.TeamTournamentApplicationRequest payload)
    {
      var result = await tournamentService.TeamApplyToTournament(this.resourceId, payload);
      if (result.Success)
      {
        return new OperationResult(201);
      }
      return new OperationResult(result.Error);
    }

    /// <summary>
    /// Withdraw a tournament application
    /// Required Permissions: `team::{teamId}::tournament--join`
    /// </summary>
    /// <param name="tournamentId">Guid of the tournament</param>
    /// <returns></returns>
    [HttpDelete("{tournamentId}")]
    [PermissionsRequired("team::{teamId}::tournament--join")]
    [ProducesResponseType(typeof(void), 204)]
    [ProducesResponseType(typeof(BaseError), 400)]
    [ProducesResponseType(typeof(void), 401)]
    [ProducesResponseType(typeof(BaseError), 404)]
    public async Task<OperationResult> WithdrawApplication([FromRoute] Guid tournamentId)
    {
      var result = await this.tournamentService.WithdrawTeamApplication(tournamentId, this.resourceId);
      if (result.Success)
      {
        return new OperationResult(204);
      }
      return new OperationResult(result.Error);
    }

    /// <summary>
    /// Withdraw a tournament application
    /// Required Permissions: `team::{teamId}::tournament--join`
    /// </summary>
    /// <param name="tournamentSlug">Slug of the tournament</param>
    /// <returns></returns>
    [HttpDelete("by-slug/{tournamentSlug}")]
    [PermissionsRequired("team::{teamId}::tournament--join")]
    [ProducesResponseType(typeof(void), 204)]
    [ProducesResponseType(typeof(BaseError), 400)]
    [ProducesResponseType(typeof(void), 401)]
    [ProducesResponseType(typeof(BaseError), 404)]
    public async Task<OperationResult> WithdrawApplication([FromRoute] string tournamentSlug)
    {
      var result = await this.tournamentService.WithdrawTeamApplication(tournamentSlug, this.resourceId);
      if (result.Success)
      {
        return new OperationResult(204);
      }
      return new OperationResult(result.Error);
    }
  }
}
