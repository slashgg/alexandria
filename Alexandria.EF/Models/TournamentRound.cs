using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Alexandria.EF.Models
{
  public class TournamentRound : BaseEntity
  {
    public string Name { get; set; }
    public string Slug { get; set; }
    public int SeriesPerRound { get; set; }
    public int SeriesGameCount { get; set; }
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }

    /* Foreign Key */
    public Guid TournamentId { get; set; }

    /* Relationships */
    [ForeignKey("TournamentId")]
    public virtual Tournament Tournament { get; set; }
    public virtual ICollection<MatchSeries> MatchSeries { get; set; } = new List<MatchSeries>();
  }
}
