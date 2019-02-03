using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.Team
{
  [DataContract]
  [JsonSchema("TeamInfo")]
  public class Info
  {
    [DataMember(Name = "id")]
    public Guid Id { get; set; }
    [DataMember(Name = "name")]
    public string Name { get; set; }
    [DataMember(Name = "abbreviation")]
    public string Abbreviation { get; set; }
    [DataMember(Name = "logoURL")]
    public string LogoURL { get; set; }
  }
}
