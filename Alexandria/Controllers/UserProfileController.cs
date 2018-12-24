using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alexandria.EF.Context;
using Alexandria.Infrastructure.Filters;
using Alexandria.Interfaces.Services;
using Alexandria.Shared.ErrorKey;
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
    [ProducesResponseType(typeof(void), 201)]
    [ProducesResponseType(typeof(BaseError), 400)]
    public async Task<OperationResult> CreateProfile([FromBody] Alexandria.DTO.UserProfile.Create payload)
    {
      var result = await userProfileService.CreateAccount(payload);
      if (result.Success)
      {
        return new OperationResult(201);
      }

      return new OperationResult(result.ErrorKey);
    }
  }
}
