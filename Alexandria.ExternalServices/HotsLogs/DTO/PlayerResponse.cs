using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Alexandria.ExternalServices.HotsLogs.DTO
{
  [DataContract]
  public class PlayerResponse
  {
    [DataMember(Name = "PlayerID")]
    public int? PlayerId { get; set; }
    [DataMember(Name = "Name")]
    public string Name { get; set; }
    [DataMember(Name = "LeaderboardRankings")]
    public List<LeaderboardRanking> Rankings { get; set; }
  }
}
