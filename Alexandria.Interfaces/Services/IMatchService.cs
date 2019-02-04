using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Svalbard.Services;

namespace Alexandria.Interfaces.Services
{
  public interface IMatchService
  {
    Task<ServiceResult> CreateScheduleRequest(Guid originTeamId, DTO.MatchSeries.CreateScheduleRequest payload);
    Task<ServiceResult<IList<DTO.MatchSeries.ScheduleRequest>>> GetPendingSchedulingRequests(Guid teamId);
    Task<ServiceResult> AcceptScheduleRequest(Guid scheduleRequestId);
    Task<ServiceResult> DeclineScheduleRequest(Guid scheduleRequestId);
  }
}
