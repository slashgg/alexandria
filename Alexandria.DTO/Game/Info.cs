using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.Game
{
  [DataContract]
  [JsonSchema("GameInfo")]
  public class Info
  {
    [DataMember(Name = "id")]
    public Guid Id { get; set; }
    [DataMember(Name = "name")]
    public string Name { get; set; }
  }
}
