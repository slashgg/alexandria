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
    public bool CastingClaimRequired { get; set; } = false;

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
    public virtual ICollection<MatchSeriesScheduleRequest> ScheduleRequests { get; set; } = new List<MatchSeriesScheduleRequest>();
    public virtual ICollection<MatchSeriesCasting> MatchSeriesCastings { get; set; } = new List<MatchSeriesCasting>();
    public virtual ICollection<MatchSeriesCastingClaim> MatchSeriesCastingClaims { get; set; } = new List<MatchSeriesCastingClaim>();

    [NotMapped]
    public MatchOutcomeState Outcome
    {
      get
      {
        var participantResults = this.Matches.SelectMany(m => m.Results).GroupBy(m => m.MatchParticipantId);

        var standardWinCount = participantResults.FirstOrDefault()?.Count(r => r.MatchOutcome == MatchOutcome.Win);
        if (standardWinCount == null)
        {
          return MatchOutcomeState.NotYetPlayed;
        }

        if (participantResults.All(prg => prg.Count(r => r.MatchOutcome == MatchOutcome.Win) == standardWinCount.Value))
        {
          return MatchOutcomeState.Draw;
        }

        return MatchOutcomeState.Determined;
      }
    }

    [NotMapped]
    public MatchParticipant Winner
    {
      get
      {
        if (!this.Matches.Any())
        {
          return null;
        }

        if (this.Outcome == MatchOutcomeState.Draw)
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

        if (this.Outcome == MatchOutcomeState.Draw)
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

    public bool IsParticipant(Guid teamId)
    {
      return this.MatchParticipants.Any(mp => mp.TeamId.Equals(teamId));
    }

    public MatchParticipant GetParticipant(Guid teamId)
    {
      return this.MatchParticipants.FirstOrDefault(mp => mp.TeamId.Equals(teamId));
    }

    public void StakeCastingClaim(Guid userId)
    {
      this.MatchSeriesCastingClaims.Add(new MatchSeriesCastingClaim(userId));
    }

    public void StakeCastingClaim(MatchSeriesCastingClaim claim)
    {
      this.MatchSeriesCastingClaims.Add(claim);
    }
  }
}
