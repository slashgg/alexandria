using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Alexandria.DTO.UserProfile
{
  [DataContract]
  public class ResendEmailVerification
  {
    [DataMember(Name = "email")]
    public string Email { get; set; }

    public ResendEmailVerification(string email)
    {
      Email = email;
    }
  }
}
