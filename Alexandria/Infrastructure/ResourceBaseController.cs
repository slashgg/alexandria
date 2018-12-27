using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Alexandria.Infrastructure
{
  [ApiController]
  public class ResourceBaseController : ControllerBase
  {
    public Guid resourceId;
  }
}
