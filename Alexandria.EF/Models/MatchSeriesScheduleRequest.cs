using System;
using System.ComponentModel.DataAnnotations.Schema;
using Alexandria.Shared.Enums;

namespace Alexandria.EF.Models
{
  public class MatchSeriesScheduleRequest : BaseEntity
  {
    public ScheduleRequestState State { get; set; } = ScheduleRequestState.Pending;
    public DateTimeOffset ProposedTimeSlot { get; set; }
    public MatchSeriesType MatchType { get; set; }
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

    public MatchSeriesScheduleRequest() { }
    public MatchSeriesScheduleRequest(Guid originTeamId, Guid targetTeamId, MatchSeriesType matchType, DateTimeOffset proposedTimeSlot, Guid? matchSeriesId)
    {
      this.OriginTeamId = originTeamId;
      this.TargetTeamId = targetTeamId;
      this.MatchType = matchType;
      this.ProposedTimeSlot = proposedTimeSlot;
      this.MatchSeriesId = matchSeriesId;
    }

    public void Accept()
    {
      this.State = ScheduleRequestState.Accepted;
    }

    public void Decline()
    {
      this.State = ScheduleRequestState.Declined;
    }

    public void Rescind()
    {
      this.State = ScheduleRequestState.Rescinded;
    }
  }
}
