using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alexandria.EF.Context;
using Alexandria.Interfaces.Services;
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

    public TournamentService(AlexandriaContext alexandriaContext, IMemoryCache memoryCache)
    {
      this.alexandriaContext = alexandriaContext;
      this.cache = memoryCache;
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
        var tournaments = await GetTournamentTree(tournamentId);
        var tournamentIds = tournaments.Select(t => t.Id);


        var tournamentParticipations = await alexandriaContext.TournamentParticipations.Include(t => t.Team)
                                                                               .ThenInclude(tt => tt.TeamMemberships)
                                                                               .ThenInclude(tm => tm.TeamRole)
                                                                               .Include(t => t.Team)
                                                                               .ThenInclude(tt => tt.TeamMemberships)
                                                                               .ThenInclude(m => m.UserProfile)
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

    public async Task<ServiceResult<DTO.Tournament.Schedule>> GetSchedule(Guid tournamentId)
    {
      var result = new ServiceResult<DTO.Tournament.Schedule>();

      var tournament = await this.alexandriaContext.Tournaments.Include(t => t.TournamentRounds)
                                                               .ThenInclude(tr => tr.MatchSeries)
                                                               .ThenInclude(tm => tm.MatchParticipants)
                                                               .ThenInclude(mp => mp.Team)
                                                               .Include(t => t.TournamentRounds)
                                                               .ThenInclude(tr => tr.MatchSeries)
                                                               .ThenInclude(ms => ms.Matches)
                                                               .ThenInclude(m => m.Results)
                                                               .FirstOrDefaultAsync(t => t.Id == tournamentId);

      if (tournament == null)
      {
        result.Error = Shared.ErrorKey.Tournament.NotFound;
        return result;
      }

      var matchSeries = tournament.TournamentRounds.SelectMany(tr => tr.MatchSeries);
      var participants = matchSeries.SelectMany(ms => ms.MatchParticipants).Select(mp => mp.TeamId).Distinct();
      var recordVault = new Dictionary<Guid, DTO.Tournament.TournamentRecord>();
      foreach (var teamId in participants)
      {
        var record = await this.GetTournamentRecordForTeam(teamId, tournamentId);
        recordVault.TryAdd(teamId, record);
      }

      var scheduleDTO = AutoMapper.Mapper.Map<DTO.Tournament.Schedule>(tournament, ctx => ctx.Items.Add("RecordVault", recordVault));
      var json = JsonConvert.SerializeObject(scheduleDTO);
      result.Succeed(scheduleDTO);

      return result;
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

    private async Task<IList<EF.Models.Tournament>> GetTournamentTree(Guid tournamentId)
    {
      var list = new List<EF.Models.Tournament>();
      var tournament = await alexandriaContext.Tournaments.Include(t => t.Tournaments).ThenInclude(t => t.Tournaments).FirstOrDefaultAsync(t => t.Id == tournamentId);

      if (tournament == null)
      {
        return new List<EF.Models.Tournament>();
      }

      if (tournament.Tournaments.Any())
      {
        foreach (var childTournament in tournament.Tournaments.ToList())
        {
          if (childTournament.Tournaments.Any())
          {
            list.AddRange(await GetTournamentTree(childTournament.Id));
          } else
          {
            list.Add(childTournament);
          }
        }
      } else
      {
        list.Add(tournament);
      }

      return list;
    }

    private async Task<DTO.Tournament.TournamentRecord> GetTournamentRecordForTeam(Guid teamId, Guid tournamentId)
    {
      var team = await this.alexandriaContext.Teams.Include(t => t.MatchParticipations)
                                                   .ThenInclude(mp => mp.MatchSeries)
                                                   .ThenInclude(ms => ms.MatchParticipants)
                                                   .Include(t => t.MatchParticipations)
                                                   .ThenInclude(mp => mp.MatchSeries)
                                                   .ThenInclude(ms => ms.Matches)
                                                   .ThenInclude(m => m.Results)
                                                   .Include(t => t.MatchParticipations)
                                                   .ThenInclude(mp => mp.MatchSeries)
                                                   .ThenInclude(ms => ms.TournamentRound)
                                                   .ThenInclude(tr => tr.Tournament)
                                                   .FirstOrDefaultAsync(t => t.Id == teamId);


      var tournamentMatches = team.MatchParticipations.Where(mp => mp.MatchSeries.TournamentRound.TournamentId == tournamentId).Select(mp => mp.MatchSeries);
      var wins = tournamentMatches.Where(m => m.Winner != null).Select(ms => ms.Winner).Where(mp => mp.TeamId == teamId).Count();
      var losses = tournamentMatches.Where(m => m.Loser != null).Select(ms => ms.Loser).Where(mp => mp.TeamId == teamId).Count();
      var draws = tournamentMatches.Count() - (wins + losses);

      return new DTO.Tournament.TournamentRecord(wins, losses, draws);

    }
  }
}
