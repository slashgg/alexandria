using System;
using System.Collections.Generic;
using System.Text;

namespace Alexandria.EF.Models
{
  public class PlayerRankingGroup : BaseEntity
  {
    public string Name { get; set; }

    /* Foreign Keys */
    public Guid GameId { get; set; }


    /* Relationships */
    public virtual ICollection<PlayerRanking> PlayerRankings { get; set; } = new List<PlayerRanking>();
    public virtual ICollection<CompetitionRankingGroupMembership> CompetitionRankingGroupMemberships { get; set; } = new List<CompetitionRankingGroupMembership>();
    public virtual Game Game { get; set; }
  }
}
