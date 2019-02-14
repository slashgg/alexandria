using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using NJsonSchema.Annotations;

namespace Alexandria.Games.HeroesOfTheStorm.DTO.MatchSeries
{
  [DataContract]
  [JsonSchema("HeroesOfTheStormMapBan")]
  public class MapBan
  {
    [DataMember(Name = "mapId")]
    public Guid MapId { get; set; }
    [DataMember(Name = "teamId")]
    public Guid TeamId { get; set; }
  }
}
