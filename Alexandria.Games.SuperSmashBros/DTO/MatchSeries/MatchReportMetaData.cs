using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using NJsonSchema.Annotations;

namespace Alexandria.Games.SuperSmashBros.DTO.MatchSeries
{
  [DataContract]
  [JsonSchema("SuperSmashBrosMatchSeriesMetaData")]
  public class SuperSmashBrosMatchReportMetaData : Alexandria.DTO.MatchSeries.MatchReportMetaData
  {
    [DataMember(Name = "game")]
    public new string Game { get; } = "super-smash-bros";
    [DataMember(Name = "gameSpecific")]
    public new SuperSmashBrosMatchSeriesGameSpecificMetaData GameSpecific { get; set; } = new SuperSmashBrosMatchSeriesGameSpecificMetaData();
  }

  [DataContract]
  [JsonSchema("SuperSmashBrosMatchSeriesGameSpecificMetaData")]
  public class SuperSmashBrosMatchSeriesGameSpecificMetaData : Alexandria.DTO.MatchSeries.TournamentMatchResultMetaData
  {
    [DataMember(Name = "fighters")]
    public IList<Fighters.Info> Fighters { get; set; } = new List<Fighters.Info>();
  }
}
