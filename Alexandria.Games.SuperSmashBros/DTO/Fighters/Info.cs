using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using NJsonSchema.Annotations;

namespace Alexandria.Games.SuperSmashBros.DTO.Fighters
{
  [DataContract]
  [JsonSchema("SuperSmashBrosFightersInfo")]
  public class Info
  {
    [DataMember(Name = "id")]
    public Guid Id { get; set; }
    [DataMember(Name = "name")]
    public string Name { get; set; }
  }
}
