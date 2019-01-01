using Alexandria.Orchestration.Mapper.UserProfile.Converter;
using AutoMapper;

namespace Alexandria.Orchestration.Mapper.UserProfile
{
  public class UserProfileProfile : Profile
  {
    public UserProfileProfile()
    {
      this.CreateMap<EF.Models.UserProfile, DTO.UserProfile.Detail>()
        .ForMember(dest => dest.Memberships, opt => opt.MapFrom(src => src.TeamMemberships));

      this.CreateMap<EF.Models.TeamMembership, DTO.UserProfile.TeamMembership>()
        .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.TeamRole.Name))
        .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.TeamRole.Permissions))
        .ForMember(dest => dest.Team, opt => opt.MapFrom(src => src.Team));

      this.CreateMap<EF.Models.Team, DTO.UserProfile.TeamMembership.TeamData>();

      this.CreateMap<EF.Models.TeamInvite, DTO.UserProfile.TeamInvite>()
        .ForMember(dest => dest.Team, opt => opt.MapFrom(src => src.Team.Name))
        .ForMember(dest => dest.CompetitionSlug, opt => opt.MapFrom(src => src.Team.Competition.Slug))
        .ForMember(dest => dest.TeamId, opt => opt.MapFrom(src => src.TeamId));

      this.CreateMap<EF.Models.Competition, DTO.UserProfile.TeamMembership.CompetitionData>();
    }
  }
}
