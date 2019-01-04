using System.Collections.Generic;
using System.Threading.Tasks;
using Alexandria.Interfaces.Services;
using Alexandria.Shared.ErrorKey;
using Alexandria.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Svalbard;

namespace Alexandria.Controllers.UserProfile
{
  [Route("user-profile/connections")]
  [ApiController]
  public class UserProfileConnectionsController : ControllerBase
  {
    private readonly IUserProfileService profileService;

    public UserProfileConnectionsController(IUserProfileService profileService)
    {
      this.profileService = profileService;
    }

    /// <summary>
    /// Get the connections of the currently logged in user.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(List<DTO.UserProfile.ConnectionDetail>), 200)]
    [ProducesResponseType(typeof(void), 401)]
    [ProducesResponseType(typeof(BaseError), 404)]
    public async Task<OperationResult<List<DTO.UserProfile.ConnectionDetail>>> GetConnections()
    {
      var userId = HttpContext.GetUserId();
      if (!userId.HasValue)
      {
        return new OperationResult<List<DTO.UserProfile.ConnectionDetail>>(404);
      }

      var result = await profileService.GetConnections(userId.Value);
      if (result.Success)
      {
        return new OperationResult<List<DTO.UserProfile.ConnectionDetail>>(result.Data);
      }

      return new OperationResult<List<DTO.UserProfile.ConnectionDetail>>(result.Error);
    }
  }
}
