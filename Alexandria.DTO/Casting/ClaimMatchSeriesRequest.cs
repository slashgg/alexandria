using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.Casting
{
  [DataContract]
  [JsonSchema("CastingClaimMatchSeriesRequest")]
  public class ClaimMatchSeriesRequest
  {
    [DataMember(Name = "matchSeriesId")]
    public Guid MatchSeriesId { get; set; }
  }
}
