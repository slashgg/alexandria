using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using NJsonSchema.Annotations;

namespace Alexandria.Games.HeroesOfTheStorm.DTO.MatchSeries
{
  [DataContract]
  [JsonSchema("HeroesOfTheStormMatchReportMetaData")]
  public class HeroesOfTheStormMatchReportMetaData : Alexandria.DTO.MatchSeries.MatchReportMetaData
  {
    [DataMember(Name = "tournament")]
    public new HeroesOfTheStormTournamentMatchResultMetaData Tournament { get; set; }
    [DataMember(Name = "game")]
    public new string Game { get; } = "heroes-of-the-storm";

    public override string CreateSubmitURL()
    {
      return $"/heroes-of-the-storm/match-series/{this.Id}/reporting";
    }
  }

  [DataContract]
  [JsonSchema("HeroesOfTheStormMatchSeriesTournamentMatchResultMetaData")]
  public class HeroesOfTheStormTournamentMatchResultMetaData : Alexandria.DTO.MatchSeries.TournamentMatchResultMetaData
  {
    [DataMember(Name = "gameSpecific")]
    public Tournament.ResultMetaData GameSpecific { get; set; }

    public HeroesOfTheStormTournamentMatchResultMetaData()
    {

    }

    public HeroesOfTheStormTournamentMatchResultMetaData(Tournament.ResultMetaData tournamentMeta)
    {
      this.GameSpecific = tournamentMeta;
    }
  }
}
