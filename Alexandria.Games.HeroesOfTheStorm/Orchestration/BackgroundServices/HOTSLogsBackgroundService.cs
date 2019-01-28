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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Alexandria.Games.HeroesOfTheStorm.Orchestration.BackgroundServices
{
  public class HOTSLogsMMRPullBackgroundService : BackgroundService
  {
    private string jobQueue;
    private IBackgroundWorker backgroundWorker;
    private HOTSLogsClient hotslogsClient;
    private IServiceProvider provider;
    public HOTSLogsMMRPullBackgroundService(IOptions<Configuration.Queue> queues, IBackgroundWorker backgroundWorker, HOTSLogsClient hotslogsClient, IServiceProvider provider)
    {
      if (queues.Value == null)
      {
        throw new NoNullAllowedException("Queues Config can't be null");
      }
      this.jobQueue = queues.Value.HOTSLogsPull;
      this.backgroundWorker = backgroundWorker;
      this.hotslogsClient = hotslogsClient;
      this.provider = provider;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      while (!stoppingToken.IsCancellationRequested)
      {
        var messages = await this.backgroundWorker.ReceiveMessages<DTO.Jobs.HOTSLogsUpdate>(this.jobQueue, 10, 15);
        if (messages != null && messages.Any())
        {
          try
          {
            foreach (var message in messages)
            {
              await this.UpdateUser(message.Data.UserProfileId, message.Data.Region);
            }
          } catch (Exception ex)
          {
            Sentry.SentrySdk.CaptureException(ex);
          }


          await this.backgroundWorker.AcknowledgeMessage(this.jobQueue, messages.Select(m => m.Receipt).ToList());
        }

        Thread.Sleep(10 * 1000);
      }
    }

    private async Task UpdateUser(Guid userProfileId, Shared.Enums.BattleNetRegion region)
    {
      using (var scope = this.provider.CreateScope())
      {

        var alexandriaContext = scope.ServiceProvider.GetService<Alexandria.EF.Context.AlexandriaContext>();
        var heroesOfTheStormContext = scope.ServiceProvider.GetService<HeroesOfTheStormContext>();

        var user = await alexandriaContext.UserProfiles.Include(u => u.ExternalAccounts).FirstOrDefaultAsync(u => u.Id == userProfileId);
        if (user == null)
        {
          return;
        }

        var battleNet = user.ExternalAccounts.FirstOrDefault(e => e.Provider == Shared.Enums.ExternalProvider.BattleNet);
        if (battleNet == null)
        {
          return;
        }

        var result = await this.hotslogsClient.GetPlayerRankings(battleNet.Name, (int)region);

        var rankings = result.Select((r) => AutoMapper.Mapper.Map<EF.Models.ExternalRanking>(r, opts => {
          opts.Items["UserProfileId"] = userProfileId;
          opts.Items["Region"] = region;
        })).ToList();
        heroesOfTheStormContext.ExternalRankings.AddRange(rankings);
        await heroesOfTheStormContext.SaveChangesAsync();
      }
    }
  }
}
