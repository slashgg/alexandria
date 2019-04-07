using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Alexandria.Games.SuperSmashBros.Configuration;
using Alexandria.Games.SuperSmashBros.EF.Context;
using Alexandria.Interfaces.Processing;
using Microsoft.AspNetCore.Mvc.RazorPages.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Sentry;

namespace Alexandria.Games.SuperSmashBros.Orchestration.BackgroundServices
{
  public class MatchResultJobWorker : BackgroundService
  {
    private readonly string matchResultQueue;
    private readonly IBackgroundWorker backgroundWorker;
    private readonly IServiceProvider provider;

    public MatchResultJobWorker(IOptions<Queue> queues,
      IBackgroundWorker backgroundWorker, IServiceProvider serviceProvider)
    {
      if (queues.Value == null)
      {
        throw new NoNullAllowedException("Queues can't be null");
      }

      this.matchResultQueue = queues.Value.MatchResults;
      this.backgroundWorker = backgroundWorker;
      this.provider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      while (!stoppingToken.IsCancellationRequested)
      {
        var messages =
          await this.backgroundWorker.ReceiveMessages<DTO.MatchSeries.AdditionalMatchSeriesData>(this.matchResultQueue,
            5);
        if (messages.Any())
        {
          foreach (var message in messages)
          {
            try
            {
              if (message.Data != null)
              {
                await this.ReportMatchResult(message.Data);
              }
            }
            catch (Exception ex)
            {
              SentrySdk.CaptureException(ex);
            }
          }

          await this.backgroundWorker.AcknowledgeMessage(this.matchResultQueue, messages.Select(m => m.Receipt).ToList());
        }

        Thread.Sleep(10 * 1000);
      }
    }

    private async Task ReportMatchResult(DTO.MatchSeries.AdditionalMatchSeriesData payload)
    {
      using (var scope = this.provider.CreateScope())
      {
        var alexandriaContext = scope.ServiceProvider.GetService<Alexandria.EF.Context.AlexandriaContext>();
        var superSmashContext = scope.ServiceProvider.GetService<SuperSmashBrosContext>();
        var matchSeriesExists = await alexandriaContext.MatchSeries.AnyAsync(ms => ms.Id == payload.MatchSeriesId);
        if (matchSeriesExists)
        {
          EF.Models.MatchReport matchReport = await superSmashContext.MatchReports.Include(mr => mr.FighterPicks).FirstOrDefaultAsync(mr => mr.MatchSeriesId == payload.MatchSeriesId);
          if (matchReport == null)
          {
            
            matchReport = new EF.Models.MatchReport(payload.MatchSeriesId);
            superSmashContext.MatchReports.Add(matchReport);
          }
          foreach (var fighterPick in payload.FighterPicks)
          {
            var fighterExists = await superSmashContext.Fighters.AnyAsync(f => f.Id == fighterPick.FighterId);
            var teamExists = await alexandriaContext.Teams.AnyAsync(t => t.Id == fighterPick.TeamId);
            if (fighterExists && teamExists)
            {
              var fighterPickModel = new EF.Models.FighterPick(fighterPick.TeamId, fighterPick.FighterId, fighterPick.MatchId);
              matchReport.FighterPicks.Add(fighterPickModel);
            }

            await superSmashContext.SaveChangesAsync();
          }
        }
      }
    }
  }
}
