using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alexandria.EF.Models
{
  public class Tournament : BaseEntity
  {
    [MaxLength(100)]
    public string Name { get; set; }
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public bool ApplicationRequired { get; set; } = true;
    public bool CanSignup { get; set; } = false;
    public DateTimeOffset? SignupOpenDate { get; set; }
    public DateTimeOffset? SignupCloseDate { get; set; }
    

    /* Foreign Keys */
    public Guid CompetitionId { get; set; }

    /* Relations */
    [ForeignKey("CompetitionId")]
    public virtual Competition Competition { get; set; }
    public virtual ICollection<TournamentHistory> TournamentHistories { get; set; } = new List<TournamentHistory>();
    public virtual ICollection<TournamentApplication> TournamentApplications { get; set; } = new List<TournamentApplication>();
  }
}
