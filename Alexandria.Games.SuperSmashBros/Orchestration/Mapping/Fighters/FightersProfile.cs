using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;

namespace Alexandria.Games.SuperSmashBros.Orchestration.Mapping.Fighters
{
  public class FightersProfile : Profile
  {
    public FightersProfile()
    {
      CreateMap<EF.Models.Fighter, DTO.Fighters.Info>();
    }
  }
}
