using Alexandria.Orchestration.Mapper.UserProfile;

namespace Alexandria.Orchestration.Mapper
{
  public static class AutoMapperConfig
  {
    public static void Initialize()
    {
      AutoMapper.Mapper.Initialize(cfg =>
      {
        cfg.AddProfile<UserProfileProfile>();
      });
    }
  }
}
