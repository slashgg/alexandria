using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using NJsonSchema.Annotations;

namespace Alexandria.Games.HeroesOfTheStorm.DTO.MatchSeries
{
  [DataContract]
  [JsonSchema("HeroesOfTheStormMatchSeriesReportingRequest")]
  public class MatchSeriesResultReportingRequest
  {
    [DataMember(Name = "mapBans")]
    public List<MapBan> MapBans { get; set; }
    [DataMember(Name = "results")]
    public List<HeroesOfTheStormMatchResultReportingRequest> Results { get; set; }
  }
}
