using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alexandria.EF.Context;
using Alexandria.Interfaces.Services;
using Alexandria.Orchestration.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Svalbard.Services;

namespace Alexandria.Orchestration.Services
{
  public class TournamentService : ITournamentService
  {
    private readonly AlexandriaContext alexandriaContext;
    private readonly IMemoryCache cache;
    private readonly TournamentUtils tournamentUtils;

    public TournamentService(AlexandriaContext alexandriaContext, IMemoryCache memoryCache, TournamentUtils tournamentUtils)
    {
      this.alexandriaContext = alexandriaContext;
      this.cache = memoryCache;
      this.tournamentUtils = tournamentUtils;
    }

    public async Task<ServiceResult<DTO.Tournament.Detail>> GetTournamentDetail(string slug)
    {
      var result = new ServiceResult<DTO.Tournament.Detail>();
      var tournamentId = (await this.alexandriaContext.Tournaments.FirstOrDefaultAsync(t => t.Slug == slug))?.Id;
      if (!tournamentId.HasValue)
      {
        result.Error = Shared.ErrorKey.Tournament.NotFound;
        return result;
      }

      return await this.GetTournamentDetail(tournamentId.Value);
    }

    public async Task<ServiceResult<DTO.Tournament.Detail>> GetTournamentDetail(Guid tournamentId)
    {
      var result = new ServiceResult<DTO.Tournament.Detail>();

      var tournament = await alexandriaContext.Tournaments.Include(t => t.Tournaments)
                                                          .ThenInclude(t1 => t1.Tournaments)
                                                          .ThenInclude(t2 => t2.Tournaments)
                                                          .ThenInclude(t4 => t4.Tournaments)
                                                          .FirstOrDefaultAsync(t => t.Id == tournamentId);
      if (tournament == null)
      {
        result.Error = Shared.ErrorKey.Tournament.NotFound;
        return result;
      }
      var detailDTO = AutoMapper.Mapper.Map<DTO.Tournament.Detail>(tournament);

      result.Succeed(detailDTO);
      return result;
    }

    public async Task<ServiceResult<IList<DTO.Competition.TournamentApplication>>> GetOpenTournamentApplications(string competitionSlug)
    {
      var result = new ServiceResult<IList<DTO.Competition.TournamentApplication>>();

      var tournaments = await alexandriaContext.Tournaments.Include(t => t.TournamentApplicationQuestions)
                                                           .Include(t => t.Competition)
                                                           .Where(t => t.CanSignup && t.Competition.Slug == competitionSlug && t.ParentTournamentId == null)
                                                           .ToListAsync();

      var tournamentDTOs = tournaments.Select(AutoMapper.Mapper.Map<DTO.Competition.TournamentApplication>).ToList();

      result.Succeed(tournamentDTOs);
      return result;
    }

    public async Task<ServiceResult<IList<DTO.Competition.TournamentApplication>>> GetOpenTournamentApplications(Guid competitionId)
    {
      var result = new ServiceResult<IList<DTO.Competition.TournamentApplication>>();

      var tournaments = await alexandriaContext.Tournaments.Include(t => t.TournamentApplicationQuestions).Where(t => t.CanSignup && t.CompetitionId == competitionId && t.ParentTournamentId == null).ToListAsync();
      var tournamentDTOs = tournaments.Select(AutoMapper.Mapper.Map<DTO.Competition.TournamentApplication>).ToList();

      result.Succeed(tournamentDTOs);
      return result;
    }

    public async Task<ServiceResult<DTO.Competition.TournamentApplication>> GetTournamentApplication(Guid tournamentId)
    {
      var result = new ServiceResult<DTO.Competition.TournamentApplication>();
      var application = await alexandriaContext.Tournaments.Include(t => t.TournamentApplicationQuestions).FirstOrDefaultAsync(ta => ta.Id == tournamentId);

      if (application == null)
      {
        result.Error = Shared.ErrorKey.Tournament.NotFound;
        return result;
      }

      var applicationDTO = AutoMapper.Mapper.Map<DTO.Competition.TournamentApplication>(application);

      result.Succeed(applicationDTO);
      return result;
    }

    public async Task<ServiceResult<DTO.Competition.TournamentApplication>> GetTournamentApplication(string tournamentSlug)
    {
      var result = new ServiceResult<DTO.Competition.TournamentApplication>();
      var application = await this.alexandriaContext.Tournaments.Include(t => t.TournamentApplicationQuestions).FirstOrDefaultAsync(t => t.Slug == tournamentSlug);
      if (application == null)
      {
        result.Error = Shared.ErrorKey.Tournament.NotFound;
        return result;
      }

      var applicationDTO = AutoMapper.Mapper.Map<DTO.Competition.TournamentApplication>(application);

      result.Succeed(applicationDTO);
      return result;
    }

    public async Task<ServiceResult> TeamApplyToTournament(Guid teamId, DTO.Tournament.TeamTournamentApplicationRequest teamApplication)
    {
      var result = new ServiceResult();

      var tournament = await this.alexandriaContext.Tournaments.Include(t => t.TournamentApplications).ThenInclude(t => t.TournamentApplicationQuestionAnswers).FirstOrDefaultAsync(t => t.Id == teamApplication.TournamentId);
      if (tournament == null)
      {
        result.Error = Shared.ErrorKey.Tournament.NotFound;
        return result;
      }

      if (!tournament.CanSignup)
      {
        result.Error = Shared.ErrorKey.Tournament.ApplicationsClosed;
        return result;
      }

      var application = tournament.TournamentApplications.FirstOrDefault(ta => ta.TeamId == teamId);
      if (application == null)
      {
        application = await this.DangerouslyCreateTournamentApplication(teamId, teamApplication);
      } else
      {
        application = await this.DangerouslyUpdateTournamentApplication(application, teamApplication.Answers);
      }

      result.Succeed();
      return result;
    }

    public async Task<ServiceResult> WithdrawTeamApplication(string tournamentSlug, Guid teamId)
    {
      var result = new ServiceResult();
      var tournament = await this.alexandriaContext.Tournaments.FirstOrDefaultAsync(t => t.Slug == tournamentSlug);
      if (tournament == null)
      {
        result.Error = Shared.ErrorKey.Tournament.NotFound;
        return result;
      }

      return await this.WithdrawTeamApplication(tournament.Id, teamId);
    }

    public async Task<ServiceResult> WithdrawTeamApplication(Guid tournamentId, Guid teamId)
    {
      var result = new ServiceResult();

      var tournamentApplication = await this.alexandriaContext.TournamentApplications.FirstOrDefaultAsync(ta => ta.TournamentId == tournamentId && ta.TeamId == teamId);

      if (tournamentApplication == null)
      {
        result.Error = Shared.ErrorKey.TournamentApplication.NotFound;
        return result;
      }

      await this.DangerouslyWithdrawTournamentApplication(tournamentApplication);
      return result;
    }

    public async Task<ServiceResult<DTO.Tournament.TournamentApplication>> GetTeamApplication(string tournamentSlug, Guid teamId)
    {
      var result = new ServiceResult<DTO.Tournament.TournamentApplication>();
      var tournament = await this.alexandriaContext.Tournaments.FirstOrDefaultAsync(t => t.Slug == tournamentSlug);
      if (tournament == null)
      {
        result.Error = Shared.ErrorKey.Tournament.NotFound;
        return result;
      }
      return await this.GetTeamApplication(tournament.Id, teamId);
    }

    public async Task<ServiceResult<DTO.Tournament.TournamentApplication>> GetTeamApplication(Guid tournamentId, Guid teamId)
    {
      var result = new ServiceResult<DTO.Tournament.TournamentApplication>();

      var tournamentApplication = await this.alexandriaContext.TournamentApplications.Include(t => t.TournamentApplicationQuestionAnswers).FirstOrDefaultAsync(ta => ta.TournamentId == tournamentId && ta.TeamId == teamId);

      if (tournamentApplication == null)
      {
        result.Error = Shared.ErrorKey.TournamentApplication.NotFound;
        return result;
      }

      var applicationDTO = AutoMapper.Mapper.Map<DTO.Tournament.TournamentApplication>(tournamentApplication);
      result.Succeed(applicationDTO);

      return result;
    }

    public async Task<ServiceResult<List<DTO.Tournament.TeamParticipation>>> GetTeamParticipations(string tournamentSlug)
    {
      var result = new ServiceResult<List<DTO.Tournament.TeamParticipation>>();
      var tournament = await alexandriaContext.Tournaments.FirstOrDefaultAsync(t => t.Slug == tournamentSlug);
      if (tournament == null)
      {
        result.Error = Shared.ErrorKey.Tournament.NotFound;
        return result;
      }

      return await this.GetTeamParticipations(tournament.Id);
    }

    public async Task<ServiceResult<List<DTO.Tournament.TeamParticipation>>> GetTeamParticipations(Guid tournamentId)
    {
      var result = new ServiceResult<List<DTO.Tournament.TeamParticipation>>();
      var participationsDTO = await this.cache.GetOrCreateAsync(Shared.Cache.Tournament.Participants(tournamentId), async (cache) =>
      {
        var tournaments = await this.tournamentUtils.GetTournamentTree(tournamentId);
        var tournamentIds = tournaments.Select(t => t.Id);


        var tournamentParticipations = await alexandriaContext.TournamentParticipations.Include(t => t.Team)
                                                                               .ThenInclude(tt => tt.TeamMemberships)
                                                                               .ThenInclude(tm => tm.TeamRole)
                                                                               .Include(t => t.Team)
                                                                               .ThenInclude(tt => tt.TeamMemberships)
                                                                               .ThenInclude(tm => tm.UserProfile)
                                                                               .ThenInclude(up => up.ExternalUserNames)
                                                                               .Include(t => t.Team)
                                                                               .ThenInclude(tt => tt.TeamMemberships)
                                                                               .ThenInclude(m => m.UserProfile)
                                                                               .Include(t => t.Team)
                                                                               .ThenInclude(t => t.Competition)
                                                                               .Where(tp => tournamentIds.Contains(tp.TournamentId) && tp.State == Shared.Enums.TournamentParticipationState.Participating)
                                                                               .ToListAsync();
        cache.SetAbsoluteExpiration(DateTimeOffset.UtcNow.AddHours(1));

        var dto = tournamentParticipations.Select(AutoMapper.Mapper.Map<DTO.Tournament.TeamParticipation>).ToList();
        return dto;
      });


      result.Succeed(participationsDTO);
      return result;
    }

    public async Task<ServiceResult<IList<DTO.Tournament.RoundDetail>>> GetTournamentRounds(Guid tournamentId)
    {
      var result = new ServiceResult<IList<DTO.Tournament.RoundDetail>>();
      var roundsDTO = await this.cache.GetOrCreateAsync(Shared.Cache.Tournament.Rounds(tournamentId), async (cache) =>
      {
        var rounds = await alexandriaContext.TournamentRounds.Where(tr => tr.TournamentId == tournamentId).ToListAsync();

        cache.SetAbsoluteExpiration(DateTimeOffset.UtcNow.AddHours(1));

        var dtos = rounds.Select(AutoMapper.Mapper.Map<DTO.Tournament.RoundDetail>).ToList();
        return dtos;
      });

      result.Succeed(roundsDTO);
      return result;
    }

    public async Task<ServiceResult<DTO.Tournament.Schedule>> GetSchedule(Guid tournamentId, int? tournamentSteps = null)
    {
      var result = new ServiceResult<DTO.Tournament.Schedule>();
      var tournamentExists = await this.alexandriaContext.Tournaments.AnyAsync(t => t.Id == tournamentId);
      if (!tournamentExists)
      {
        result.Error = Shared.ErrorKey.Tournament.NotFound;
        return result;
      }

      var tournaments = (await this.tournamentUtils.GetTournamentTree(tournamentId, tournamentSteps, 0)).Select(t => t.Id);
      var matchRounds = new List<DTO.Tournament.ScheduleRound>();
      foreach (var recursiveTournamentId in tournaments)
      {
        var rounds = await this.LoadTournamentSchedule(recursiveTournamentId);
        matchRounds.AddRange(rounds);
      }

      result.Succeed(new DTO.Tournament.Schedule { Rounds = matchRounds });

      return result;
    }

    public async Task<ServiceResult<IList<DTO.Tournament.MatchSeries>>> GetMatchesForTeamInTournament(Guid teamId, Guid tournamentId)
    {
      var result = new ServiceResult<IList<DTO.Tournament.MatchSeries>>();

      var tournaments = (await this.tournamentUtils.GetTournamentTree(tournamentId, null, 0)).Select(t => t.Id);
      var matches = await this.alexandriaContext.MatchSeries.Include(ms => ms.TournamentRound)
                                                            .ThenInclude(tr => tr.MatchSeries)
                                                            .ThenInclude(trms => trms.MatchParticipants)
                                                            .Include(ms => ms.MatchParticipants)
                                                            .ThenInclude(mp => mp.Team)
                                                            .Include(ms => ms.Matches)
                                                            .ThenInclude(m => m.Results)
                                                            .Where(ms => ms.MatchParticipants.Any(mp => mp.TeamId == teamId))
                                                            .Where(ms => tournaments.Any(t => ms.TournamentRound.TournamentId == t))
                                                            .ToListAsync();

      var participants = matches.SelectMany(ms => ms.MatchParticipants).Select(mp => mp.TeamId).Distinct();
      var recordVault = new Dictionary<Guid, DTO.Tournament.TournamentRecord>();
      foreach (var participantTeamId in participants)
      {
        var record = await this.tournamentUtils.GetTournamentRecordForTeam(participantTeamId, tournamentId);
        recordVault.TryAdd(participantTeamId, record);
      }

      var tournamentMatchDTOs = matches.Select(ms => AutoMapper.Mapper.Map<DTO.Tournament.MatchSeries>(ms, ctx => ctx.Items.Add("RecordVault", recordVault))).ToList();
      result.Succeed(tournamentMatchDTOs);


      return result;
    }

    private async Task<IList<DTO.Tournament.ScheduleRound>> LoadTournamentSchedule(Guid tournamentId)
    {
      var tournamentRoundsSchedule = await this.cache.GetOrCreateAsync(Shared.Cache.Tournament.Schedule(tournamentId), async (cache) =>
      {

        var tournamentRounds = await this.alexandriaContext.TournamentRounds.Include(tr => tr.MatchSeries)
                                                                            .ThenInclude(ms => ms.MatchParticipants)
                                                                            .ThenInclude(mp => mp.Team)
                                                                            .Include(tr => tr.MatchSeries)
                                                                            .ThenInclude(ms => ms.Matches)
                                                                            .ThenInclude(m => m.Results)
                                                                            .Where(tr => tr.TournamentId == tournamentId)
                                                                            .ToListAsync();



        var matchSeries = tournamentRounds.SelectMany(tr => tr.MatchSeries);
        var participants = matchSeries.SelectMany(ms => ms.MatchParticipants).Select(mp => mp.TeamId).Distinct();
        var recordVault = new Dictionary<Guid, DTO.Tournament.TournamentRecord>();
        foreach (var teamId in participants)
        {
          var record = await this.tournamentUtils.GetTournamentRecordForTeam(teamId, tournamentId);
          recordVault.TryAdd(teamId, record);
        }

        cache.SetAbsoluteExpiration(DateTimeOffset.UtcNow.AddMinutes(30));
        var rounds = tournamentRounds.Select(r => AutoMapper.Mapper.Map<DTO.Tournament.ScheduleRound>(r, ctx => ctx.Items.Add("RecordVault", recordVault))).ToList();
        return rounds;
      });

      return tournamentRoundsSchedule;
    }

    private Task<EF.Models.TournamentApplication> DangerouslyCreateTournamentApplication(Guid teamId, DTO.Tournament.TeamTournamentApplicationRequest teamApplication)
    {
      var application = new EF.Models.TournamentApplication(teamId, teamApplication.TournamentId);

      foreach (var answer in teamApplication.Answers)
      {
        var questionAnswer = new EF.Models.TournamentApplicationQuestionAnswer(answer.Id, answer.Answer);
        application.AddAnswer(questionAnswer);
      }

      application.Initialize();
      this.alexandriaContext.TournamentApplications.Add(application);

      return Task.FromResult(application);
    }

    private Task<EF.Models.TournamentApplication> DangerouslyUpdateTournamentApplication(EF.Models.TournamentApplication application, IList<DTO.Tournament.TournamentApplicationQuestionAnswer> answers)
    {
      foreach (var answer in answers)
      {
        var questionAnswer = new EF.Models.TournamentApplicationQuestionAnswer(answer.Id, answer.Answer);
        application.AddAnswer(questionAnswer);
      }

      application.Mark(Shared.Enums.TournamentApplicationState.Pending, "Updated");
      this.alexandriaContext.TournamentApplications.Update(application);

      return Task.FromResult(application);
    }

    private Task<EF.Models.TournamentApplication> DangerouslyWithdrawTournamentApplication(EF.Models.TournamentApplication application)
    {
      application.Mark(Shared.Enums.TournamentApplicationState.Withdrawn, "Withdrawn");
      this.alexandriaContext.TournamentApplications.Update(application);

      return Task.FromResult(application);
    }
  }
}
