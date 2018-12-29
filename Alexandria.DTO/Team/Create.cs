using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.Team
{
  [DataContract]
  [JsonSchema("TeamCreate")]

  public class Create
  {
    [DataMember(Name = "name"), Required]
    public string Name { get; set; }
    [DataMember(Name = "invites")]
    public List<string> Invites { get; set; } = new List<string>();
    [DataMember(Name = "abbreviation")]
    public string Abbreviation { get; set; }
  }
}
