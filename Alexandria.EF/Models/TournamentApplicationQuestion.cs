using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Alexandria.Shared.Enums;

namespace Alexandria.EF.Models
{
  public class TournamentApplicationQuestion : BaseEntity
  {
    public string QuestionKey { get; set; }
    public FieldType FieldType { get; set; }
    public IList<string> SelectOptions { get; set; } = new List<string>();
    public bool Optional { get; set; }
    public string DefaultValue { get; set; }

    /* Foreign Key */

    public Guid TournamentId { get; set; }

    /* Relations */
    [ForeignKey("TournamentId")]
    public virtual Tournament Tournament { get; set; }
    public virtual ICollection<TournamentApplicationQuestionAnswer> TournamentApplicationQuestionAnswers { get; set; } = new List<TournamentApplicationQuestionAnswer>();

  }
}
