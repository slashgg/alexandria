using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Alexandria.Shared.Enums;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.UserProfile
{
  [DataContract]
  [JsonSchema("TeamTeamInvite")]
  public class TeamInvite
  {
    [DataMember(Name = "id")]
    public Guid Id { get; set; }
    [DataMember(Name = "createdAt")]
    public DateTime CreatedAt { get; set; }
    [DataMember(Name = "teamId")]
    public Guid TeamId { get; set; }
    [DataMember(Name = "team")]
    public string Team { get; set; }
    [DataMember(Name = "state")]
    public InviteState State { get; set; }
  }
}
