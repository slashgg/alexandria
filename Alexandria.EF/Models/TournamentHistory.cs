using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Alexandria.Shared.Enums;

namespace Alexandria.EF.Models
{
  public class TournamentHistory : BaseEntity
  {
    public TournamentState State { get; set; }
    [MaxLength(500)]
    public string Notes { get; set; }

    /* Foreign Keys */
    public Guid TournamentId { get; set; }

    /* Relations */
    [ForeignKey("TournamentId")]
    public virtual Tournament Tournament { get; set; }
  }
}
