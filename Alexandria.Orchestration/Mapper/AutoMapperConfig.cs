using Alexandria.Orchestration.Mapper.Casting;
using Alexandria.Orchestration.Mapper.Competition;
using Alexandria.Orchestration.Mapper.Game;
using Alexandria.Orchestration.Mapper.Marketing;
using Alexandria.Orchestration.Mapper.MatchSeries;
using Alexandria.Orchestration.Mapper.Tournament;
using Alexandria.Orchestration.Mapper.UserProfile;
using Alexandria.Orchestration.Mapper.Utility;
using AutoMapper;

namespace Alexandria.Orchestration.Mapper
{
  public static class AutoMapperBase
  {
    public static void Initialize(IMapperConfigurationExpression cfg)
    {
      cfg.AddProfile<UserProfileProfile>();
      cfg.AddProfile<TeamProfile>();
      cfg.AddProfile<CompetitionProfile>();
      cfg.AddProfile<TournamentProfile>();
      cfg.AddProfile<MarketingProfile>();
      cfg.AddProfile<GameProfile>();
      cfg.AddProfile<UtilityProfile>();
      cfg.AddProfile<MatchSeriesProfile>();
      cfg.AddProfile<CastingProfile>();
    }
  }
}
