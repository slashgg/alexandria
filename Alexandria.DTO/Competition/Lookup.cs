using System;
using System.Runtime.Serialization;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.Competition
{
  [DataContract]
  [JsonSchema("CompetitionLookup")]
  public class Lookup
  {
    [DataMember(Name ="competitionLevel")]
    public Guid? CompetitionLevel { get; set; } = null;
    [DataMember(Name = "teamSize")]
    public int? TeamSize { get; set; } = null;
    [DataMember(Name = "game")]
    public Guid? Game { get; set; } = null;
    [DataMember(Name = "active")]
    public bool Active { get; set; } = true;
  }
}
