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
    public Guid TournamentApplicationId { get; set; }

    /* Relations */
    [ForeignKey("TournamentApplicationId")]
    public virtual TournamentApplication TournamentApplication { get; set; }

    public TournamentApplicationHistory() { }
    public TournamentApplicationHistory(TournamentApplicationState state, string notes)
    {
      this.State = state;
      this.Notes = notes;
    }
  }
}
