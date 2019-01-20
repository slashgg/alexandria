using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Svalbard.Services;

namespace Alexandria.Interfaces.Services
{
  public interface ICompetitionService
  {
    Task<ServiceResult<DTO.Competition.Detail>> GetCompetitionDetail(Guid competitionId);
    Task<ServiceResult<DTO.Competition.Detail>> GetCompetitionDetail(string slug);
    Task<ServiceResult<IList<DTO.Competition.Detail>>> GetCompetitionsByGame(Guid gameId);
    Task<ServiceResult<IList<DTO.Competition.Detail>>> GetActiveCompetitions();
    Task<ServiceResult<IList<DTO.Competition.Tournament>>> GetTournaments(Guid competitionId);
    Task<ServiceResult<IList<DTO.Competition.Tournament>>> GetTournaments(string competitionSlug);
    Task<ServiceResult<DTO.Competition.Tournament>> GetTournament(Guid tournamentId);
    Task<ServiceResult<DTO.Competition.Tournament>> GetTournament(string slug);
    Task<ServiceResult<IList<DTO.Competition.Detail>>> SearchCompetitions(DTO.Competition.Lookup lookup);
    Task<ServiceResult<IList<DTO.Competition.CompetitionLevel>>> GetCompetitionLevels();
    Task<ServiceResult<IList<DTO.Game.Detail>>> GetSupportedGames();

  }
}
