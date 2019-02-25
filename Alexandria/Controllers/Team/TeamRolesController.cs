using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alexandria.Infrastructure.Filters;
using Alexandria.Interfaces.Services;
using Alexandria.Shared.ErrorKey;
using Alexandria.Shared.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Svalbard;

namespace Alexandria.Controllers.Team
{
  [Route("teams/{teamId}/roles")]
  [ResourceSelectFilter("teamId")]
  [ApiController]
  public class TeamRolesController : ResourceBaseController
  {
    private readonly ITeamService teamService;

    public TeamRolesController(ITeamService teamService)
    {
      this.teamService = teamService;
    }

    /// <summary>
    /// Displays the available Roles for this Team
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(IList<DTO.Team.Role>), 200)]
    [ProducesResponseType(typeof(BaseError), 404)]
    public async Task<OperationResult<IList<DTO.Team.Role>>> GetAvailableRoles()
    {
      if (this.resourceId == Guid.Empty)
      {
        return new OperationResult<IList<DTO.Team.Role>>(404);
      }

      var result = await this.teamService.GetAvailableRoles(this.resourceId);
      if (result.Success)
      {
        return new OperationResult<IList<DTO.Team.Role>>(result.Data);
      }

      return new OperationResult<IList<DTO.Team.Role>>(result.Error);
    }
  }
}
