using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Svalbard.Services;

namespace Alexandria.Interfaces.Services
{
  public interface ITournamentService
  {
    Task<ServiceResult<IList<DTO.Competition.TournamentApplication>>> GetOpenTournamentApplications(string competitionSlug);
    Task<ServiceResult<IList<DTO.Competition.TournamentApplication>>> GetOpenTournamentApplications(Guid competitionId);
    Task<ServiceResult<DTO.Competition.TournamentApplication>> GetTournamentApplication(string tournamentSlug);
    Task<ServiceResult<DTO.Competition.TournamentApplication>> GetTournamentApplication(Guid tournamentId);
    Task<ServiceResult<DTO.Tournament.TournamentApplication>> GetTeamApplication(Guid tournamentId, Guid teamId);
    Task<ServiceResult<DTO.Tournament.TournamentApplication>> GetTeamApplication(string tournamentSlug, Guid teamId);
    Task<ServiceResult> WithdrawTeamApplication(Guid tournamentId, Guid teamId);
    Task<ServiceResult> WithdrawTeamApplication(string tournamentSlug, Guid teamId);
    Task<ServiceResult> TeamApplyToTournament(Guid teamId, DTO.Tournament.TeamTournamentApplicationRequest teamApplication);
    Task<ServiceResult<List<DTO.Tournament.TeamParticipation>>> GetTeamParticipations(Guid tournamentId);
    Task<ServiceResult<List<DTO.Tournament.TeamParticipation>>> GetTeamParticipations(string tournamentSlug);
  }
}
