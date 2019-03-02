using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Alexandria.Shared.Enums;
using Alexandria.Shared.Utils;

namespace Alexandria.EF.Models
{
  [ProtectedResource("match-series-casting-participation")]
  public class MatchSeriesCastingParticipation : BaseEntity
  {
    public CastingRole Role { get; set; } = CastingRole.PlayByPlay;

    /* Foreign Keys */
    public Guid UserProfileId { get; set; }
    public Guid MatchSeriesCastingId { get; set; }

    
    /* Relationships */

    [ForeignKey("UserProfileId")]
    public virtual UserProfile UserProfile { get; set; }
    [ForeignKey("MatchSeriesCastingId")]
    public virtual MatchSeriesCasting MatchSeriesCasting { get; set; }

    public MatchSeriesCastingParticipation() { }

    public MatchSeriesCastingParticipation(Guid userId)
    {
      this.UserProfileId = userId;
    }

    public MatchSeriesCastingParticipation(Guid userId, CastingRole role)
    {
      this.UserProfileId = userId;
      this.Role = role;
    }
  }
}
