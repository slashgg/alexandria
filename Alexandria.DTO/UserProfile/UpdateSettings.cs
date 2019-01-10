using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Alexandria.DTO.UserProfile
{
  [DataContract]
  public class UpdateSettings
  {
    [DataMember(Name = "username")]
    public string Username { get; set; }
    [DataMember(Name = "email")]
    public string Email { get; set; }
    [DataMember(Name = "dob")]
    public DateTimeOffset? DateOfBirth { get; set; }
  }
}
