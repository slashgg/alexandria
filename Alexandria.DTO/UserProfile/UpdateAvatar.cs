using System.Runtime.Serialization;

namespace Alexandria.DTO.UserProfile
{
  [DataContract]
  public class UpdateAvatar
  {
    [DataMember(Name = "correlationId")]
    public uint PresignedUrlCorrelationId { get; set; }
  }
}
