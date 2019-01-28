using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Alexandria.ExternalServices.HOTSLogs;
using Alexandria.Games.HeroesOfTheStorm.EF.Context;
using Alexandria.Interfaces.Processing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Alexandria.Games.HeroesOfTheStorm.Orchestration.BackgroundServices
{
  public class HeroesOfTheStormCronWorker : BackgroundService
  {
    private string cronQueue;
    private string hotslogsJobQueue;
    private IBackgroundWorker backgroundWorker;
    private HOTSLogsClient hotslogsClient;
    private IServiceProvider provider;

    public HeroesOfTheStormCronWorker(IOptions<Configuration.Queue> queues, IBackgroundWorker backgroundWorker, HOTSLogsClient hotslogsClient, IServiceProvider provider)
    {
      if (queues.Value == null)
      {
        throw new NoNullAllowedException("Queues Config can't be null");
      }
      this.cronQueue = queues.Value.HeroesOfTheStormCron;
      this.hotslogsJobQueue = queues.Value.HOTSLogsPull;
      this.backgroundWorker = backgroundWorker;
      this.hotslogsClient = hotslogsClient;
      this.provider = provider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      while (!stoppingToken.IsCancellationRequested)
      {
        var messages = await this.backgroundWorker.ReceiveMessages<DTO.Cron.Identifier>(this.cronQueue, 1, 30);
        if (messages != null && messages.Any())
        {
          try
          {
            var type = messages.FirstOrDefault().Data;
            switch (type.CronIdentifier)
            {
              case "hotslogs-pull":
                await this.HOTSLogsPullCron();
                break;
              default:
                break;

            }
          }
          catch (Exception ex)
          {
            Sentry.SentrySdk.CaptureException(ex);
          }
          finally
          {
            await this.backgroundWorker.AcknowledgeMessage(this.cronQueue, messages.FirstOrDefault().Receipt);
          }

          Thread.Sleep(180 * 1000);
        }
      }
    }

    private async Task HOTSLogsPullCron()
    {
      using (var scope = this.provider.CreateScope())
      {
        var alexandriaContext = scope.ServiceProvider.GetService<Alexandria.EF.Context.AlexandriaContext>();
        var heroesOfTheStormContext = scope.ServiceProvider.GetService<HeroesOfTheStormContext>();
        var gameMembership = alexandriaContext.UserProfiles.Where(u => u.TeamMemberships.Any(tm => tm.Team.Competition.Game.InternalIdentifier == Shared.GlobalAssosications.Game.HeroesOfTheStorm))
                                                           .Where(u => u.ExternalAccounts.Any(exa => exa.Provider == Shared.Enums.ExternalProvider.BattleNet))
                                                           .Select(u => u.Id)
                                                           .ToList();


        var userNeedsRankingUpdate = heroesOfTheStormContext.ExternalRankings.Where(er => er.MMRSource == Shared.Enums.MMRSource.HOTSLogs)
                                                                             .Where(er => gameMembership.Contains(er.UserProfileId))
                                                                             .GroupBy(er => new { er.UserProfileId, er.CreatedAt })
                                                                             .Where(ger => ger.OrderByDescending(er => er.CreatedAt).FirstOrDefault().CreatedAt < DateTime.UtcNow.AddDays(-14))
                                                                             .SelectMany(ger => ger.Select(er => er.UserProfileId))
                                                                             .Distinct()
                                                                             .ToList();


        var firstTimers = gameMembership.Where(gm => !userNeedsRankingUpdate.Contains(gm))
                                        .Where(gm => !heroesOfTheStormContext.ExternalRankings.Any(er => er.UserProfileId == gm))
                                        .ToList();

        var usersToUpdate = userNeedsRankingUpdate.Concat(firstTimers).ToList();

        foreach (var userId in usersToUpdate)
        {
          await this.backgroundWorker.SendMessage(this.hotslogsJobQueue, new DTO.Jobs.HOTSLogsUpdate(userId));
        }
      }
    }
  }
}
