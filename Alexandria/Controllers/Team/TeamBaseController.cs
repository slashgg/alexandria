using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
