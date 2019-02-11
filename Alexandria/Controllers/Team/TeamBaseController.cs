using System;
using Microsoft.AspNetCore.Mvc;

namespace Alexandria.Controllers.Team
{
  [Route("api/[controller]")]
  [ApiController]
  public class TeamBaseController : ControllerBase
  {
    protected readonly Guid teamId;  
  }
}
