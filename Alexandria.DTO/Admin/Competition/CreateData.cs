using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.Admin.Competition
{
  [DataContract]
  [JsonSchema("AdminCompetitionCreate")]
  public class CreateData
  {
    [DataMember(Name = "name")]
    public string Name { get; set; }
    [DataMember(Name = "slug")]
    public string Slug { get; set; }
    [DataMember(Name = "title")]
    public string Title { get; set; }
    [DataMember(Name = "description")]
    public string Description { get; set; }
    [DataMember(Name = "maxTeamSize")]
    public int? MaxTeamSize { get; set; }
    [DataMember(Name = "minTeamSize")]
    public int MinTeamSize { get; set; } = 1;
    [DataMember(Name = "competitionLevelId")]
    public Guid CompetitionLevelId { get; set; }
    [DataMember(Name = "ruleSlug")]
    public string RuleSlug { get; set; }
  }
}
