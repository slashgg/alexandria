using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.Casting
{
  [DataContract]
  [JsonSchema("CastableMatchSeries")]
  public class CastableMatchSeries : MatchSeries.Detail
  {
    [DataMember(Name = "requiresApproval")]
    public bool RequiresApproval { get; set; }
    [DataMember(Name = "competition")]
    public Competition.Info Competition { get; set; }
  }
}
