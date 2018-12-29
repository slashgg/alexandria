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
    [DataMember(Name ="teamId")]
    public Guid TeamId { get; set; }
    [DataMember(Name ="teamName")]
    public string TeamName { get; set; }
    [DataMember(Name ="role")]
    public string Role { get; set; }
    [DataMember(Name="permissions")]
    public List<string> Permissions { get; set; }
  }
}
