using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Svalbard.Services;

namespace Alexandria.Interfaces.Services
{
  public interface IGameCastingService
  {
    Task<ServiceResult<IList<DTO.Competition.Info>>> GetCastableCompetitions(Guid userId);
  }
}
