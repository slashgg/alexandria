using System.Runtime.Serialization;
using Alexandria.Shared.Enums;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.UserProfile
{
  [DataContract]
  [JsonSchema("UserProfileConnectionDetail")]
  public class ConnectionDetail
  {
    [DataMember(Name = "provider")]
    public ExternalProvider Provider { get; set; }

    [DataMember(Name = "name")]
    public string Name { get; set; }

    [DataMember(Name = "externalId")]
    public string ExternalId { get; set; }
  }
}
