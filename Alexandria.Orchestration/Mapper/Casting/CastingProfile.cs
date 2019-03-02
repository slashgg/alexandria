using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;

namespace Alexandria.Orchestration.Mapper.Casting
{
  public class CastingProfile : Profile
  {
    public CastingProfile()
    {
      CreateMap<EF.Models.MatchSeries, DTO.Casting.CastableMatchSeries>()
        .IncludeBase<EF.Models.MatchSeries, DTO.MatchSeries.Detail>()
        .ForMember(dest => dest.Competition, opt => opt.MapFrom(src => src.TournamentRound.Tournament.Competition))
        .ForMember(dest => dest.RequiresApproval, opt => opt.MapFrom(src => src.CastingClaimRequired));

      CreateMap<EF.Models.MatchSeriesCasting, DTO.Casting.Cast>()
        .ForMember(dest => dest.StreamURL, opt => opt.MapFrom(src => src.StreamingURL))
        .ForMember(dest => dest.Competition, opt => opt.MapFrom(src => src.MatchSeries.TournamentRound.Tournament.Competition))
        .ForMember(dest => dest.CastMembers, opt => opt.MapFrom(src => src.MatchSeriesCastingParticipants));

      CreateMap<EF.Models.MatchSeriesCastingParticipation, DTO.Casting.CastMember>()
        .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserProfile.DisplayName));

    }
  }
}
