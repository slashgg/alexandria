using AutoMapper;

namespace Alexandria.Orchestration.Mapper
{
  public class TeamProfile : Profile
  {
    public TeamProfile()
    {
      this.CreateMap<EF.Models.Team, DTO.Team.Detail>()
        .ForMember(dest => dest.Competition, opt => opt.MapFrom(src => src.Competition))
        .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.TeamMemberships));

      this.CreateMap<EF.Models.TeamMembership, DTO.Team.Membership>()
        .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.TeamRole.Name))
        .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.UserProfile.DisplayName))
        .ForMember(dest => dest.AvatarURL, opt => opt.MapFrom(src => src.UserProfile.AvatarURL))
        .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserProfileId))
        .ForMember(dest => dest.MemberSince, opt => opt.MapFrom(src => src.CreatedAt));

      this.CreateMap<EF.Models.TeamInvite, DTO.Team.Invite>()
        .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserProfile.UserName))
        .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

      this.CreateMap<EF.Models.Competition, DTO.Team.Detail.CompetitionData>();
    }
  }
}
