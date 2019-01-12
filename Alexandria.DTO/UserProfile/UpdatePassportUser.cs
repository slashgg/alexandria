using System.Runtime.Serialization;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.UserProfile
{
  [DataContract]
  [JsonSchema("UserProfileUpdatePassportUser")]
  public class UpdatePassportUser
  {
    [DataMember(Name = "email")]
    public string Email { get; set; }
    [DataMember(Name = "username")]
    public string UserName { get; set; }
  }
}
