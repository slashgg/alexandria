using System;
using System.Runtime.Serialization;
using Alexandria.Shared.Enums;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.MatchSeries
{
  [DataContract]
  [JsonSchema("MatchSeriesMatchResult")]
  public class MatchResult
  {
    [DataMember(Name = "order")]
    public int MatchOrder { get; set; }
    [DataMember(Name ="matchId")]
    public Guid MatchId { get; set; }
    [DataMember(Name = "outcome")]
    public MatchOutcome MatchOutCome { get; set; }
  }
}
