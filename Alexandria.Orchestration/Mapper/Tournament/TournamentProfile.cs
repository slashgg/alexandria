using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace Alexandria.Orchestration.Mapper.Tournament
{
  public class TournamentProfile : Profile
  {
    public TournamentProfile()
    {
      CreateMap<EF.Models.Tournament, DTO.Tournament.Info>();

      CreateMap<EF.Models.TournamentApplication, DTO.Tournament.TournamentApplication>()
        .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.TournamentApplicationQuestionAnswers));

      CreateMap<EF.Models.TournamentApplicationQuestionAnswer, DTO.Tournament.TournamentApplicationQuestionAnswer>();
      CreateMap<EF.Models.TournamentParticipation, DTO.Tournament.TeamParticipation>()
        .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.TeamId))
        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Team.Name))
        .ForMember(dest => dest.LogoURL, opt => opt.MapFrom(src => src.Team.LogoURL))
        .ForMember(dest => dest.Memberships, opt => opt.MapFrom(src => src.Team.TeamMemberships))
        .ForMember(dest => dest.Abbreviation, opt => opt.MapFrom(src => src.Team.Abbreviation));

      CreateMap<EF.Models.Tournament, DTO.Tournament.Detail>()
        .ForMember(dest => dest.Children, opt => opt.MapFrom(src => src.Tournaments.Select(AutoMapper.Mapper.Map<DTO.Tournament.Detail>)));


      CreateMap<EF.Models.MatchSeries, DTO.Tournament.MatchSeries>()
        .ForMember(dest => dest.Round, opt => opt.MapFrom(src => src.TournamentRound))
        .ForMember(dest => dest.GameCast, opt => opt.MapFrom(src => src.MatchSeriesCastings.FirstOrDefault()))
        .ForMember(dest => dest.Participants, opt => opt.MapFrom(src => src.MatchParticipants));

      CreateMap<EF.Models.MatchParticipant, DTO.Tournament.MatchSeriesParticipant>()
        .IncludeBase<EF.Models.MatchParticipant, DTO.MatchSeries.MatchSeriesParticipant>()
        .ForMember(dest => dest.TournamentRecord, opt => opt.MapFrom((src, dest, destMember, ctx) => ((Dictionary<Guid, DTO.Tournament.TournamentRecord>)ctx.Items["RecordVault"])[dest.Team.Id]));

      CreateMap<EF.Models.TournamentRound, DTO.Tournament.RoundDetail>()
        .ForMember(dest => dest.SeriesCount, opt => opt.MapFrom(src => src.SeriesPerRound));

      CreateMap<EF.Models.TournamentRound, DTO.Tournament.ScheduleRound>();
      CreateMap<EF.Models.Tournament, DTO.Tournament.Schedule>()
        .ForMember(src => src.Rounds, opt => opt.MapFrom(src => src.TournamentRounds));


      CreateMap<EF.Models.TeamMembership, DTO.Tournament.TeamMembership>()
        .IncludeBase<EF.Models.TeamMembership, DTO.Team.Membership>()
        .ForMember(dest => dest.ExternalUserName, opt => opt.MapFrom(src => src.UserProfile.GetUserNameForGame(src.Team.Competition.GameId)));
    }
  }
}
