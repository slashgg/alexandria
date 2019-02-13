using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Alexandria.EF.Models;

namespace Alexandria.Games.HeroesOfTheStorm.EF.Models
{
  public class MatchReport : BaseEntity
  {
    public Guid MatchId { get; set; }
    public string ReplayURL { get; set; }
    public bool ReplayParsed { get; set; } = false;
    public DateTimeOffset? ReplayedParsedAt { get; set; }


    /* Foreign Keys */
    public Guid MapId { get; set; }

    /* Relationships */
    [ForeignKey("MapId")]
    public virtual Map Map { get; set; }

    public MatchReport() { }

    public MatchReport(Guid matchId)
    {
      this.MatchId = matchId;
    }

    public MatchReport(Guid matchId, Guid mapId)
    {
      this.MatchId = matchId;
      this.MapId = mapId;
    }

    public MatchReport(Guid matchId, Guid mapId, string replayURL)
    {
      this.MatchId = matchId;
      this.MapId = mapId;
      this.ReplayURL = replayURL;
    }
  }
}
