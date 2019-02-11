using System;
using System.Collections.Generic;
using System.Text;
using Alexandria.EF.Models;

namespace Alexandria.Games.HeroesOfTheStorm.EF.Models
{
  public class Map : BaseEntity
  {
    public string Name { get; set; }

    /* Relationships */
    public virtual ICollection<TournamentMap> TournamentMaps { get; set; }
    public virtual ICollection<MapBan> MapBans { get; set; }

    public Map() { }
    public Map(string name)
    {
      this.Name = name;
    }
  }
}
