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
  }
}
