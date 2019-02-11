using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alexandria.EF.Context;
using Alexandria.Games.HeroesOfTheStorm.EF.Context;
using Microsoft.EntityFrameworkCore;

namespace Alexandria.Games.HeroesOfTheStorm.Orchestration.Services
{
  public class RankingService
  {
    private readonly AlexandriaContext alexandriaContext;
    private readonly HeroesOfTheStormContext heroesOfTheStormContext;

    public RankingService(AlexandriaContext alexandriaContext, HeroesOfTheStormContext heroesOfTheStormContext)
    {
      this.alexandriaContext = alexandriaContext;
      this.heroesOfTheStormContext = heroesOfTheStormContext;
    }

    public async Task AcquireHOTSLogsUpdate(Guid userId)
    {
      var user = this.alexandriaContext.UserProfiles.Include(u => u.ExternalAccounts);
    }
  }
}
