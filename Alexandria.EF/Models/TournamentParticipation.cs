using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Alexandria.Shared.Enums;

namespace Alexandria.EF.Models
{
  public class TournamentParticipation : BaseEntity
  {
    public TournamentParticipationState State { get; set; }

    /* Foreign Keys */
    public Guid TeamId { get; set; }
    public Guid TournamentId { get; set; }

    /* Relationships */
    [ForeignKey("TeamId")]
    public virtual Team Team { get; set; }
    [ForeignKey("TournamentId")]
    public virtual Tournament Tournament { get; set; }
  }
}
