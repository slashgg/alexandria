using System;
using System.Threading.Tasks;
using Alexandria.DTO.UserProfile;
using Svalbard.Services;

namespace Alexandria.Interfaces.Services
{
  public interface IPassportClient
  {
    Task<ServiceResult> UpdateProfile(Guid profileId, UpdatePassportUser dto);
    Task<ServiceResult> ResendEmailVerification(string email);
  }
}
