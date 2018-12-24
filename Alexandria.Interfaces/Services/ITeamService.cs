using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Svalbard.Services;

namespace Alexandria.Interfaces.Services
{
  public interface ITeamService
  {
    Task<ServiceResult> CreateTeam(DTO.Team.Create teamData);
  }
}
