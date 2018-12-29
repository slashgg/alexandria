using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Alexandria.DTO.Team;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.UserProfile
{
  [DataContract]
  [JsonSchema("UserProfileDetail")]
  public class Detail
  {
    [DataMember(Name = "email")]
    public string Email { get; set; }
    [DataMember(Name ="displayName")]
    public string DisplayName { get; set; }
    [DataMember(Name ="userName")]
    public string UserName { get; set; }
    [DataMember(Name = "avatarURL")]
    public string AvatarURL { get; set; }
    [DataMember(Name ="memberships")]
    public Dictionary<Guid, TeamMembership> Memberships { get; set; }
  }
}
