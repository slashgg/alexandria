using System;
using System.ComponentModel.DataAnnotations.Schema;
using Alexandria.Shared.Enums;

namespace Alexandria.EF.Models
{
  public class MatchParticipantResult : BaseEntity
  {
    public MatchOutcome MatchOutcome { get; set; } = MatchOutcome.Unknown;
    /* Foreign Key */
    public Guid MatchParticipantId { get; set; }
    public Guid MatchId { get; set; }

    /* Relationships */
    [ForeignKey("MatchParticipantId")]
    public virtual MatchParticipant MatchParticipant { get; set; }
    [ForeignKey("MatchId")]
    public virtual Match Match { get; set; }
  }
}
