using System.Runtime.Serialization;

namespace Alexandria.DTO.UserProfile
{
  [DataContract]
  public class UpdatePassportUser
  {
    [DataMember(Name = "email")]
    public string Email { get; set; }
    [DataMember(Name = "username")]
    public string UserName { get; set; }
  }
}
