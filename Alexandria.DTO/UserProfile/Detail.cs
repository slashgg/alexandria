using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Alexandria.DTO.Team;

namespace Alexandria.DTO.UserProfile
{
  [DataContract]
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
