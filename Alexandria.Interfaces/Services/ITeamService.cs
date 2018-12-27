using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Svalbard.Services;

namespace Alexandria.Interfaces.Services
{
  public interface ITeamService
  {
    Task<ServiceResult> DisbandTeam(Guid teamId);
    Task<ServiceResult> CreateTeam(Guid competitionId, DTO.Team.Create teamData);
  }
}
