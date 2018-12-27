using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;
using Alexandria.Shared.Enums;

namespace Alexandria.DTO.Team
{
  [DataContract]
  public class InviteRequest
  {
    [DataMember(Name = "invitee"), Required]
    public string Invitee { get; set; }
  }

  [DataContract]
  public class Invite
  {
    [DataMember(Name = "id")]
    public Guid Name { get; set; }
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
