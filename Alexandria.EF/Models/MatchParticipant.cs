using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Alexandria.EF.Models
{
  public class MatchParticipant : BaseEntity
  {
    /* Foreign Keys */
    public Guid MatchSeriesId { get; set; }
    public Guid TeamId { get; set; }

    /* Relationships */
    [ForeignKey("MatchSeriesId")]
    public virtual MatchSeries MatchSeries { get; set; }
    [ForeignKey("TeamId")]
    public virtual Team Team { get; set; }
    public virtual ICollection<MatchParticipantResult> Results { get; set; } = new List<MatchParticipantResult>();

    [NotMapped]
    public IEnumerable<MatchParticipantResult> Wins
      { get { return this.Results.Where(r => r.MatchOutcome == Shared.Enums.MatchOutcome.Win); } }
    [NotMapped]
    public IEnumerable<MatchParticipantResult> Losses
      { get { return this.Results.Where(r => r.MatchOutcome == Shared.Enums.MatchOutcome.Forfeit || r.MatchOutcome == Shared.Enums.MatchOutcome.Loss || r.MatchOutcome == Shared.Enums.MatchOutcome.Disqualified); } }
    [NotMapped]
    public IEnumerable<MatchParticipantResult> Draws
      { get { return this.Results.Where(r => r.MatchOutcome == Shared.Enums.MatchOutcome.Draw); } }
  }
}
