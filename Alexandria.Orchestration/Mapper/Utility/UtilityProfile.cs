using AutoMapper;

namespace Alexandria.Orchestration.Mapper.Utility
{
  public class UtilityProfile : Profile
  {
    public UtilityProfile()
    {
      CreateMap<EF.Models.ProfanityFilter, DTO.Util.ProfanityFilter.ProfanityFilter>();
    }
  }
}
