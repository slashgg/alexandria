using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Alexandria.Shared.Enums;

namespace Alexandria.EF.Models
{
  public class Match : BaseEntity
  {
    public int MatchOrder { get; set; }
    public MatchState State { get; set; } = MatchState.Pending;
    public MatchOutcomeState OutcomeState { get; set; } = MatchOutcomeState.NotYetPlayed;
    [NotMapped]
    public virtual IEnumerable<MatchParticipantResult> Winners { get { return this.Results.Where(r => r.MatchOutcome == MatchOutcome.Win);  } }
    [NotMapped]
    public virtual IEnumerable<MatchParticipantResult> Losers { get { return this.Results.Where(r => r.MatchOutcome == MatchOutcome.Loss || r.MatchOutcome == MatchOutcome.Forfeit || r.MatchOutcome == MatchOutcome.Disqualified);  } }

    /* Foreign Keys */
    public Guid MatchSeriesId { get; set; }

    /* Relationships */
    [ForeignKey("MatchSeriesId")]
    public virtual MatchSeries MatchSeries { get; set; }
    public virtual ICollection<MatchParticipantResult> Results { get; set; }
  }
}
