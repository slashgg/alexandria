using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.Team
{
  [DataContract]
  [JsonSchema("TeamMembership")]
  public class Membership
  {
    [DataMember(Name = "id")]
    public Guid Id { get; set; }
    [DataMember(Name = "userId")]
    public Guid UserId { get; set; }
    [DataMember(Name = "displayName")]
    public string DisplayName { get; set; }
    [DataMember(Name = "userName")]
    public string UserName { get; set; }
    [DataMember(Name = "role")]
    public string Role { get; set; }
    [DataMember(Name = "memberSince")]
    public DateTime MemberSince { get; set; }
  }
}
