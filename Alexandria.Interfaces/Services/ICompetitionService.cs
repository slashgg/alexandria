using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Svalbard.Services;

namespace Alexandria.Interfaces.Services
{
  public interface ICompetitionService
  {
    Task<ServiceResult<DTO.Competition.Detail>> GetCompetitionDetail(Guid competitionId);
    Task<ServiceResult<DTO.Competition.Detail>> GetCompetitionBySlug(string name);
    Task<ServiceResult<IList<DTO.Competition.Detail>>> GetCompetitionsByGame(Guid gameId);
    Task<ServiceResult<IList<DTO.Competition.Detail>>> GetActiveCompetitions();
    Task<ServiceResult<IList<DTO.Competition.Tournament>>> GetTournaments(Guid competitionId);
    Task<ServiceResult<IList<DTO.Competition.Tournament>>> GetTournaments(string competitionSlug);
    Task<ServiceResult<DTO.Competition.Tournament>> GetTournament(Guid tournamentId);
    Task<ServiceResult<DTO.Competition.Tournament>> GetTournament(string slug);

  }
}
