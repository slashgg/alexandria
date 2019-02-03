using System;
using System.Collections.Generic;
using System.Text;
using Alexandria.DTO.Tournament;
using Alexandria.EF.Models;
using AutoMapper;

namespace Alexandria.Orchestration.Mapper.Tournament.Converters
{
  public class MatchSeriesParticipantConverter : ITypeConverter<EF.Models.MatchParticipant, DTO.Tournament.MatchSeriesParticipant>
  {
    public MatchSeriesParticipant Convert(MatchParticipant source, MatchSeriesParticipant destination, ResolutionContext context)
    {
      if (source == null)
      {
        return null;
      }

      var participant = new MatchSeriesParticipant();
      participant.Id = source.Id;
      return participant;
    }
  }
}
