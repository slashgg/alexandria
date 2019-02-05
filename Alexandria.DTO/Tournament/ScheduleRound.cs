using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.Tournament
{
  [DataContract]
  [JsonSchema("TournamentScheduleRound")]
  public class ScheduleRound
  {
    [DataMember(Name = "id")]
    public Guid Id { get; set; }
    [DataMember(Name = "name")]
    public string Name { get; set; }
    [DataMember(Name = "startDate")]
    public DateTimeOffset? StartDate { get; set; }
    [DataMember(Name = "endDate")]
    public DateTimeOffset? EndDate { get; set; }
    [DataMember(Name = "series")]
    public List<DTO.Tournament.MatchSeries> MatchSeries { get; set; } = new List<MatchSeries>();
    [DataMember(Name = "tournament")]
    public Tournament.Info Tournament { get; set; }
  }
}
