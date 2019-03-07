using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.Tournament
{
  [DataContract]
  [JsonSchema("TournamentMatchSeries")]
  public class MatchSeries : DTO.MatchSeries.Detail
  {
    [DataMember(Name = "round")]
    public RoundDetail Round { get; set; }

    [DataMember(Name = "participants")]
    public new List<MatchSeriesParticipant> Participants { get; set; } = new List<MatchSeriesParticipant>();
    [DataMember(Name = "cast")]
    public DTO.Casting.Cast GameCast { get; set; }
  }
}
