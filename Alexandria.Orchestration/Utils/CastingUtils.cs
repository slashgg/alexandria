using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alexandria.EF.Context;
using Alexandria.EF.Models;
using Alexandria.Interfaces.Services;
using Alexandria.Shared.Utils;
using Microsoft.EntityFrameworkCore;

namespace Alexandria.Orchestration.Utils
{
  public class CastingUtils
  {
    private readonly AlexandriaContext alexandriaContext;
    private readonly IAuthorizationService authorizationService;

    public CastingUtils(AlexandriaContext context, IAuthorizationService authorizationService)
    {
      this.alexandriaContext = context;
      this.authorizationService = authorizationService;
    }

    public async Task<bool> CanCast(Guid userId, Guid matchSeriesId)
    {
      var competition = (await this.alexandriaContext.MatchSeries
          .Include(ms => ms.TournamentRound)
          .ThenInclude(tr => tr.Tournament)
          .FirstOrDefaultAsync(ms => ms.Id == matchSeriesId))
        ?.TournamentRound
        ?.Tournament
        ?.CompetitionId;

      if (!competition.HasValue)
      {
        return true;
      }

      var ARN = AuthorizationHelper.GenerateARN(typeof(Competition), competition.ToString(),
        Shared.Permissions.Competition.CastGame);

      return await this.authorizationService.Can(userId, ARN);
    }
  }
}
