using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Alexandria.EF.Models;

namespace Alexandria.Games.HeroesOfTheStorm.EF.Models
{
  public class MapBan : BaseEntity
  {
    public Guid MapId { get; set; }
    public Guid TeamId { get; set; }
    public Guid MatchSeriesId { get; set; }

    [ForeignKey("MapId")]
    public virtual Map Map { get; set; }

    public MapBan() { }

    public MapBan(Guid mapId, Guid teamId, Guid matchSeriesId)
    {
      this.MapId = mapId;
      this.TeamId = teamId;
      this.MatchSeriesId = matchSeriesId;
    }
  }
}
