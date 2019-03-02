using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Alexandria.Shared.Enums;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.Casting
{
  [DataContract]
  [JsonSchema("CastingCastMember")]
  public class CastMember
  {
    [DataMember(Name = "id")]
    public Guid Id { get; set; }
    [DataMember(Name = "role")]
    public CastingRole Role { get; set; }
    [DataMember(Name = "username")]
    public string UserName { get; set; }
    [DataMember(Name = "userId")]
    public Guid UserProfileId { get; set; }
  }
}
