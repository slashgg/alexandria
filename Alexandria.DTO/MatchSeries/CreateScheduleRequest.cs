using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Alexandria.Shared.Enums;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.MatchSeries
{
  [DataContract]
  [JsonSchema("MatchSeriesCreateScheduleRequest")]
  public class CreateScheduleRequest
  {
    [DataMember(Name = "targetTeamId")]
    public Guid TargetTeamId { get; set; }
    [DataMember(Name = "proposedTimeSlot")]
    public DateTimeOffset ProposedTimeSlot { get; set; }
    [DataMember(Name = "matchSeriesId")]
    public Guid? MatchSeriesId { get; set; }
    [DataMember(Name = "matchType")]
    public MatchSeriesType MatchType { get; set; }
  }
}
