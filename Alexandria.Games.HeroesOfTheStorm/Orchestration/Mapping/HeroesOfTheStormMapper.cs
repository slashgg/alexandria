using Alexandria.Games.HeroesOfTheStorm.Orchestration.Mapping.MatchSeries;
using Alexandria.Orchestration.Mapper.Games;
using AutoMapper;

namespace Alexandria.Games.HeroesOfTheStorm.Orchestration.Mapping
{
  public static class HeroesOfTheStormMapper
  {
    public static void Initialize(IMapperConfigurationExpression cfg)
    {
      cfg.AddProfile<RankingProfile>();
      cfg.AddProfile<MatchProfile>();
    }
  }
}
