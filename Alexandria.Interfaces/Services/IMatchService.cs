using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Svalbard.Services;

namespace Alexandria.Interfaces.Services
{
  public interface IMatchService
  {
    Task<ServiceResult<IList<DTO.MatchSeries.Detail>>> GetPendingMatchesForTeam(Guid teamId);
    Task<ServiceResult> CreateScheduleRequest(Guid originTeamId, DTO.MatchSeries.CreateScheduleRequest payload);
    Task<ServiceResult<DTO.MatchSeries.PendingScheduleRequests>> GetPendingSchedulingRequests(Guid teamId);
    Task<ServiceResult> AcceptScheduleRequest(Guid scheduleRequestId);
    Task<ServiceResult> DeclineScheduleRequest(Guid scheduleRequestId);
    Task<ServiceResult> RescindSCheduleRequest(Guid scheduleRequestId);
  }
}
