using System;
using System.Collections.Generic;
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
  [Route("teams/{teamId}/schedule-request")]
  [ResourceSelectFilter("teamId")]
  [ApiController]
  public class TeamScheduleRequestController : ResourceBaseController
  {
    private readonly IMatchService matchService;

    public TeamScheduleRequestController(IMatchService matchService)
    {
      this.matchService = matchService;
    }

    /// <summary>
    /// Gets the teams schedule requests
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [PermissionsRequired("team::{teamId}::match--schedule")]
    [ProducesResponseType(typeof(DTO.MatchSeries.PendingScheduleRequests), 200)]
    [ProducesResponseType(typeof(Svalbard.Error), 404)]
    public async Task<OperationResult<DTO.MatchSeries.PendingScheduleRequests>> GetPendingScheduleRequests()
    {
      if (this.resourceId == Guid.Empty)
      {
        return new OperationResult<DTO.MatchSeries.PendingScheduleRequests>(404);
      }

      var result = await this.matchService.GetPendingSchedulingRequests(this.resourceId);

      if (result.Success)
      {
        return new OperationResult<DTO.MatchSeries.PendingScheduleRequests>(result.Data);
      }

      return new OperationResult<DTO.MatchSeries.PendingScheduleRequests>(result.Error);
    }

    /// <summary>
    /// Submits a schedule requests for a match
    /// </summary>
    /// <param name="payload"></param>
    /// <returns></returns>
    [HttpPost]
    [PermissionsRequired("team::{teamId}::match--schedule")]
    [ProducesResponseType(typeof(void), 201)]
    [ProducesResponseType(typeof(void), 401)]
    [ProducesResponseType(typeof(Svalbard.Error), 404)]
    [ProducesResponseType(typeof(Svalbard.Error), 409)]
    public async Task<OperationResult> CreateScheduleRequest(DTO.MatchSeries.CreateScheduleRequest payload)
    {
      if (this.resourceId == Guid.Empty)
      {
        return new OperationResult<IList<DTO.MatchSeries.ScheduleRequest>>(404);
      }

      var result = await this.matchService.CreateScheduleRequest(this.resourceId, payload);
      if (result.Success)
      {
        return new OperationResult(201);
      }

      return new OperationResult(result.Error);
    }

    /// <summary>
    /// Accepts the specified schedule request
    /// </summary>
    /// <param name="scheduleRequestId"></param>
    /// <returns></returns>
    [HttpPut("{scheduleRequestId}")]
    [PermissionsRequired("team::{teamId}::match--schedule")]
    [ProducesResponseType(typeof(void), 204)] 
    [ProducesResponseType(typeof(void), 401)]
    [ProducesResponseType(typeof(Svalbard.Error), 404)]
    public async Task<OperationResult> AcceptScheduleRequest(Guid scheduleRequestId)
    {
      var result = await this.matchService.AcceptScheduleRequest(scheduleRequestId);
      if (result.Success)
      {
        return new OperationResult(204);
      }

      return new OperationResult(result.Error);
    }

    /// <summary>
    /// Decline the schedule request
    /// </summary>
    /// <param name="scheduleRequestId"></param>
    /// <returns></returns>
    [HttpDelete("{scheduleRequestId}")]
    [PermissionsRequired("team::{teamId}::match--schedule")]
    [ProducesResponseType(typeof(void), 204)]
    [ProducesResponseType(typeof(void), 401)]
    [ProducesResponseType(typeof(Svalbard.Error), 404)]
    public async Task<OperationResult> DeclineScheduleRequest(Guid scheduleRequestId)
    {
      var result = await this.matchService.DeclineScheduleRequest(scheduleRequestId);
      if (result.Success)
      {
        return new OperationResult(204);
      }

      return new OperationResult(result.Error);
    }

    /// <summary>
    /// Rescind the sent schedule request
    /// </summary>
    /// <param name="scheduleRequestId"></param>
    /// <returns></returns>
    [HttpDelete("{scheduleRequestId}/rescind")]
    [PermissionsRequired("team::{teamId}::match--schedule")]
    [ProducesResponseType(typeof(void), 204)]
    [ProducesResponseType(typeof(void), 401)]
    [ProducesResponseType(typeof(Svalbard.Error), 404)]
    public async Task<OperationResult> RescindSCheduleRequest(Guid scheduleRequestId)
    {
      var result = await this.matchService.RescindSCheduleRequest(scheduleRequestId);
      if (result.Success)
      {
        return new OperationResult(204);
      }

      return new OperationResult(result.Error);
    }
  }
}
