using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using NJsonSchema.Annotations;

namespace Alexandria.Games.SuperSmashBros.DTO.MatchSeries
{
  [JsonSchema("SuperSmashBrosAdditionalMatchSeriesData")]
  [DataContract]
  public class AdditionalMatchSeriesData
  {
    [DataMember(Name = "matchSeriesId")]
    public Guid MatchSeriesId { get; set; }
    [DataMember(Name = "fighterPicks")]
    public List<FighterPick> FighterPicks { get; set; }
  }
}
