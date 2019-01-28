using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;

namespace Alexandria.Orchestration.Mapper.Games
{
  public class RankingProfile : Profile
  {
    public RankingProfile()
    {
      CreateMap<ExternalServices.HOTSLogs.DTO.LeaderboardRanking, Alexandria.Games.HeroesOfTheStorm.EF.Models.ExternalRanking>()
        .ForMember(dest => dest.Ranking, opt => opt.MapFrom(src => src.CurrentMMR))
        .ForMember(dest => dest.GameMode, opt => opt.MapFrom(src => src.GameMode))
        .ForMember(dest => dest.MMRSource, opt => opt.MapFrom(src => Shared.Enums.MMRSource.HOTSLogs))
        .ForMember(dest => dest.BattleNetRegion, opt => opt.MapFrom(src => src.LeagueId))
        .ForMember(dest => dest.UserProfileId, opt => opt.MapFrom((src, dest, destMember, ctx) => ctx.Items["UserProfileId"]));
    }
  }
}
