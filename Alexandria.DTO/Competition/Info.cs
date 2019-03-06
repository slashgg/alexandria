using System;
using System.Runtime.Serialization;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.Competition
{
  [DataContract]
  [JsonSchema("CompetitionInfo")]
  public class Info
  {
    [DataMember(Name = "id")]
    public Guid Id { get; set; }
    [DataMember(Name = "name")]
    public string Name { get; set; }
    [DataMember(Name = "slug")]
    public string Slug { get; set; }
  }
}
