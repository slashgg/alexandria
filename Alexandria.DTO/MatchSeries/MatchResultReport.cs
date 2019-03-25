using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Alexandria.Shared.Enums;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.MatchSeries
{
  [DataContract]
  [JsonSchema("MatchSeriesResultReportingRequest")]
  public class MatchResultReport
  {
    [DataMember(Name = "outcome")]
    public MatchOutcomeState Outcome { get; set; }
    [DataMember(Name = "matchId")]
    public Guid? MatchId { get; set; }
    [DataMember(Name = "results")]
    public List<MatchParticipantResult> Results { get; set; }
    [DataMember(Name = "gameSpecific")]
    public virtual object GameSpecific { get; set; }

    public class MatchParticipantResult
    {
      [DataMember(Name = "teamId")]
      public Guid TeamId { get; set; }
      [DataMember(Name = "result")]
      public MatchOutcome Result { get; set; }
    }
  }
}
