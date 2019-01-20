using System.Runtime.Serialization;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.Team
{
  [DataContract]
  [JsonSchema("TeamUpdateLogo")]
  public class UpdateLogo
  {
    [DataMember(Name = "logoURL")]
    public string LogoURL { get; set; }
  }
}
