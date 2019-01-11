using System.Threading.Tasks;
using Alexandria.DTO.UserProfile;
using Alexandria.Interfaces.Services;
using Alexandria.Shared.ErrorKey;
using Alexandria.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Svalbard;

namespace Alexandria.Controllers.UserProfile
{
  [Route("user-profile/avatar")]
  [ApiController]
  public class UserProfileAvatarController : ControllerBase
  {
    private readonly IUserProfileService userProfiles;
    private readonly IFileService fileService;
    private readonly ILogger<UserProfileAvatarController> logger;

    public UserProfileAvatarController(IUserProfileService userProfiles, IFileService fileService, ILogger<UserProfileAvatarController> logger)
    {
      this.userProfiles = userProfiles;
      this.fileService = fileService;
      this.logger = logger;
    }

    /// <summary>
    /// Updates the avatar for the logged in user.
    /// </summary>
    /// <param name="data">The update payload.</param>
    /// <returns></returns>
    [HttpPut]
    [Authorize]
    [ProducesResponseType(typeof(void), 204)]
    [ProducesResponseType(typeof(void), 401)]
    [ProducesResponseType(typeof(BaseError), 400)]
    public async Task<OperationResult> UpdateAvatar([FromBody] UpdateAvatar data)
    {
      if (data == null)
      {
        return new OperationResult(400);
      }

      var userId = HttpContext.GetUserId();
      if (!userId.HasValue)
      {
        return new OperationResult(401);
      }

      var presignedResult = fileService.GetFromCorrelationId(data.PresignedUrlCorrelationId);
      if (!presignedResult.Success)
      {
        return new OperationResult(presignedResult.Error);
      }

      var result = await userProfiles.UpdateAvatar(userId.Value, presignedResult.Data.Url);
      if (result.Success)
      {
        if (!string.IsNullOrEmpty(result.Data))
        {
          // Try to remove the old url, if it exists
          var deleteResult = await fileService.DeleteByUrl(result.Data);
          if (!deleteResult.Success)
          {
            // We don't notify the client of this file as it is backend cleanup and they 
            // can't do anything about it
            logger.LogError(deleteResult.Error.Key + $", {result.Data}");
          }
        }

        return new OperationResult(204);
      }

      return new OperationResult(result.Error);
    }
  }
}
