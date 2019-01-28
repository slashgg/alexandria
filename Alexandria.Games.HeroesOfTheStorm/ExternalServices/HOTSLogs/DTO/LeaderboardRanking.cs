using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Alexandria.ExternalServices.HOTSLogs.DTO
{
  [DataContract]
  public class LeaderboardRanking
  {
    [DataMember(Name = "GameMode")]
    public string GameMode { get; set; }
    [DataMember(Name = "LeagueID")]
    public int? LeagueId { get; set; }
    [DataMember(Name = "LeagueRank")]
    public int? LeagueRank { get; set; }
    [DataMember(Name = "CurrentMMR")]
    public int? CurrentMMR { get; set; }
  }
}
