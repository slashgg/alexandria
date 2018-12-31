using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;

namespace Alexandria.Orchestration.Mapper.Tournament
{
  public class TournamentProfile : Profile
  {
    public TournamentProfile()
    {
      CreateMap<EF.Models.TournamentApplication, DTO.Tournament.TournamentApplication>()
        .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.TournamentApplicationQuestionAnswers));

      CreateMap<EF.Models.TournamentApplicationQuestionAnswer, DTO.Tournament.TournamentApplicationQuestionAnswer>();
    }
  }
}
