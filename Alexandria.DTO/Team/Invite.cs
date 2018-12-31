using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;
using Alexandria.Shared.Enums;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.Team
{
  [DataContract]
  [JsonSchema("TeamInviteRequest")]
  public class InviteRequest
  {
    [DataMember(Name = "invitee"), Required]
    public string Invitee { get; set; }
  }

  [DataContract]
  [JsonSchema("userProfileTeamInvite")]
  public class Invite
  {
    [DataMember(Name = "id")]
    public Guid Id { get; set; }
    [DataMember(Name = "createdAt")]
    public DateTime CreatedAt { get; set; }
    [DataMember(Name = "state")]
    public InviteState State { get; set; }
    [DataMember(Name = "userName")]
    public string UserName { get; set; }
    [DataMember(Name = "email")]
    public string Email { get; set; }
  }
}
