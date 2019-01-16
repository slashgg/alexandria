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
      CreateMap<EF.Models.TournamentParticipation, DTO.Tournament.TeamParticipation>()
        .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.TeamId))
        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Team.Name))
        .ForMember(dest => dest.LogoURL, opt => opt.MapFrom(src => src.Team.LogoURL))
        .ForMember(dest => dest.Abbreviation, opt => opt.MapFrom(src => src.Team.Abbreviation));
    }
  }
}
