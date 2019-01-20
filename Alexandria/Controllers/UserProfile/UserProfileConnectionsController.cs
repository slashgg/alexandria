using System.Collections.Generic;
using System.Threading.Tasks;
using Alexandria.Infrastructure.Filters;
using Alexandria.Interfaces.Services;
using Alexandria.Shared.Enums;
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

    /// <summary>
    /// Gets the connection
    /// </summary>
    /// <param name="provider">The name of the provider to fetch</param>
    /// <returns></returns>
    [HttpGet("{provider}")]
    [ProducesResponseType(typeof(DTO.UserProfile.ConnectionDetail), 200)]
    [ProducesResponseType(typeof(void), 401)]
    [ProducesResponseType(typeof(void), 404)]
    public async Task<OperationResult<DTO.UserProfile.ConnectionDetail>> GetConnection(string provider)
    {
      var userId = HttpContext.GetUserId();
      if (!userId.HasValue)
      {
        return new OperationResult<DTO.UserProfile.ConnectionDetail>(404);
      }

      ExternalProvider providerValue;
      switch (provider)
      {
        case "battle.net":
          providerValue = ExternalProvider.BattleNet;
          break;
        case "discord":
          providerValue = ExternalProvider.Discord;
          break;
        case "twitch":
          providerValue = ExternalProvider.Twitch;
          break;
        default:
          providerValue = 0;
          break;
      }
      var result = await profileService.GetConnection(userId.Value, providerValue);
      if (result.Success)
      {
        return new OperationResult<DTO.UserProfile.ConnectionDetail>(result.Data);
      }

      return new OperationResult<DTO.UserProfile.ConnectionDetail>(result.Error);
    }


    /// <summary>
    /// Deletes the connection
    /// Requires permission: `external-account::{connectionId}::delete`.
    /// This method is idempotent.
    /// </summary>
    /// <param name="connectionId">GUID of the connection to delete</param>
    /// <returns></returns>
    [HttpDelete("{connectionId}")]
    [PermissionsRequired("external-account::{connectionId}::delete")]
    [ProducesResponseType(typeof(void), 204)]
    [ProducesResponseType(typeof(void), 401)]
    public async Task<Svalbard.OperationResult> DeleteConnection(string connectionId)
    {
      var userId = HttpContext.GetUserId();
      if (!userId.HasValue)
      {
        return new Svalbard.OperationResult(204);
      }

      var result = await profileService.DeleteConnection(connectionId);
      if (result.Success)
      {
        return new Svalbard.OperationResult(204);
      }

      return new Svalbard.OperationResult(result.Error);
    }

    /// <summary>
    /// Creates the given connection. Requires the @slashgg/alexandria.backchannel scope.
    /// </summary>
    /// <param name="createDto"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize("Backchannel")]
    [ProducesResponseType(typeof(void), 201)]
    [ProducesResponseType(typeof(void), 401)]
    [ProducesResponseType(typeof(BaseError), 400)]
    public async Task<Svalbard.OperationResult> CreateConnection(DTO.UserProfile.CreateConnection createDto)
    {
      var result = await profileService.CreateConnection(createDto);
      if (result.Success)
      {
        return new Svalbard.OperationResult(201);
      }

      return new Svalbard.OperationResult(result.Error);
    }
  }
}
