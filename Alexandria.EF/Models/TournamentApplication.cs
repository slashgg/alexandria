using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Alexandria.Shared.Enums;

namespace Alexandria.EF.Models
{
  public class TournamentApplication : BaseEntity
  {
    public TournamentApplicationState State { get; set; }
    
    /* Foreign Key */
    public Guid TeamId { get; set; }
    public Guid TournamentId { get; set; }

    /* Relations */
    [ForeignKey("TeamId")]
    public virtual Team Team { get; set; }
    [ForeignKey("TournamentId")]
    public virtual Tournament Tournament { get; set; }
    public virtual ICollection<TournamentApplicationHistory> TournamentApplicationHistories { get; set; } = new List<TournamentApplicationHistory>();
  }
}
