using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Alexandria.Shared.Enums;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.MatchSeries
{
  [DataContract]
  [JsonSchema("MatchSeriesScheduleRequest")]
  public class ScheduleRequest
  {
    [DataMember(Name = "id")]
    public Guid Id { get; set; }
    [DataMember(Name = "proposedTmeSlot")]
    public DateTimeOffset ProposedTimeSlot { get; set; }
    [DataMember(Name = "originTeam")]
    public Team.Info OriginTeam { get; set; }
    [DataMember(Name = "targetTeam")]
    public Team.Info TargetTeam { get; set; }
    [DataMember(Name = "match")]
    public MatchSeries.Detail Match { get; set; }
    [DataMember(Name = "matchType")]
    public MatchSeriesType MatchType { get; set; }
  }
}
