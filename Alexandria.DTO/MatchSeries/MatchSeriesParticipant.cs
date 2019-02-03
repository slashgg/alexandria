using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.MatchSeries
{
  [DataContract]
  [JsonSchema("MatchMatchSeriesParticipant")]
  public class MatchSeriesParticipant
  {
    [DataMember(Name = "id")]
    public Guid Id { get; set; }
    [DataMember(Name = "team")]
    public Team.Info Team { get; set; } = new Team.Info();
    [DataMember(Name = "matchResults")]
    public List<MatchResult> MatchResults { get; set; } = new List<MatchResult>();
    public int Wins { get; set; }
    public int Losses { get; set; }
    public int Draws { get; set; }
  }
}
