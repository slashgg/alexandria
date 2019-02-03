using AutoMapper;

namespace Alexandria.Orchestration.Mapper.Game
{
  public class GameProfile : Profile
  {
    public GameProfile()
    {
      CreateMap<EF.Models.Game, DTO.Game.Detail>();
      CreateMap<EF.Models.Game, DTO.Game.Info>();
    }
  }
}
