using System.Linq;
using AutoMapper;

namespace Alexandria.Orchestration.Mapper.Competition
{
  public class CompetitionProfile : Profile
  {
    public CompetitionProfile()
    {
      CreateMap<EF.Models.Competition, DTO.Competition.Detail>()
        .ForMember(dest => dest.RulesSlug, opt => opt.MapFrom(src => src.RulesSlug))
        .ForMember(dest => dest.TeamCount, opt => opt.MapFrom(src => src.Teams.Count(t => t.TeamState == Shared.Enums.TeamState.Active)))
        .ForMember(dest => dest.PlayerCount, opt => opt.MapFrom(src => src.Teams.Where(t => t.TeamState == Shared.Enums.TeamState.Active).SelectMany(t => t.TeamMemberships).Count()))
        .ForMember(dest => dest.Game, opt => opt.MapFrom(src => src.Game));

      CreateMap<EF.Models.Game, DTO.Competition.Detail.GameData>();
      CreateMap<EF.Models.Tournament, DTO.Competition.Tournament>();

      CreateMap<EF.Models.Tournament, DTO.Competition.TournamentApplication>()
        .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.TournamentApplicationQuestions))
        .ForMember(dest => dest.Tournament, opt => opt.MapFrom(src => src));

      CreateMap<EF.Models.Tournament, DTO.Competition.TournamentApplication.TournamentData>();
      CreateMap<EF.Models.TournamentApplicationQuestion, DTO.Competition.TournamentApplicationQuestion>()
        .ForMember(dest => dest.Question, opt => opt.MapFrom(src => src.QuestionKey));
    }
  }
}
