using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Alexandria.DTO.Team
{
  [DataContract]
  public class Membership
  {
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
