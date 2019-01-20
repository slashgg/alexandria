using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.Game
{
  [DataContract]
  [JsonSchema("GameDetail")]
  public class Detail
  {
    [DataMember(Name = "name")]
    public string Name { get; set; }
    [DataMember(Name = "id")]
    public Guid Id { get; set; }
  }
}
