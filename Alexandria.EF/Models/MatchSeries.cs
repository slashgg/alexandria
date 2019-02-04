using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using Alexandria.Shared.Enums;

namespace Alexandria.EF.Models
{
  public class MatchSeries : BaseEntity
  {
    public DateTimeOffset? ScheduledAt { get; set; }
    public MatchSeriesType Type { get; set; }
    public MatchState State { get; set; }

    /* Foreign key */
    public Guid GameId { get; set; }
    public Guid? TournamentRoundId { get; set; }

    /* Relationships */
    [ForeignKey("GameId")]
    public virtual Game Game { get; set; }
    [ForeignKey("TournamentRoundId")]
    public virtual TournamentRound TournamentRound { get; set; }
    public virtual ICollection<MatchParticipant> MatchParticipants { get; set; } = new List<MatchParticipant>();
    public virtual ICollection<Match> Matches { get; set; } = new List<Match>();
    public virtual ICollection<MatchSeriesScheduleRequest> ScheduleRequests { get; set; }

    [NotMapped]
    public MatchParticipant Winner
    {
      get
      {
        if (!this.Matches.Any())
        {
          return null;
        }
        var wins = this.Matches.SelectMany(m => m.Winners);
        var participants = wins.GroupBy(w => w.MatchParticipantId).OrderByDescending(mpig => mpig.Count());
        var winnerGroup = participants.FirstOrDefault();
        var winnerId = winnerGroup.Key;
        var winner = this.MatchParticipants.FirstOrDefault(m => m.Id == winnerId);
        return winner;
      }
    }

    [NotMapped]
    public MatchParticipant Loser
    {
      get
      {
        if (!this.Matches.Any())
        {
          return null;
        }
        var losses = this.Matches.SelectMany(m => m.Losers);
        var participants = losses.GroupBy(w => w.MatchParticipantId).OrderByDescending(mpig => mpig.Count());
        var loserGroups = participants.FirstOrDefault();
        var loserId = loserGroups.Key;
        var loser = this.MatchParticipants.FirstOrDefault(m => m.Id == loserId);
        return loser;
      }
    }
  }
}
