using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alexandria.EF.Models
{
  public class FavoriteCompetition : BaseEntity
  {
    /* Foreign Key */
    public Guid CompetitionId { get; set; }
    public Guid UserProfileId { get; set; }

    /* Relationships */
    [ForeignKey("CompetitionId")]
    public virtual Competition Competition { get; set; }
    [ForeignKey("UserProfileId")]
    public virtual UserProfile UserProfile { get; set; }
  }
}
