using System;
using System.ComponentModel.DataAnnotations.Schema;
using Alexandria.Shared.Enums;

namespace Alexandria.EF.Models
{
  public class MatchSeriesScheduleRequest : BaseEntity
  {
    public ScheduleRequestState State { get; set; }
    public DateTimeOffset ProposedTimeSlot { get; set; }
    /* Foreign Key */
    public Guid OriginTeamId { get; set; }
    public Guid TargetTeamId { get; set; }
    public Guid? MatchSeriesId { get; set; }

    /* Relationships */
    [ForeignKey("OriginTeamId")]
    public virtual Team OriginTeam { get; set; }
    [ForeignKey("TargetTeamId")]
    public virtual Team TargetTeam { get; set; }
    [ForeignKey("MatchSeriesId")]
    public virtual MatchSeries MatchSeries { get; set; }
  }
}
