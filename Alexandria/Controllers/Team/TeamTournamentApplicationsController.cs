﻿using System;
using System.Threading.Tasks;
using Alexandria.Consumer.Shared.Infrastructure.Filters;
using Alexandria.Shared.Infrastructure;
using Alexandria.Infrastructure.Filters;
using Alexandria.Interfaces.Services;
using Alexandria.Shared.ErrorKey;
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
    /// <param name="teamId">Guid of the Team</param>
    /// <param name="tournamentId">Guid of the Tournament</param>
    /// <returns></returns>
    [HttpGet("{tournamentId}")]
    [PermissionsRequired("team::{teamId}::tournament--join")]
    [ProducesResponseType(typeof(DTO.Tournament.TournamentApplication), 200)]
    [ProducesResponseType(typeof(void), 401)]
    [ProducesResponseType(typeof(Svalbard.Error), 404)]
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
    /// <param name="teamId">Guid of the Team</param>
    /// <param name="tournamentSlug">Slug of the tournament</param>
    /// <returns></returns>
    [HttpGet("by-slug/{tournamentSlug}")]
    [PermissionsRequired("team::{teamId}::tournament--join")]
    [ProducesResponseType(typeof(DTO.Tournament.TournamentApplication), 200)]
    [ProducesResponseType(typeof(void), 401)]
    [ProducesResponseType(typeof(Svalbard.Error), 404)]
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
    /// <param name="teamId">GUID of the team</param>
    /// <param name="payload">Creation Object for Team Application</param>
    /// <returns></returns>
    [HttpPost]
    [PermissionsRequired("team::{teamId}::tournament--join")]
    [ProducesResponseType(typeof(void), 201)]
    [ProducesResponseType(typeof(Svalbard.Error), 400)]
    [ProducesResponseType(typeof(void), 401)]
    [ProducesResponseType(typeof(Svalbard.Error), 404)]
    [ProducesResponseType(typeof(Svalbard.Error), 423)]
    public async Task<Svalbard.OperationResult> ApplyToTournament([FromBody] DTO.Tournament.TeamTournamentApplicationRequest payload)
    {
      var result = await tournamentService.TeamApplyToTournament(this.resourceId, payload);
      if (result.Success)
      {
        return new Svalbard.OperationResult(201);
      }
      return new Svalbard.OperationResult(result.Error);
    }

    /// <summary>
    /// Withdraw a tournament application
    /// Required Permissions: `team::{teamId}::tournament--join`
    /// </summary>
    /// <param name="teamId">Guid of the team</param>
    /// <param name="tournamentId">Guid of the tournament</param>
    /// <returns></returns>
    [HttpDelete("{tournamentId}")]
    [PermissionsRequired("team::{teamId}::tournament--join")]
    [ProducesResponseType(typeof(void), 204)]
    [ProducesResponseType(typeof(Svalbard.Error), 400)]
    [ProducesResponseType(typeof(void), 401)]
    [ProducesResponseType(typeof(Svalbard.Error), 404)]
    public async Task<Svalbard.OperationResult> WithdrawApplication([FromRoute] Guid tournamentId)
    {
      var result = await this.tournamentService.WithdrawTeamApplication(tournamentId, this.resourceId);
      if (result.Success)
      {
        return new Svalbard.OperationResult(204);
      }
      return new Svalbard.OperationResult(result.Error);
    }

    /// <summary>
    /// Withdraw a tournament application
    /// Required Permissions: `team::{teamId}::tournament--join`
    /// </summary>
    /// <param name="teamId">Guid of the team</param>
    /// <param name="tournamentSlug">Slug of the tournament</param>
    /// <returns></returns>
    [HttpDelete("by-slug/{tournamentSlug}")]
    [PermissionsRequired("team::{teamId}::tournament--join")]
    [ProducesResponseType(typeof(void), 204)]
    [ProducesResponseType(typeof(Svalbard.Error), 400)]
    [ProducesResponseType(typeof(void), 401)]
    [ProducesResponseType(typeof(Svalbard.Error), 404)]
    public async Task<Svalbard.OperationResult> WithdrawApplication([FromRoute] string tournamentSlug)
    {
      var result = await this.tournamentService.WithdrawTeamApplication(tournamentSlug, this.resourceId);
      if (result.Success)
      {
        return new Svalbard.OperationResult(204);
      }
      return new Svalbard.OperationResult(result.Error);
    }
  }
}
