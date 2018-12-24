using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alexandria.Infrastructure.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Alexandria.Controllers.Competition
{
  [ApiController]
  [CompetitionSelectFilter]
  public class CompetitionBaseController : ControllerBase
  {
    public Guid CompetitionId { get; set; }
  }
}
