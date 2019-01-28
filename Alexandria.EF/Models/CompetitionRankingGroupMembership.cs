using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Alexandria.EF.Models
{
  public class CompetitionRankingGroupMembership : BaseEntity
  {
    /* Foreign Keys */
    public Guid CompetitionId { get; set; }
    public Guid PlayerRankingGroupId { get; set; }

    /* Relationships */
    [ForeignKey("CompetitionId")]
    public virtual Competition Competition { get; set; }
    [ForeignKey("PlayerRankingGroupId")]
    public virtual PlayerRankingGroup PlayerRankingGroup { get; set; }
  }
}
