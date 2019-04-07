using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using NJsonSchema.Annotations;

namespace Alexandria.Games.SuperSmashBros.DTO.MatchSeries
{
  [JsonSchema("SuperSmashBrosMatchSeriesFighterPick")]
  [DataContract]
  public class FighterPick
  {
    [DataMember(Name = "teamId")]
    public Guid TeamId { get; set; }
    [DataMember(Name = "fighterId")]
    public Guid FighterId { get; set; }
    [DataMember(Name = "matchId")]
    public Guid MatchId { get; set; }
  }
}
