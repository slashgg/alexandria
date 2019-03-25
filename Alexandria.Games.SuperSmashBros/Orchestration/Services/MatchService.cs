using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alexandria.EF.Context;
using Alexandria.Games.SuperSmashBros.DTO.MatchSeries;
using Alexandria.Games.SuperSmashBros.EF.Context;
using Alexandria.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Svalbard.Services;

namespace Alexandria.Games.SuperSmashBros.Orchestration.Services
{
  public class SuperSmashBrosMatchService
  {
    private readonly AlexandriaContext alexandriaContext;
    private readonly SuperSmashBrosContext superSmashBrosContext;
    private readonly IMatchService matchService;

    public SuperSmashBrosMatchService(IMatchService matchService, AlexandriaContext alexandriaContext, SuperSmashBrosContext superSmashBrosContext)
    {
      this.alexandriaContext = alexandriaContext;
      this.superSmashBrosContext = superSmashBrosContext;
      this.matchService = matchService;
    }

    public async Task<ServiceResult<DTO.MatchSeries.SuperSmashBrosMatchReportMetaData>> GetMatchResultMetaData(Guid matchSeriesId)
    {
      var result = new ServiceResult<DTO.MatchSeries.SuperSmashBrosMatchReportMetaData>();
      var originalMetaData = await this.matchService.GetResultSubmitMetaData(matchSeriesId);

      if (!originalMetaData.Success)
      {
        result.Error = originalMetaData.Error;
        return result;
      }

      var smashBrosMetaData = AutoMapper.Mapper.Map<DTO.MatchSeries.SuperSmashBrosMatchReportMetaData>(originalMetaData.Data);
      var fighters = (await this.superSmashBrosContext.Fighters.ToListAsync()).Select(AutoMapper.Mapper.Map<DTO.Fighters.Info>).ToList();
      smashBrosMetaData.GameSpecific.Fighters = fighters;

      result.Succeed(smashBrosMetaData);
      return result;
    }

  }
}
