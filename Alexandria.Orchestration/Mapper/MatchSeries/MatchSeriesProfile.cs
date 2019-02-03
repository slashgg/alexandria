using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;

namespace Alexandria.Orchestration.Mapper.MatchSeries
{
  public class MatchSeriesProfile : Profile
  {
    public MatchSeriesProfile()
    {
      CreateMap<EF.Models.MatchParticipantResult, DTO.MatchSeries.MatchResult>();
      CreateMap<EF.Models.MatchParticipant, DTO.MatchSeries.MatchSeriesParticipant>()
        .ForMember(dest => dest.MatchResults, opt => opt.MapFrom(src => src.Results))
        .ForMember(dest => dest.Wins, opt => opt.MapFrom(src => src.Wins.Count()))
        .ForMember(dest => dest.Losses, opt => opt.MapFrom(src => src.Losses.Count()))
        .ForMember(dest => dest.Draws, opt => opt.MapFrom(src => src.Draws.Count()));
    }
  }
}
