using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alexandria.EF.Context;
using Alexandria.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Svalbard.Services;

namespace Alexandria.Orchestration.Services
{
  public class TournamentService : ITournamentService
  {
    private readonly AlexandriaContext alexandriaContext;

    public TournamentService(AlexandriaContext alexandriaContext)
    {
      this.alexandriaContext = alexandriaContext;
    }

    public async Task<ServiceResult<IList<DTO.Competition.TournamentApplication>>> GetOpenTournamentApplications(string competitionSlug)
    {
      var result = new ServiceResult<IList<DTO.Competition.TournamentApplication>>();

      var tournaments = await alexandriaContext.Tournaments.Include(t => t.TournamentApplicationQuestions)
                                                           .Include(t => t.Competition)
                                                           .Where(t => t.CanSignup && t.Competition.Slug == competitionSlug)
                                                           .ToListAsync();

      var tournamentDTOs = tournaments.Select(AutoMapper.Mapper.Map<DTO.Competition.TournamentApplication>).ToList();

      result.Succeed(tournamentDTOs);
      return result;
    }

    public async Task<ServiceResult<IList<DTO.Competition.TournamentApplication>>> GetOpenTournamentApplications(Guid competitionId)
    {
      var result = new ServiceResult<IList<DTO.Competition.TournamentApplication>>();

      var tournaments = await alexandriaContext.Tournaments.Include(t => t.TournamentApplicationQuestions).Where(t => t.CanSignup && t.CompetitionId == competitionId).ToListAsync();
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
      var tournamentParticipations = await alexandriaContext.TournamentParticipations.Include(t => t.Team)
                                                                                     .ThenInclude(t => t.TeamMemberships)
                                                                                     .Where(tp => tp.TournamentId == tournamentId && tp.State == Shared.Enums.TournamentParticipationState.Participating)
                                                                                     .ToListAsync();

      result.Data = tournamentParticipations.Select(AutoMapper.Mapper.Map<DTO.Tournament.TeamParticipation>).ToList();
      result.Succeed();
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
  }
}
