using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alexandria.EF.Context;
using Alexandria.Shared.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Alexandria.Orchestration.Utils
{
  public class MatchSeriesUtils
  {
    private readonly AlexandriaContext alexandriaContext;

    public MatchSeriesUtils(AlexandriaContext alexandriaContext)
    {
      this.alexandriaContext = alexandriaContext;
    }

    public async Task<string> GetMatchSeriesMetaDataEndpointURL(Guid matchSeriesId)
    {
      var matchSeries = await this.alexandriaContext.MatchSeries.Include(ms => ms.Game).FirstOrDefaultAsync(ms => ms.Id.Equals(matchSeriesId));

      if (matchSeries == null)
      {
        return null;
      }

      var gameIdentifier = matchSeries.Game.InternalIdentifier;
      
      if (gameIdentifier.Equals(Shared.GlobalAssosications.Game.HeroesOfTheStorm))
      {
        return $"/{gameIdentifier}/match-series/{matchSeriesId}/reporting/meta";
      } else
      {
        return null;
      }
    } 
  }
}
