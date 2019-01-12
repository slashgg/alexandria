using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.UserProfile
{
  [DataContract]
  [JsonSchema("UserProfileResendEmailVerification")]
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
