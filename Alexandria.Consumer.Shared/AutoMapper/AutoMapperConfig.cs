using AutoMapper;
using AutoMapper.Configuration;

namespace Alexandria.Consumer.Shared.AutoMapper
{
  public class AutoMapperConfig
  {
    public static void Initialize()
    {
      var cfg = new MapperConfigurationExpression();
      Alexandria.Orchestration.Mapper.AutoMapperBase.Initialize(cfg);
      Alexandria.Games.HeroesOfTheStorm.Orchestration.Mapping.HeroesOfTheStormMapper.Initialize(cfg);
      Alexandria.Games.SuperSmashBros.Orchestration.Mapping.SuperSmashBrosMapper.Initialize(cfg);

      Mapper.Initialize(cfg);
    }
  }
}
