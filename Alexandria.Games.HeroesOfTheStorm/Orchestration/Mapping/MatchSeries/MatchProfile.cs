using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;

namespace Alexandria.Games.HeroesOfTheStorm.Orchestration.Mapping.MatchSeries
{
  public class MatchProfile : Profile
  {
    public MatchProfile()
    {
      this.CreateMap<Alexandria.DTO.MatchSeries.MatchReportMetaData, DTO.MatchSeries.HeroesOfTheStormMatchReportMetaData>()
        .AfterMap((src, dest) => dest.SubmitURL = dest.CreateSubmitURL());
      this.CreateMap<EF.Models.TournamentSettings, DTO.Tournament.ResultMetaData>()
        .ForMember(dest => dest.MapPool, opt => opt.MapFrom(src => src.TournamentMaps));
      this.CreateMap<EF.Models.TournamentMap, DTO.Map.Info>()
        .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Map.Id))
        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Map.Name));
    }
  }
}
