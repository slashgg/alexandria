using Alexandria.DTO.UserProfile;
using Alexandria.Shared.Enums;
using Svalbard.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alexandria.Interfaces.Services
{
  public interface IUserProfileService
  {
    Task<ServiceResult<DTO.UserProfile.Detail>> GetUserProfileDetail(Guid userId);
    Task<ServiceResult> CreateAccount(DTO.UserProfile.Create account);
    Task<ServiceResult<IList<DTO.UserProfile.TeamInvite>>> GetTeamInvites(Guid userId);
    Task<ServiceResult<IList<string>>> GetPermissions(Guid userId);
    Task<ServiceResult<List<ConnectionDetail>>> GetConnections(Guid userId);
    Task<ServiceResult> CreateConnection(CreateConnection createDto);
    Task<ServiceResult> DeleteConnection(string connectionId);
    Task<ServiceResult<ConnectionDetail>> GetConnection(Guid userId, ExternalProvider provider);
  }
}
