using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.UserProfile
{
  [DataContract]
  [JsonSchema("UserProfileExternalUserName")]
  public class ExternalUserName
  {
    [DataMember(Name = "userName")]
    public string UserName { get; set; }
    [DataMember(Name = "logoURL")]
    public string LogoURL { get; set; }
    [DataMember(Name = "serviceName")]
    public string ServiceName { get; set; }

    public ExternalUserName() { }

    public ExternalUserName(string userName, string logoURL, string serviceName)
    {
      this.UserName = userName;
      this.LogoURL = logoURL;
      this.ServiceName = serviceName;
    }
  }
}
