using System;
using System.ComponentModel.DataAnnotations.Schema;
using Alexandria.Shared.Enums;

namespace Alexandria.EF.Models
{
  public class TournamentApplicationHistory : BaseEntity
  {
    public TournamentApplicationState State { get; set; }
    public string Notes { get; set; }
    
    /* Foreign Key */
    public Guid TeamId { get; set; }

    /* Relations */
    [ForeignKey("TeamId")]
    public virtual Team Team { get; set; }
  }
}
