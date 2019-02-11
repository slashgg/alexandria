using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alexandria.Interfaces.Services;
using Alexandria.Orchestration.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Alexandria.Controllers
{
  [Route("match-series")]
  [ApiController]
  public class MatchSeriesController : ControllerBase
  {
    private readonly IMatchService matchService;
    private readonly MatchSeriesUtils matchSeriesUtils;

    public MatchSeriesController(IMatchService matchService, MatchSeriesUtils matchSeriesUtils)
    {
      this.matchService = matchService;
      this.matchSeriesUtils = matchSeriesUtils;
    }
  }
}
