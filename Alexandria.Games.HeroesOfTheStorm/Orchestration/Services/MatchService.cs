using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alexandria.EF.Context;
using Alexandria.Games.HeroesOfTheStorm.EF.Context;
using Alexandria.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Svalbard.Services;

namespace Alexandria.Games.HeroesOfTheStorm.Orchestration.Services
{
  public class HeroesOfTheStormMatchService
  {
    private readonly AlexandriaContext alexandriaContext;
    private readonly HeroesOfTheStormContext heroesOfTheStormContext;
    private readonly IMatchService matchService;

    public HeroesOfTheStormMatchService(AlexandriaContext alexandriaContext, HeroesOfTheStormContext heroesOfTheStormContext, IMatchService matchService)
    {
      this.alexandriaContext = alexandriaContext;
      this.heroesOfTheStormContext = heroesOfTheStormContext;
      this.matchService = matchService;
    }

    public async Task<ServiceResult<DTO.MatchSeries.HeroesOfTheStormMatchReportMetaData>> GetMatchResultMetaData(Guid matchSeriesId)
    {
      var result = new ServiceResult<DTO.MatchSeries.HeroesOfTheStormMatchReportMetaData>();
      var originalMetaData = await this.matchService.GetResultSubmitMetaData(matchSeriesId);

      if (!originalMetaData.Success)
      {
        result.Error = originalMetaData.Error;
        return result;
      }

      var heroesMetaData = AutoMapper.Mapper.Map<DTO.MatchSeries.HeroesOfTheStormMatchReportMetaData>(originalMetaData.Data);
      var tournamentId = (await alexandriaContext.MatchSeries.Include(ms => ms.TournamentRound).ThenInclude(tr => tr.Tournament).FirstOrDefaultAsync(ms => ms.Id.Equals(matchSeriesId)))?.TournamentRound?.Tournament?.Id;
      if (tournamentId.HasValue)
      {
        var tournamentSettings = await this.heroesOfTheStormContext.TournamentSettings.Include(ts => ts.TournamentMaps).ThenInclude(tm => tm.Map).FirstOrDefaultAsync(ts => ts.TournamentId.Equals(tournamentId.Value));
        if (tournamentSettings != null)
        {
          var dtoTournamentSettings = AutoMapper.Mapper.Map<DTO.Tournament.ResultMetaData>(tournamentSettings);
          heroesMetaData.Tournament = new DTO.MatchSeries.HeroesOfTheStormTournamentMatchResultMetaData(dtoTournamentSettings);
        }
      }

      result.Succeed(heroesMetaData);
      return result;
    }
  }
}
