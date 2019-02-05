using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alexandria.EF.Context;
using Alexandria.Interfaces.Processing;
using Alexandria.Interfaces.Services;
using Alexandria.Orchestration.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Svalbard.Services;

namespace Alexandria.Orchestration.Services
{
  public class MatchService : IMatchService
  {
    private IMemoryCache cache;
    private AlexandriaContext alexandriaContext;
    private ICacheBreaker cacheBreaker;
    private TournamentUtils tournamentUtils;

    public MatchService(AlexandriaContext alexandriaContext, IMemoryCache cache, ICacheBreaker cacheBreaker, TournamentUtils tournamentUtils)
    {
      this.alexandriaContext = alexandriaContext;
      this.cache = cache;
      this.cacheBreaker = cacheBreaker;
      this.tournamentUtils = tournamentUtils;
    }

    public async Task<ServiceResult<IList<DTO.MatchSeries.ScheduleRequest>>> GetPendingSchedulingRequests(Guid teamId)
    {
      var result = new ServiceResult<IList<DTO.MatchSeries.ScheduleRequest>>();
      var pendingRequests = await this.alexandriaContext.MatchSeriesScheduleRequests.Include(mssr => mssr.OriginTeam)
                                                                                    .Include(mssr => mssr.MatchSeries)
                                                                                    .ThenInclude(ms => ms.Matches)
                                                                                    .ThenInclude(m => m.Results)
                                                                                    .Include(mssr => mssr.MatchSeries)
                                                                                    .ThenInclude(ms => ms.MatchParticipants)
                                                                                    .ThenInclude(mp => mp.Team)
                                                                                    .Where(mssr => mssr.TargetTeamId == teamId)
                                                                                    .Where(mssr => mssr.State == Shared.Enums.ScheduleRequestState.Pending)
                                                                                    .ToListAsync();

      var dtos = pendingRequests.Select(AutoMapper.Mapper.Map<DTO.MatchSeries.ScheduleRequest>).ToList();
      result.Succeed(dtos);

      return result;
    }

    public async Task<ServiceResult> CreateScheduleRequest(Guid originTeamId, DTO.MatchSeries.CreateScheduleRequest payload)
    {
      var result = new ServiceResult();
      var originTeam = await this.alexandriaContext.Teams.Include(t => t.OriginatingScheduleRequests).FirstOrDefaultAsync(t => t.Id == originTeamId);
      if (originTeam == null)
      {
        result.Error = Shared.ErrorKey.Team.TeamNotFound;
        return result;
      }

      var targetTeamExists = await this.alexandriaContext.Teams.AnyAsync(t => t.Id == payload.TargetTeamId);
      if (!targetTeamExists)
      {
        result.Error = Shared.ErrorKey.Team.TeamNotFound;
        return result;
      }

      var timeSlotTaken = originTeam.OriginatingScheduleRequests.Any(osr => osr.State == Shared.Enums.ScheduleRequestState.Pending && osr.TargetTeamId == payload.TargetTeamId);
      if (timeSlotTaken)
      {
        result.Error = Shared.ErrorKey.ScheduleRequest.ScheduleRequestWithTimeslotAlreadyExists;
        return result;
      }

      if (payload.MatchSeriesId.HasValue && payload.MatchSeriesId.Value != Guid.Empty)
      {
        var match = await this.alexandriaContext.MatchSeries.FirstOrDefaultAsync(ms => ms.Id == payload.MatchSeriesId.Value);
        if (match == null)
        {
          result.Error = Shared.ErrorKey.MatchSeries.NotFound;
          return result;
        }

        if (match.Type != payload.MatchType)
        {
          result.Error = Shared.ErrorKey.ScheduleRequest.UnmatchingMatchTypes;
          return result;
        }
      }

      var scheduleRequest = this.DangerouslyCreateScheduleRequest(originTeamId, payload);

      if (scheduleRequest != null)
      {
        result.Succeed();
      }

      return result;
    }

    public async Task<ServiceResult> AcceptScheduleRequest(Guid scheduleRequestId)
    {
      var result = new ServiceResult();
      var scheduleRequest = await this.alexandriaContext.MatchSeriesScheduleRequests.Include(mssr => mssr.MatchSeries).FirstOrDefaultAsync(mssr => mssr.Id == scheduleRequestId);
      if (scheduleRequest == null)
      {
        result.Error = Shared.ErrorKey.ScheduleRequest.NotFound;
        return result;
      }

      await this.DangerouslyAcceptScheduleRequest(scheduleRequest);

      result.Succeed();
      return result;
    }

    public async Task<ServiceResult> DeclineScheduleRequest(Guid scheduleRequestId)
    {
      var result = new ServiceResult();
      var scheduleRequest = await this.alexandriaContext.MatchSeriesScheduleRequests.Include(mssr => mssr.MatchSeries).FirstOrDefaultAsync(mssr => mssr.Id == scheduleRequestId);
      if (scheduleRequest == null)
      {
        result.Error = Shared.ErrorKey.ScheduleRequest.NotFound;
        return result;
      }

      this.DangerouslyDeclineScheduleRequest(scheduleRequest);

      result.Succeed();
      return result;
    }


    private EF.Models.MatchSeriesScheduleRequest DangerouslyCreateScheduleRequest(Guid originTeamId, DTO.MatchSeries.CreateScheduleRequest payload)
    {
      var scheduleRequest = new EF.Models.MatchSeriesScheduleRequest(originTeamId, payload.TargetTeamId, payload.MatchType, payload.ProposedTimeSlot, payload.MatchSeriesId);

      this.alexandriaContext.MatchSeriesScheduleRequests.Add(scheduleRequest);

      return scheduleRequest;
    }

    private async Task DangerouslyAcceptScheduleRequest(EF.Models.MatchSeriesScheduleRequest scheduleRequest)
    {
      if (scheduleRequest.MatchSeries != null)
      {
        var matchSeries = scheduleRequest.MatchSeries;
        matchSeries.ScheduledAt = scheduleRequest.ProposedTimeSlot;
        alexandriaContext.MatchSeries.Update(matchSeries);
        if (matchSeries.TournamentRoundId.HasValue)
        {
          var tournamentId = (await alexandriaContext.Tournaments.FirstOrDefaultAsync(t => t.TournamentRounds.Any(tr => tr.Id == matchSeries.TournamentRoundId.Value)))?.Id;
          if (tournamentId.HasValue)
          {
            var tournaments = (await this.tournamentUtils.GetTournamentParents(tournamentId.Value)).Select(t => t.Id);

            foreach (var cacheBreakTournament in tournaments)
            {
              this.cacheBreaker.Break(Shared.Cache.Tournament.Schedule(cacheBreakTournament));
            }
          }
        }
      }
      else
      {
        throw new NotImplementedException("Creating a Match not yet supported");
      }

      scheduleRequest.Accept();

      alexandriaContext.MatchSeriesScheduleRequests.Update(scheduleRequest);
    }

    private void DangerouslyDeclineScheduleRequest(EF.Models.MatchSeriesScheduleRequest scheduleRequest)
    {
      scheduleRequest.Decline();
      alexandriaContext.MatchSeriesScheduleRequests.Update(scheduleRequest);
    }
  }
}
