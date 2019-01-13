using Alexandria.Orchestration.Mapper.Marketing.Converters;
using AutoMapper;

namespace Alexandria.Orchestration.Mapper.Marketing
{
  public class MarketingProfile : Profile
  {
    public MarketingProfile()
    {
      this.CreateMap<EF.Models.UserProfile, DTO.Marketing.Contact>()
        .ConvertUsing(new ContactConverter());
    }
  }
}
