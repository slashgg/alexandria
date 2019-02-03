using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
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
    public TournamentType Type { get; set; }

    /* Foreign Keys */
    public Guid CompetitionId { get; set; }
    public Guid? ParentTournamentId { get; set; }

    /* Relations */
    [ForeignKey("CompetitionId")]
    public virtual Competition Competition { get; set; }
    [ForeignKey("ParentTournamentId")]
    public virtual Tournament ParentTournament { get; set; }
    public virtual ICollection<TournamentHistory> TournamentHistories { get; set; } = new List<TournamentHistory>();
    public virtual ICollection<TournamentApplication> TournamentApplications { get; set; } = new List<TournamentApplication>();
    public virtual ICollection<TournamentApplicationQuestion> TournamentApplicationQuestions { get; set; } = new List<TournamentApplicationQuestion>();
    public virtual ICollection<TournamentParticipation> TournamentParticipations { get; set; } = new List<TournamentParticipation>();
    public virtual ICollection<TournamentParticipationHistory> TournamentParticipationHistories { get; set; } = new List<TournamentParticipationHistory>();
    public virtual ICollection<TournamentRound> TournamentRounds { get; set; } = new List<TournamentRound>();
    public virtual ICollection<Tournament> Tournaments { get; set; } = new List<Tournament>();
  }
}
