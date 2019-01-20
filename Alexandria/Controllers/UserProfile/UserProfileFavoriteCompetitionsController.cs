using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alexandria.Infrastructure.Filters;
using Alexandria.Interfaces.Services;
using Alexandria.Shared.ErrorKey;
using Alexandria.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Svalbard;

namespace Alexandria.Controllers.UserProfile
{
  [Route("user-profile/favorite-competitions")]
  [ApiController]
  [Authorize]
  public class UserProfileFavoriteCompetitionsController : ControllerBase
  {
    private readonly IUserProfileService userProfileService;

    public UserProfileFavoriteCompetitionsController(IUserProfileService userProfileService)
    {
      this.userProfileService = userProfileService;
    }

    /// <summary>
    /// Gets a list of all the users favorite competitions
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(IList<DTO.UserProfile.FavoriteCompetition>), 200)]
    [ProducesResponseType(401)]
    public async Task<OperationResult<IList<DTO.UserProfile.FavoriteCompetition>>> GetFavoriteCompetitions()
    {
      var userId = HttpContext.GetUserId();
      if (!userId.HasValue)
      {
        return new OperationResult<IList<DTO.UserProfile.FavoriteCompetition>>(401);
      }

      var result = await this.userProfileService.GetFavoriteCompetitions(userId.Value);

      return new OperationResult<IList<DTO.UserProfile.FavoriteCompetition>>(result.Data);
    }

    /// <summary>
    /// Adds a new favorite competition to the user
    /// </summary>
    /// <param name="payload">CompetitionId in Body</param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(401)]
    [ProducesResponseType(typeof(BaseError), 404)]
    [ProducesResponseType(typeof(BaseError), 409)]
    public async Task<OperationResult> AddFavorite([FromBody] DTO.UserProfile.AddFavoriteCompetition payload)
    {
      var userId = HttpContext.GetUserId();
      if (userId == null)
      {
        return new OperationResult(401);
      }

      var result = await this.userProfileService.AddFavoriteCompetition(userId.Value, payload.CompetitionId);
      if (result.Success)
      {
        return new OperationResult(201);
      }

      return new OperationResult(result.Error);
    }

    /// <summary>
    /// Deletes a favorited competition for the user
    /// </summary>
    /// <param name="id">Guid of the Favorite Entity</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [PermissionsRequired("favorite-competition::{id}::delete")]
    [ProducesResponseType(204)]
    [ProducesResponseType(401)]
    [ProducesResponseType(typeof(BaseError), 404)]
    public async Task<OperationResult> DeleteFavorite([FromRoute] Guid id)
    {
      var userId = HttpContext.GetUserId();
      if (userId == null)
      {
        return new OperationResult(401);
      }

      var result = await this.userProfileService.DeleteFavoriteCompetition(id);
      if (result.Success)
      {
        return new OperationResult(201);
      }

      return new OperationResult(result.Error);
    }
  }
}
