using AutoMapper;

namespace Alexandria.Orchestration.Mapper.Competition
{
  public class CompetitionProfile : Profile
  {
    public CompetitionProfile()
    {
      CreateMap<EF.Models.Competition, DTO.Competition.Detail>()
        .ForMember(dest => dest.FormattedName, opt => opt.MapFrom(src => src.Name.Remove(0, 1)))
        .ForMember(dest => dest.Game, opt => opt.MapFrom(src => src.Game));

      CreateMap<EF.Models.Game, DTO.Competition.Detail.GameData>();
    }
  }
}
