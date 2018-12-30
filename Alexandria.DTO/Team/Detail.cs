using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Alexandria.Shared.Enums;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.Team
{
  [DataContract]
  [JsonSchema("TeamDetail")]
  public class Detail
  {
    [DataMember(Name = "id")]
    public Guid Id { get; set; }
    [DataMember(Name = "name")]
    public string Name { get; set; }
    [DataMember(Name = "slug")]
    public string Slug { get; set; }
    [DataMember(Name = "abbreviation")]
    public string Abbreviation { get; set; }
    [DataMember(Name = "members")]
    public List<Membership> Members { get; set; }
    [DataMember(Name = "state")]
    public TeamState TeamState { get; set; }
    [DataMember(Name = "logoURL")]
    public string LogoURL { get; set; }
    [DataMember(Name = "competition")]
    public CompetitionData Competition { get; set; } = new CompetitionData();

    [DataContract]
    [JsonSchema("TeamCompetitionDetail")]
    public class CompetitionData
    {
      [DataMember(Name = "name")]
      public string Name { get; set; }
      [DataMember(Name ="id")]
      public Guid Id { get; set; }
    }
  }
}
