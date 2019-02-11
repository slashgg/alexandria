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
      CreateMap<EF.Models.MatchSeries, DTO.MatchSeries.Detail>();
      CreateMap<EF.Models.MatchParticipant, DTO.MatchSeries.MatchSeriesParticipant>()
        .ForMember(dest => dest.MatchResults, opt => opt.MapFrom(src => src.Results))
        .ForMember(dest => dest.Wins, opt => opt.MapFrom(src => src.Wins.Count()))
        .ForMember(dest => dest.Losses, opt => opt.MapFrom(src => src.Losses.Count()))
        .ForMember(dest => dest.Draws, opt => opt.MapFrom(src => src.Draws.Count()));

      CreateMap<EF.Models.MatchSeriesScheduleRequest, DTO.MatchSeries.ScheduleRequest>()
        .ForMember(dest => dest.MatchSeries, opt => opt.MapFrom(src => src.MatchSeries));

      CreateMap<EF.Models.MatchSeries, DTO.MatchSeries.MatchReportMetaData>()
        .ForMember(dest => dest.Game, opt => opt.MapFrom(src => src.Game.InternalIdentifier))
        .ForMember(dest => dest.Participants, opt => opt.MapFrom(src => src.MatchParticipants.Select(mp => mp.Team)));

      CreateMap<EF.Models.Match, DTO.MatchSeries.MatchInfo>()
        .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.MatchOrder));
    }
  }
}
