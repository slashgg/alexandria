﻿using AutoMapper;

namespace Alexandria.Orchestration.Mapper.UserProfile
{
  public class UserProfileProfile : Profile
  {
    public UserProfileProfile()
    {
      this.CreateMap<EF.Models.UserProfile, DTO.UserProfile.Detail>()
        .ForMember(dest => dest.Memberships, opt => opt.MapFrom(src => src.TeamMemberships))
        .ForMember(dest => dest.Birthday, opt => opt.MapFrom(src => src.Birthday.HasValue ? src.Birthday.Value.ToString("yyyy-MM-dd") : string.Empty));

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
      this.CreateMap<EF.Models.ExternalAccount, DTO.UserProfile.ConnectionDetail>();

      this.CreateMap<EF.Models.FavoriteCompetition, DTO.UserProfile.FavoriteCompetition>()
        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Competition.Name));

      CreateMap<EF.Models.ExternalUserName, DTO.UserProfile.ExternalUserName>();
    }
  }
}
