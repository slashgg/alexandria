using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Alexandria.Shared.Enums;

namespace Alexandria.EF.Models
{
  public class TournamentApplication : BaseEntity
  {
    public TournamentApplicationState State { get; set; } = TournamentApplicationState.Pending;
    
    /* Foreign Key */
    public Guid TeamId { get; set; }
    public Guid TournamentId { get; set; }

    /* Relations */
    [ForeignKey("TeamId")]
    public virtual Team Team { get; set; }
    [ForeignKey("TournamentId")]
    public virtual Tournament Tournament { get; set; }
    public virtual ICollection<TournamentApplicationHistory> TournamentApplicationHistories { get; set; } = new List<TournamentApplicationHistory>();
    public virtual ICollection<TournamentApplicationQuestionAnswer> TournamentApplicationQuestionAnswers { get; set; } = new List<TournamentApplicationQuestionAnswer>();

    public TournamentApplication(Guid teamId, Guid tournamentId)
    {
      this.TeamId = teamId;
      this.TournamentId = tournamentId;
    }

    public void Initialize()
    {
      this.TournamentApplicationHistories.Add(new TournamentApplicationHistory(TournamentApplicationState.Pending, "Created"));
    }

    public void Mark(TournamentApplicationState state, string notes = "")
    {
      this.State = state;
      this.TournamentApplicationHistories.Add(new TournamentApplicationHistory(state, notes));
    }

    public void AddAnswer(TournamentApplicationQuestionAnswer answer)
    {
      var existingAnswer = this.TournamentApplicationQuestionAnswers.FirstOrDefault(a => a.TournamentApplicationQuestionId == answer.Id);
      if (existingAnswer != null)
      {
        existingAnswer.Answer = answer.Answer;
        return;
      }

      this.TournamentApplicationQuestionAnswers.Add(answer);
    }
  }
}
