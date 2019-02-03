using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Alexandria.Shared.Enums;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.Tournament
{
  [DataContract]
  [JsonSchema("TournamentDetail")]
  public class Detail : DTO.Competition.Tournament
  {
    [DataMember(Name = "children")]
    public List<Detail> Children { get; set; } = new List<Detail>();
  }
}
