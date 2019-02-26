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
    public CastingRole Role { get; set; }

    /* Foreign Keys */
    public Guid UserProfileId { get; set; }
    public Guid MatchSeriesCastingId { get; set; }

    
    /* Relationships */

    [ForeignKey("UserProfileId")]
    public virtual UserProfile UserProfile { get; set; }
    [ForeignKey("MatchSeriesCastingId")]
    public virtual MatchSeriesCasting MatchSeriesCasting { get; set; }
  }
}
