using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alexandria.EF.Context;
using Alexandria.EF.Models;
using Alexandria.Interfaces.Services;
using Alexandria.Shared.Extensions;
using Alexandria.Shared.Utils;
using Microsoft.EntityFrameworkCore;

namespace Alexandria.Orchestration.Utils
{
  public class MatchSeriesUtils
  {
    private readonly AlexandriaContext alexandriaContext;
    private readonly IAuthorizationService authorizationService;

    public MatchSeriesUtils(AlexandriaContext alexandriaContext, IAuthorizationService authorizationService)
    {
      this.alexandriaContext = alexandriaContext;
      this.authorizationService = authorizationService;
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
      } else if (gameIdentifier.Equals(Shared.GlobalAssosications.Game.SuperSmashBrosUltimate))
      {
        return $"/super-smash-bros/match-series/{matchSeriesId}/reporting/meta";
      } else
      {
        return null;
      }
    }

    public async Task<bool> CanReport(Guid matchSeriesId, Guid userProfileId)
    {
      var matchSeries = await this.alexandriaContext.MatchSeries
                                                    .Include(ms => ms.MatchParticipants)
                                                    .ThenInclude(mp => mp.Team)
                                                    .ThenInclude(t => t.TeamMemberships)
                                                    .ThenInclude(tm => tm.TeamRole)
                                                    .FirstOrDefaultAsync(ms => ms.Id == matchSeriesId);

      if (matchSeries == null)
      {
        return false;
      }

      var teamMemberships = matchSeries.MatchParticipants.Select(mp => mp.Team).SelectMany(t => t.TeamMemberships);
      var teamMembership = teamMemberships.FirstOrDefault(tm => tm.UserProfileId == userProfileId);
      
      if (teamMembership == null)
      {
        return false;
      }

      return await this.authorizationService.Can(userProfileId, AuthorizationHelper.GenerateARN(typeof(Team), teamMembership.TeamId.ToString(), Shared.Permissions.Team.ReportMatchResult));
    }
  }
}
