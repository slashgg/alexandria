using System.Runtime.Serialization;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.Tournament
{
  [DataContract]
  [JsonSchema("TournamentMatchSeriesParticipant")]
  public class MatchSeriesParticipant : DTO.MatchSeries.MatchSeriesParticipant
  {
    [DataMember(Name = "tournamentRecord")]
    public TournamentRecord TournamentRecord { get; set; } = new DTO.Tournament.TournamentRecord();
  }
}
