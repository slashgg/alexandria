using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Alexandria.Shared.Enums;

namespace Alexandria.EF.Models
{
  public class Tournament : BaseEntity
  {
    [MaxLength(100)]
    public string Name { get; set; }
    public string Slug { get; set; }
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public bool ApplicationRequired { get; set; } = true;
    public bool CanSignup { get; set; } = false;
    public DateTimeOffset? SignupOpenDate { get; set; }
    public DateTimeOffset? SignupCloseDate { get; set; }
    public string TokenImageURL { get; set; }
    public TournamentState State { get; set; } = TournamentState.Pending;

    /* Foreign Keys */
    public Guid CompetitionId { get; set; }

    /* Relations */
    [ForeignKey("CompetitionId")]
    public virtual Competition Competition { get; set; }
    public virtual ICollection<TournamentHistory> TournamentHistories { get; set; } = new List<TournamentHistory>();
    public virtual ICollection<TournamentApplication> TournamentApplications { get; set; } = new List<TournamentApplication>();
    public virtual ICollection<TournamentApplicationQuestion> TournamentApplicationQuestions { get; set; } = new List<TournamentApplicationQuestion>();
  }
}
