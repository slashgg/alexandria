using System;
using System.ComponentModel.DataAnnotations.Schema;
using Alexandria.EF.Models;

namespace Alexandria.Games.SuperSmashBros.EF.Models
{
  public class FighterPick : BaseEntity
  {
    public Guid TeamId { get; set; }
    /* Foreign Keys */
    public Guid FighterId { get; set; }
    public Guid MatchId { get; set; }
    public Guid MatchReportId { get; set; }

    /* Relationships */
    [ForeignKey("FighterId")]
    public virtual Fighter Fighter { get; set; }
    [ForeignKey("MatchReportId")]
    public virtual MatchReport MatchReport { get; set; }

    public FighterPick()
    {
    }

    public FighterPick(Guid teamId, Guid fighterId, Guid matchId)
    {
      this.TeamId = teamId;
      this.FighterId = fighterId;
      this.MatchId = matchId;
    }
  }
}
