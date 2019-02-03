using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Alexandria.DTO.Tournament
{
  [DataContract]
  public class TournamentRecord
  {
    [DataMember(Name = "wins")]
    public int Wins { get; set; }
    [DataMember(Name = "losses")]
    public int Losses { get; set; }
    [DataMember(Name = "draws")]
    public int Draws { get; set; }

    public TournamentRecord() { }
    public TournamentRecord(int wins, int losses, int draws)
    {
      this.Wins = wins;
      this.Losses = losses;
      this.Draws = draws;
    }
  }
}
