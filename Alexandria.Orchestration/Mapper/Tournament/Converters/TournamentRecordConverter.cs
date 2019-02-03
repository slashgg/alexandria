using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;

namespace Alexandria.Orchestration.Mapper.Tournament.Converters
{
  public class TournamentRecordConverter : IValueConverter<EF.Models.MatchParticipant, DTO.Tournament.TournamentRecord>
  {
    public DTO.Tournament.TournamentRecord Convert(EF.Models.MatchParticipant sourceMember, ResolutionContext context)
    {
      var record = new DTO.Tournament.TournamentRecord();
      //var matches = sourceMember.MatchSeries.TournamentRound.Tournament.
      //record.Draws = D
      return record;
    }
  }
}
