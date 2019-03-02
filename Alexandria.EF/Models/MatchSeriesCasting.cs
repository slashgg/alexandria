using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Alexandria.Shared.Enums;
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
    public virtual ICollection<MatchSeriesCastingParticipation> MatchSeriesCastingParticipants { get; set; } = new List<MatchSeriesCastingParticipation>();

    public MatchSeriesCasting()
    {
    }

    public MatchSeriesCasting(string streamURL, DateTimeOffset? startsAt)
    {
      this.StreamingURL = streamURL;
      this.StartsAt = startsAt;
    }

    public void AddParticipant(Guid userId, CastingRole role)
    {
      if (this.MatchSeriesCastingParticipants.Any(mscp => mscp.UserProfileId == userId))
      {
        return;
      }

      this.MatchSeriesCastingParticipants.Add(new MatchSeriesCastingParticipation(userId, role));
    }
  }
}
