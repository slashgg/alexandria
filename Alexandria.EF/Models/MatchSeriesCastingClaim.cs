using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Alexandria.Shared.Utils;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Alexandria.EF.Models
{
  [ProtectedResource("match-series-casting-claim")]
  public class MatchSeriesCastingClaim : BaseEntity
  {
    public Guid UserProfileId { get; set; }
    public Guid MatchSeriesId { get; set; }

    /* Relationships */
    [ForeignKey("UserProfileId")]
    public virtual UserProfile UserProfile { get; set; }
    [ForeignKey("MatchSeriesId")]
    public virtual MatchSeries MatchSeries { get; set; }
  }
}
