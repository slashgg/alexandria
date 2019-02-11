using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alexandria.EF.Context;
using Alexandria.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Alexandria.Orchestration.BackgroundServices.Crons
{
  public class ExternalUserNameCron
  {
    private IServiceProvider provider;

    public ExternalUserNameCron(IServiceProvider provider)
    {
      this.provider = provider;
    }

    public async Task Work()
    {
      using (var scope = this.provider.CreateScope())
      {
        var alexandriaContext = scope.ServiceProvider.GetRequiredService<AlexandriaContext>();

        var games = await alexandriaContext.Games
                                     .Include(g => g.GameExternalUserNameGenerator)
                                     .ThenInclude(geung => geung.ExternalUserNameGenerator)
                                     .Where(g => g.GameExternalUserNameGenerator != null)
                                     .ToListAsync();
        var userUtils = scope.ServiceProvider.GetRequiredService<IUserUtils>();

        foreach (var game in games)
        {
          var users = await alexandriaContext.UserProfiles
                                                        .Include(u => u.TeamMemberships)
                                                        .ThenInclude(tm => tm.Team)
                                                        .ThenInclude(t => t.Competition)
                                                        .Include(u => u.ExternalUserNames)
                                                        .Where(u => u.TeamMemberships
                                                          .Any(tm => tm.Team.Competition.GameId.Equals(game.Id)))
                                                        .Where(u => !u.ExternalUserNames
                                                          .Any(eu => eu.GameId.Equals(game.Id)))
                                                        .Select(u => u.Id)
                                                        .ToListAsync();

          foreach (var userId in users)
          {
            await userUtils.GenerateExternalUserName(userId, game.Id);
          }
        }

        await alexandriaContext.SaveChangesAsync();
      }
    }
  }
}
