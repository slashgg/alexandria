using System.Runtime.Serialization;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.UserProfile
{
  [DataContract]
  [JsonSchema("UserProfileUpdateAvatar")]
  public class UpdateAvatar
  {
    [DataMember(Name = "correlationId")]
    public uint PresignedUrlCorrelationId { get; set; }
  }
}
