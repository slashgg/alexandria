using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Alexandria.EF.Models
{
  public class PlayerRanking : BaseEntity
  {
    [Column(TypeName = "decimal(18,2)")]
    public decimal MMR { get; set; }

    /* Foreign Keys */
    public Guid UserProfileId { get; set; }
    public Guid PlayerRankingGroupId { get; set; }
    public Guid GameId { get; set; }

    /* Relationships */
    [ForeignKey("UserProfileId")]
    public virtual UserProfile UserProfile { get; set; }
    [ForeignKey("PlayerRankingGroupId")]
    public virtual PlayerRankingGroup PlayerRankingGroup { get; set; }
    [ForeignKey("GameId")]
    public virtual Game Game { get; set; }
  }
}
