using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Alexandria.Interfaces.Services;
using Alexandria.Shared.ErrorKey;
using Alexandria.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Svalbard;

namespace Alexandria.Admin.Controllers
{
  [Route("permissions")]
  [ApiController]
  public class PermissionsController : AdminController
  {
    private readonly IUserProfileService userService;

    public PermissionsController(IUserProfileService userService)
    {
      this.userService = userService;
    }

    /// <summary>
    /// Get Permissions for the User
    /// </summary>
    /// <returns></returns>
    [ProducesResponseType(typeof(IList<string>), 200)]
    [ProducesResponseType(typeof(BaseError), 401)]
    public async Task<OperationResult<IList<string>>> GetPermissions()
    {
      var userId = this.HttpContext.GetUserId();
      if (!userId.HasValue)
      {
        return new OperationResult<IList<string>>(401);
      }

      var permissions = await this.userService.GetPermissions(userId.Value, Shared.Permissions.Namespace.Admin);
      return new OperationResult<IList<string>>(permissions);
    }
  }
}
