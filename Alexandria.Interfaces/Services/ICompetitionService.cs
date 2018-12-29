using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Svalbard.Services;

namespace Alexandria.Interfaces.Services
{
  public interface ICompetitionService
  {
    Task<ServiceResult<DTO.Competition.Detail>> GetCompetitionDetail(Guid competitionId);
    Task<ServiceResult<DTO.Competition.Detail>> GetCompetitionByName(string name);
    Task<ServiceResult<IList<DTO.Competition.Detail>>> GetCompetitionsByGame(Guid gameId);
    Task<ServiceResult<IList<DTO.Competition.Detail>>> GetActiveCompetitions();
  }
}
