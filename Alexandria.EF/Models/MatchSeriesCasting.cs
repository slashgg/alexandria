using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Alexandria.Shared.Utils;

namespace Alexandria.EF.Models
{
  [ProtectedResource("match-series-casting")]
  public class MatchSeriesCasting : BaseEntity

  {
  public string StreamingURL { get; set; }
  public string VODURL { get; set; }
  public DateTimeOffset? StartsAt { get; set; }

  /* Foreign Keys */
  public Guid MatchSeriesId { get; set; }

  /* Relationships */
  [ForeignKey("MatchSeriesId")] public virtual MatchSeries MatchSeries { get; set; }
  public virtual ICollection<MatchSeriesCastingParticipation> MatchSeriesCastingParticipants { get; set; }
  }
}
