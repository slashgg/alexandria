using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.UserProfile
{
  [DataContract]
  [JsonSchema("TeamTeamMembership")]
  public class TeamMembership
  {
    [DataMember(Name = "id")]
    public Guid Id { get; set; }
    [DataMember(Name ="role")]
    public string Role { get; set; }
    [DataMember(Name="permissions")]
    public List<string> Permissions { get; set; }
    [DataMember(Name = "team")]
    public TeamData Team { get; set; } = new TeamData();

    [JsonSchema("UserProfileTeamMembershipCompetitionData")]
    [DataContract]
    public class CompetitionData
    {
      [DataMember(Name = "id")]
      public Guid Id { get; set; }
      [DataMember(Name = "slug")]
      public string Slug { get; set; }
      [DataMember(Name = "name")]
      public string Name { get; set; }
    }

    [JsonSchema("UserProfileTamMembershipTeamData")]
    [DataContract]
    public class TeamData
    {
      [DataMember(Name = "id")]
      public Guid Id { get; set; }
      [DataMember(Name = "slug")]
      public string Slug { get; set; }
      [DataMember(Name = "name")]
      public string Name { get; set; }
      [DataMember(Name = "competition")]
      public CompetitionData Competition { get; set; } = new CompetitionData();
    }
  }
}
