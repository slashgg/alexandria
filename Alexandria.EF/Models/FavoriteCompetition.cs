using System;
using System.ComponentModel.DataAnnotations.Schema;
using Alexandria.Shared.Utils;

namespace Alexandria.EF.Models
{
  [ProtectedResource("favorite-competition")]
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

    public FavoriteCompetition() { }
    public FavoriteCompetition(Guid userId, Guid competitionId)
    {
      this.UserProfileId = userId;
      this.CompetitionId = competitionId;
    }
  }
}
