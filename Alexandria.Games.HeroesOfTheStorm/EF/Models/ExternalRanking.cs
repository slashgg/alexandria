using System;
using System.Collections.Generic;
using System.Text;
using Alexandria.EF.Models;
using Alexandria.Shared.Enums;

namespace Alexandria.Games.HeroesOfTheStorm.EF.Models
{
  public class ExternalRanking : BaseEntity
  {
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public int? Ranking { get; set; }
    public string GameMode { get; set; }
    public MMRSource MMRSource { get; set; }
    public Guid UserProfileId { get; set; }
    public BattleNetRegion BattleNetRegion { get; set; }
  }
}
