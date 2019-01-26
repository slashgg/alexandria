using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alexandria.Infrastructure;
using Alexandria.Infrastructure.Filters;
using Alexandria.Interfaces.Services;
using Alexandria.Shared.ErrorKey;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Svalbard;

namespace Alexandria.Controllers.Team
{
  [Route("teams/{teamId}")]
  [ResourceSelectFilter("teamId")]
  [ApiController]
  public class TeamDetailController : ResourceBaseController
  {
    private readonly ITeamService teamService;

    public TeamDetailController(ITeamService teamService)
    {
      this.teamService = teamService;
    }

    /// <summary>
    /// Updates a Team Logo
    /// </summary>
    /// <param name="payload">URL of Team Logo</param>
    /// <returns></returns>
    [HttpPost("logo")]
    [PermissionsRequired("team::{teamId}::logo")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(BaseError), 404)]
    public async Task<OperationResult> UpdateTeamAvatar([FromBody] DTO.Team.UpdateLogo payload)
    {
      if (this.resourceId != Guid.Empty)
      {
        var result = await teamService.UpdateTeamLogo(this.resourceId, payload.LogoURL);
        if (result.Success)
        {
          return new OperationResult(204);
        }

        return new OperationResult(result.Error);
      }

      return new OperationResult(404);
    }
  }
}
