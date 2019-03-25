using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Alexandria.DTO.MatchSeries;
using NJsonSchema.Annotations;

namespace Alexandria.Games.HeroesOfTheStorm.DTO.MatchSeries
{
  [JsonSchema("HeroesOfTheStormMatchResultReport")]
  [DataContract]
  public class HeroesOfTheStormMatchResultReport : MatchResultReport
  {
    [DataMember(Name = "replayURL")]
    public string ReplayURL { get; set; }
    [DataMember(Name = "mapId")]
    public Guid? MapId { get; set; }
  }
}
