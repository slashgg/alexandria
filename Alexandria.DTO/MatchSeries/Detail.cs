using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Alexandria.Shared.Enums;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.MatchSeries
{
  [DataContract]
  [JsonSchema("MatchSeriesDetail")]
  public class Detail
  {
    [DataMember(Name = "id")]
    public Guid Id { get; set; }
    [DataMember(Name = "scheduledAt")]
    public DateTimeOffset? ScheduledAt { get; set; }
    [DataMember(Name = "game")]
    public Game.Info Game { get; set; }
    [DataMember(Name = "state")]
    public MatchState State { get; set; }
    [DataMember(Name = "participants")]
    public virtual List<MatchSeriesParticipant> Participants { get; set; } = new List<MatchSeriesParticipant>();
  }
}
