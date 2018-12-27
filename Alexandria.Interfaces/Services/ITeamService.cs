using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Svalbard.Services;

namespace Alexandria.Interfaces.Services
{
  public interface ITeamService
  {
    Task<ServiceResult> DisbandTeam(Guid teamId);
    Task<ServiceResult> CreateTeam(Guid competitionId, DTO.Team.Create teamData);
    Task<ServiceResult<DTO.Team.Detail>> GetTeamDetail(Guid teamId);
    Task<ServiceResult<IList<DTO.Team.Invite>>> GetTeamInvites(Guid teamId);
    Task<ServiceResult> RemoveMember(Guid membershipId, string notes, bool forceRemove = false);
    Task<ServiceResult> InviteMember(Guid teamId, string invitee);
  }
}
