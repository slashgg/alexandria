using System;
using System.Collections.Generic;
using System.Text;
using Alexandria.Games.SuperSmashBros.DTO.MatchSeries;
using AutoMapper;
using Microsoft.AspNetCore.Antiforgery.Internal;

namespace Alexandria.Games.SuperSmashBros.Orchestration.Mapping.MatchSeries
{
  public class MatchSeriesProfile : Profile
  {
    public MatchSeriesProfile()
    {
      CreateMap<Alexandria.DTO.MatchSeries.MatchReportMetaData, DTO.MatchSeries.SuperSmashBrosMatchReportMetaData>()
        .ForMember(dest => dest.GameSpecific, opt => opt.UseDestinationValue())
        .AfterMap((src, dest) => dest.ReportingType = "versus")
        .AfterMap((src, dest) => dest.SubmitURL = dest.CreateSubmitURL());
    }
  }
}
