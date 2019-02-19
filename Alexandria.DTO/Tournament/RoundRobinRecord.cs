using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json.Schema;
using NJsonSchema.Annotations;

namespace Alexandria.DTO.Tournament
{
  [DataContract]
  [JsonSchema("TournamentRoundRobinRecord")]
  public class RoundRobinRecord
  {
    [DataMember(Name = "totalPoints")]
    public int TotalPoints { get; set; }
    [DataMember(Name = "matchSeriesPlayed")]
    public int MatchSeriesPlayed { get; set; }
    [DataMember(Name = "wins")]
    public int Wins { get; set; }
    [DataMember(Name = "losses")]
    public int Losses { get; set; }
    [DataMember(Name = "draws")]
    public int Draws { get; set; }
    [DataMember(Name = "matchLosses")]
    public int MatchLosses { get; set; }
    [DataMember(Name = "matchWins")]
    public int MatchWins { get; set; }
    [DataMember(Name = "matchDraws")]
    public int MatchDraws { get; set; }
    [DataMember(Name = "winPercentage")]
    public decimal WinPercentage { get; set; }
    [DataMember(Name = "team")]
    public Team.Info Team { get; set; }
  }
}
