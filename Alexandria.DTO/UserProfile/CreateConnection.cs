using System.Runtime.Serialization;
using Alexandria.Shared.Enums;

namespace Alexandria.DTO.UserProfile
{
  [DataContract]
  public class CreateConnection
  {
    [DataMember(Name = "userId")]
    public string UserId { get; set; }

    [DataMember(Name = "externalId")]
    public string ExternalId { get; set; }

    [DataMember(Name = "provider")]
    public ExternalProvider Provider { get; set; }

    [DataMember(Name = "name")]
    public string Name { get; set; }
  }
}
