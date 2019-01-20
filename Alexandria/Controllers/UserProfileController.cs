using System.Collections.Generic;
using System.Threading.Tasks;
using Alexandria.Interfaces.Services;
using Alexandria.Shared.ErrorKey;
using Alexandria.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Svalbard;

namespace Alexandria.Controllers
{
  [Route("user-profiles")]
  [ApiController]
  public class UserProfileController : ControllerBase
  {
    private readonly IUserProfileService userProfileService;
    public UserProfileController(IUserProfileService userProfileService)
    {
      this.userProfileService = userProfileService;
    }

    /// <summary>
    /// This will create a UserProfile after an Account has been established in Passport
    /// </summary>
    /// <param name="payload">The User Account Information</param>
    /// <returns></returns>
    [HttpPost]
    [Authorize("Backchannel")]
    [ProducesResponseType(typeof(void), 201)]
    [ProducesResponseType(typeof(BaseError), 400)]
    [ProducesResponseType(typeof(BaseError), 409)]
    public async Task<Svalbard.OperationResult> CreateProfile([FromBody] Alexandria.DTO.UserProfile.Create payload)
    {
      var result = await userProfileService.CreateAccount(payload);
      if (result.Success)
      {
        return new Svalbard.OperationResult(201);
      }
      return new Svalbard.OperationResult(result.Error);
    }

    /// <summary>
    /// Get the UserProfile of the logged in User
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(DTO.UserProfile.Detail), 200)]
    [ProducesResponseType(typeof(BaseError), 400)]
    [ProducesResponseType(typeof(void), 401)]
    [ProducesResponseType(typeof(BaseError), 404)]
    public async Task<OperationResult<DTO.UserProfile.Detail>> GetUserProfile()
    {
      var userId = HttpContext.GetUserId();
      if (userId == null)
      {
        return new OperationResult<DTO.UserProfile.Detail>(404);
      }

      var result = await this.userProfileService.GetUserProfileDetail(userId.Value);
      if (result.Success)
      {
        return new OperationResult<DTO.UserProfile.Detail>(result.Data);
      }

      return new OperationResult<DTO.UserProfile.Detail>(result.Error);
    }

    /// <summary>
    /// Update the profile settings for the logged in user.
    /// </summary>
    /// <param name="payload">The settings payload</param>
    /// <returns></returns>
    [HttpPut]
    [Authorize]
    [ProducesResponseType(typeof(void), 204)]
    [ProducesResponseType(typeof(BaseError), 400)]
    [ProducesResponseType(typeof(BaseError), 401)]
    [ProducesResponseType(typeof(BaseError), 409)]
    public async Task<Svalbard.OperationResult> UpdateUserProfile([FromBody] DTO.UserProfile.UpdateSettings payload)
    {
      var userId = HttpContext.GetUserId();
      if (!userId.HasValue)
      {
        return new Svalbard.OperationResult(401);
      }

      var result = await userProfileService.UpdateSettings(userId.Value, payload);
      if (result.Success)
      {
        return new Svalbard.OperationResult(204);
      }

      return new Svalbard.OperationResult(result.Error);
    }

    /// <summary>
    /// Resends email verification for the logged in user.
    /// </summary>
    /// <returns></returns>
    [HttpGet("verification")]
    [Authorize]
    [ProducesResponseType(typeof(void), 204)]
    [ProducesResponseType(typeof(BaseError), 401)]
    public async Task<Svalbard.OperationResult> ResendEmailVerification()
    {
      var userId = HttpContext.GetUserId();
      if (!userId.HasValue)
      {
        return new Svalbard.OperationResult(401);
      }

      var result = await userProfileService.ResendEmailVerification(userId.Value);
      if (result.Success)
      {
        return new Svalbard.OperationResult(204);
      }

      return new Svalbard.OperationResult(result.Error);
    }

    /// <summary>
    /// Get the Permissions for the logged in User
    /// </summary>
    /// <returns></returns>
    [HttpGet("permissions")]
    [Authorize]
    [ProducesResponseType(typeof(IList<string>), 200)]
    [ProducesResponseType(typeof(void), 401)]
    [ProducesResponseType(typeof(BaseError), 404)]
    public async Task<OperationResult<IList<string>>> GetPermissions()
    {
      var userId = HttpContext.GetUserId();
      if (userId == null)
      {
        return new OperationResult<IList<string>>(404);
      }

      var result = await this.userProfileService.GetPermissions(userId.Value);
      return new OperationResult<IList<string>>(result.Data);
    }
  }
}
