using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Alexandria.EF.Models
{
  public class Game : BaseEntity
  {
    [MaxLength(500)]
    public string Name { get; set; }
    public string Slug { get; set; }
    public string InternalIdentifier { get; set; }


    /* Relations */
    public virtual GameExternalUserNameGenerator GameExternalUserNameGenerator { get; set; }
    public virtual ICollection<Competition> Competitions { get; set; } = new List<Competition>();
    public virtual ICollection<PlayerRanking> PlayerRankings { get; set; } = new List<PlayerRanking>();
    public virtual ICollection<PlayerRankingGroup> PlayerRankingGroups { get; set; } = new List<PlayerRankingGroup>();
    public virtual ICollection<ExternalUserName> ExternalUserNames { get; set; } = new List<ExternalUserName>();

  }
}
