using Alexandria.Orchestration.Mapper.Competition;
using Alexandria.Orchestration.Mapper.Game;
using Alexandria.Orchestration.Mapper.Marketing;
using Alexandria.Orchestration.Mapper.Tournament;
using Alexandria.Orchestration.Mapper.UserProfile;
using Alexandria.Orchestration.Mapper.Utility;

namespace Alexandria.Orchestration.Mapper
{
  public static class AutoMapperConfig
  {
    public static void Initialize()
    {
      AutoMapper.Mapper.Initialize(cfg =>
      {
        cfg.AddProfile<UserProfileProfile>();
        cfg.AddProfile<TeamProfile>();
        cfg.AddProfile<CompetitionProfile>();
        cfg.AddProfile<TournamentProfile>();
        cfg.AddProfile<MarketingProfile>();
        cfg.AddProfile<GameProfile>();
        cfg.AddProfile<UtilityProfile>();
      });
    }
  }
}
