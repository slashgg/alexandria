using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Alexandria.Shared.Enums;

namespace Alexandria.EF.Models
{
  public class TournamentParticipationHistory : BaseEntity
  {
    public TournamentParticipationState State { get; set; }
    public string Notes { get; set; }
    
    /* Foreign Keys */
    public Guid TournamentId { get; set; }
    public Guid TeamId { get; set; }

    /* Relationships */
    [ForeignKey("TeamId")]
    public virtual Team Team { get; set; }
    [ForeignKey("TournamentId")]
    public virtual Tournament Tournament { get; set; }
  }
}
