using System;
using System.ComponentModel.DataAnnotations.Schema;
using Alexandria.EF.Models;

namespace Alexandria.Games.SuperSmashBros.EF.Models
{
  public class FighterPick : BaseEntity
  {
    public Guid UserProfileId { get; set; }
    /* Foreign Keys */
    public Guid FighterId { get; set; }
    public Guid MatchReportId { get; set; }

    /* Relationships */
    [ForeignKey("FighterId")]
    public virtual Fighter Fighter { get; set; }
    [ForeignKey("MatchReportId")]
    public virtual MatchReport MatchReport { get; set; }
  }
}
