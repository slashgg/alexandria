using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Alexandria.Shared.Enums;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.MatchSeries
{
  [DataContract]
  [JsonSchema("MatchSeriesMatchInfo")]
  public class MatchInfo
  {
    [DataMember(Name = "id")]
    public Guid Id { get; set; }
    [DataMember(Name = "order")]
    public int Order { get; set; }
    [DataMember(Name = "state")]
    public MatchState State { get; set; }
  }
}
