using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alexandria.EF.Context;
using Alexandria.EF.Models;
using Alexandria.Interfaces.Processing;
using Alexandria.Interfaces.Services;
using Alexandria.Orchestration.Utils;
using Alexandria.Shared.Enums;
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

    public async Task<ServiceResult<IList<DTO.MatchSeries.Detail>>> GetPendingMatchesForTeam(Guid teamId)
    {
      var result = new ServiceResult<IList<DTO.MatchSeries.Detail>>();
      var matches = await this.alexandriaContext.MatchSeries.Include(ms => ms.Matches)
                                                            .ThenInclude(m => m.Results)
                                                            .Include(m => m.Game)
                                                            .Include(ms => ms.MatchParticipants)
                                                            .ThenInclude(mp => mp.Team)
                                                            .Where(mssr => mssr.State == Shared.Enums.MatchState.Pending)
                                                            .Where(mssr => mssr.MatchParticipants.Any(mp => mp.TeamId.Equals(teamId)))
                                                            .ToListAsync();

      var matchDTOs = matches.Select(AutoMapper.Mapper.Map<DTO.MatchSeries.Detail>).ToList();
      result.Succeed(matchDTOs);

      return result;
    }

    public async Task<ServiceResult<DTO.MatchSeries.PendingScheduleRequests>> GetPendingSchedulingRequests(Guid teamId)
    {
      var result = new ServiceResult<DTO.MatchSeries.PendingScheduleRequests>();
      var pendingTargetRequests = await this.alexandriaContext.MatchSeriesScheduleRequests.Include(mssr => mssr.OriginTeam)
                                                                                    .Include(mssr => mssr.TargetTeam)
                                                                                    .Include(mssr => mssr.MatchSeries)
                                                                                    .ThenInclude(ms => ms.Matches)
                                                                                    .ThenInclude(m => m.Results)
                                                                                    .Include(mssr => mssr.MatchSeries)
                                                                                    .ThenInclude(ms => ms.MatchParticipants)
                                                                                    .ThenInclude(mp => mp.Team)
                                                                                    .Where(mssr => mssr.TargetTeamId == teamId)
                                                                                    .Where(mssr => mssr.State == Shared.Enums.ScheduleRequestState.Pending)
                                                                                    .ToListAsync();

      var pendingOriginRequests = await this.alexandriaContext.MatchSeriesScheduleRequests.Include(mssr => mssr.OriginTeam)
                                                                                    .Include(mssr => mssr.TargetTeam)
                                                                                    .Include(mssr => mssr.MatchSeries)
                                                                                    .ThenInclude(ms => ms.Matches)
                                                                                    .ThenInclude(m => m.Results)
                                                                                    .Include(mssr => mssr.MatchSeries)
                                                                                    .ThenInclude(ms => ms.MatchParticipants)
                                                                                    .ThenInclude(mp => mp.Team)
                                                                                    .Where(mssr => mssr.OriginTeamId == teamId)
                                                                                    .Where(mssr => mssr.State == Shared.Enums.ScheduleRequestState.Pending)
                                                                                    .ToListAsync();

      var targetDTOs = pendingTargetRequests.Select(AutoMapper.Mapper.Map<DTO.MatchSeries.ScheduleRequest>).ToList();
      var originDTOs = pendingOriginRequests.Select(AutoMapper.Mapper.Map<DTO.MatchSeries.ScheduleRequest>).ToList();

      result.Succeed(new DTO.MatchSeries.PendingScheduleRequests(originDTOs, targetDTOs));

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

      var timeSlotTaken = originTeam.OriginatingScheduleRequests.Any(osr => osr.State == Shared.Enums.ScheduleRequestState.Pending && osr.MatchSeriesId == payload.MatchSeriesId);
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

    public async Task<ServiceResult<DTO.MatchSeries.MatchReportMetaData>> GetResultSubmitMetaData(Guid matchSeriesId)
    {
      var result = new ServiceResult<DTO.MatchSeries.MatchReportMetaData>();
      var matchSeries = await this.alexandriaContext.MatchSeries.Include(ms => ms.MatchParticipants)
                                                                .ThenInclude(mp => mp.Team)
                                                                .Include(ms => ms.Matches)
                                                                .Include(ms => ms.Game)
                                                                .Include(ms => ms.TournamentRound)
                                                                .FirstOrDefaultAsync(ms => ms.Id == matchSeriesId);
      if (matchSeries == null)
      {
        result.Error = Shared.ErrorKey.MatchSeries.NotFound;
        return result;
      }

      var resultMetaData = AutoMapper.Mapper.Map<DTO.MatchSeries.MatchReportMetaData>(matchSeries);
      result.Succeed(resultMetaData);

      return result;
    }

    public async Task<ServiceResult> RescindSCheduleRequest(Guid scheduleRequestId)
    {
      var result = new ServiceResult();
      var scheduleRequest = await this.alexandriaContext.MatchSeriesScheduleRequests.Include(mssr => mssr.MatchSeries).FirstOrDefaultAsync(mssr => mssr.Id == scheduleRequestId);
      if (scheduleRequest == null)
      {
        result.Error = Shared.ErrorKey.ScheduleRequest.NotFound;
        return result;
      }

      this.DangerouslyRescindScheduleRequest(scheduleRequest);

      result.Succeed();
      return result;
    }

    public async Task<ServiceResult> ReportMatchSeriesResult(Guid matchSeriesId, IEnumerable<DTO.MatchSeries.MatchResultReport> results)
    {
      var result = new ServiceResult();

      var matchSeries = await this.alexandriaContext.MatchSeries.Include(ms => ms.Matches)
                                                                .Include(ms => ms.MatchParticipants)
                                                                .ThenInclude(mp => mp.Team)
                                                                .FirstOrDefaultAsync(ms => ms.Id.Equals(matchSeriesId));
      if (matchSeries == null)
      {
        result.Error = Shared.ErrorKey.MatchSeries.NotFound;
        return result;
      }

      if (matchSeries.State != Shared.Enums.MatchState.Pending)
      {
        result.Error = Shared.ErrorKey.MatchSeries.AlreadyReported;
        return result;
      }


      var matchOrder = 1;
      foreach (var matchResult in results)
      {
        ServiceResult reportingServiceResult = null;
        if (matchResult.MatchId.HasValue)
        {
          reportingServiceResult = await this.ReportMatchResult(matchResult);
        }
        else
        {
          reportingServiceResult = await this.CreateAndReportMatchResult(matchResult, matchSeriesId, matchOrder);
        }


        if (reportingServiceResult.Error != null)
        {
          result.Error = reportingServiceResult.Error;
          return result;
        }

        matchOrder++;
      }

      this.DangerouslyReportMatchSeries(matchSeries, MatchState.Complete);
      result.Succeed();

      return result;
    }

    public async Task<ServiceResult> ReportMatchResult(DTO.MatchSeries.MatchResultReport matchResult)
    {
      var result = new ServiceResult();
      if (!matchResult.MatchId.HasValue)
      {
        result.Error = Shared.ErrorKey.Match.NotFound;
        return result;
      }

      var match = await this.alexandriaContext.Matches
                                              .Include(m => m.Results)
                                              .Include(m => m.MatchSeries)
                                              .ThenInclude(ms => ms.MatchParticipants)
                                              .ThenInclude(mp => mp.Team)
                                              .FirstOrDefaultAsync(m => m.Id.Equals(matchResult.MatchId.Value));

      if (match == null)
      {
        result.Error = Shared.ErrorKey.Match.NotFound;
        return result;
      }

      if (!match.State.Equals(Shared.Enums.MatchState.Pending))
      {
        result.Error = Shared.ErrorKey.Match.AlreadyReported;
        return result;
      }

      var allTeamsParticipating = match.MatchSeries.MatchParticipants.Select(mp => mp.TeamId).All(tId => match.MatchSeries.IsParticipant(tId) == true);
      if (!allTeamsParticipating)
      {
        result.Error = Shared.ErrorKey.Match.InvalidParticipant;
        return result;
      }

      this.DangerouslyReportMatch(match, matchResult);
      result.Succeed();
      return result;

    }

    public async Task<ServiceResult> CreateAndReportMatchResult(DTO.MatchSeries.MatchResultReport report,
      Guid matchSeriesId, int order)
    {
      var result = new ServiceResult();
      var matchSeries = await this.alexandriaContext.MatchSeries
        .Include(ms => ms.MatchParticipants)
        .ThenInclude(mp => mp.Team)
        .FirstOrDefaultAsync(ms => ms.Id == matchSeriesId);

      if (matchSeries == null)
      {
        result.Error = Shared.ErrorKey.MatchSeries.NotFound;
        return result;
      }

      var match = new Match(matchSeriesId);
      match.MatchSeries = matchSeries;
      match.MatchOrder = order;
      this.alexandriaContext.Matches.Add(match);
      this.DangerouslyReportMatch(match, report);
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

    private void DangerouslyRescindScheduleRequest(EF.Models.MatchSeriesScheduleRequest scheduleRequest)
    {
      scheduleRequest.Rescind();
      alexandriaContext.MatchSeriesScheduleRequests.Update(scheduleRequest);
    }

    private void DangerouslyReportMatchSeries(EF.Models.MatchSeries matchSeries, MatchState state)
    {
      matchSeries.State = state;
      this.alexandriaContext.MatchSeries.Update(matchSeries);
    }

    private void DangerouslyReportMatch(EF.Models.Match match, DTO.MatchSeries.MatchResultReport matchResult)
    {
      match.State = Shared.Enums.MatchState.Complete;
      match.OutcomeState = matchResult.Outcome;

      foreach (var teamResult in matchResult.Results)
      {
        var participant = match.MatchSeries.GetParticipant(teamResult.TeamId);
        if (participant == null)
        {
          throw new NoNullAllowedException("Participant can't be null");
        }
        var matchParticipantResult = new MatchParticipantResult(match.Id, participant.Id, teamResult.Result);
        match.Results.Add(matchParticipantResult);
      }

      //this.alexandriaContext.Matches.Update(match);
    }
  }
}
