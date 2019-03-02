using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.Casting
{
  [DataContract]
  [JsonSchema("CastingCreateCastData")]
  public class CreateCastData
  {
    [DataMember(Name = "streamURL")]
    public string StreamURL { get; set; }
    [DataMember(Name = "VODURL")]
    public string VODURL { get; set; }
    [DataMember(Name = "matchSeriesId")]
    public Guid MatchSeriesId { get; set; }
    [DataMember(Name = "startsAt")]
    public DateTimeOffset? StartsAt { get; set; }
  }
}
