using Alexandria.Games.SuperSmashBros.Orchestration.Mapping.Fighters;
using Alexandria.Games.SuperSmashBros.Orchestration.Mapping.MatchSeries;
using AutoMapper;

namespace Alexandria.Games.SuperSmashBros.Orchestration.Mapping
{
  public static class SuperSmashBrosMapper
  {
    public static void Initialize(IMapperConfigurationExpression cfg)
    {
      cfg.AddProfile<FightersProfile>();
      cfg.AddProfile<MatchSeriesProfile>();
    }
  }
}
