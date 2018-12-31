using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Alexandria.EF.Models
{
  public class TournamentApplicationQuestionAnswer : BaseEntity
  {
    public string Answer { get; set; }

    /* Foreign Keys */

    public Guid TournamentApplicationId { get; set; }
    public Guid TournamentApplicationQuestionId { get; set; }

    /* Relationships */
    [ForeignKey("TournamentApplicationId")]
    public virtual TournamentApplication TournamentApplication { get; set; }
    [ForeignKey("TournamentApplicationQuestionId")]
    public virtual TournamentApplicationQuestion TournamentApplicationQuestion { get; set; }

    public TournamentApplicationQuestionAnswer() { }

    public TournamentApplicationQuestionAnswer(Guid questionId, string answer)
    {
      this.Answer = answer;
      this.TournamentApplicationQuestionId = questionId;
    }
  }
}
