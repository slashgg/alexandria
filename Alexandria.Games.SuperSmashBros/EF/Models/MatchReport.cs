using System;
using System.Collections.Generic;
using System.Text;
using Alexandria.EF.Models;

namespace Alexandria.Games.SuperSmashBros.EF.Models
{
  public class MatchReport : BaseEntity
  {
    public Guid MatchSeriesId { get; set; }

    public virtual ICollection<FighterPick> FighterPicks { get; set; } = new List<FighterPick>();
    public MatchReport() { }
    public MatchReport(Guid matchSeriesId)
    {
      this.MatchSeriesId = matchSeriesId;
    }
  }
}
