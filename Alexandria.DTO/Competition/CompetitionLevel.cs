using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.Competition
{
  [DataContract]
  [JsonSchema("CompetitionLevel")]
  public class CompetitionLevel
  {
    [DataMember(Name = "id")]
    public Guid Id { get; set; }
    [DataMember(Name = "name")]
    public string Name { get; set; }
    [DataMember(Name = "level")]
    public int Level { get; set; }
  }
}
