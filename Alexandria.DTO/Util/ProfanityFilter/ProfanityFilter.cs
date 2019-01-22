using System.Runtime.Serialization;
using Alexandria.Shared.Enums;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.Util.ProfanityFilter
{
  [DataContract]
  [JsonSchema("ProfanityFilter")]
  public class ProfanityFilter
  {
    [DataMember(Name = "word")]
    public string Word { get; set; }
    [DataMember(Name = "severity")]
    public ProfanityFilterSeverity Severity { get; set; }
  }
}
