using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;
using Alexandria.Shared.Validation;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.Admin.Competition
{
  [DataContract]
  [JsonSchema("AdminCompetitionCreate")]
  public class CreateData
  {
    [Required, EFUnique("Competitions", "Name")]
    [DataMember(Name = "name")]
    public string Name { get; set; }
    [Required, EFUnique("Competitions", "Slug")]
    [DataMember(Name = "slug")]
    public string Slug { get; set; }
    [Required, EFUnique("Competitions", "Title")]
    [DataMember(Name = "title")]
    public string Title { get; set; }
    [DataMember(Name = "description")]
    public string Description { get; set; }
    [DataMember(Name = "maxTeamSize")]
    public int? MaxTeamSize { get; set; }
    [Required, MinValue(1)]
    [DataMember(Name = "minTeamSize")]
    public int MinTeamSize { get; set; } = 1;
    [Required]
    [DataMember(Name = "competitionLevelId")]
    public Guid CompetitionLevelId { get; set; }
    [DataMember(Name = "ruleSlug")]
    public string RuleSlug { get; set; }
  }
}
