using System;
using System.Collections.Generic;
using System.Text;
using Alexandria.EF.Models;

namespace Alexandria.Games.HeroesOfTheStorm.EF.Models
{
  public class TournamentSettings : BaseEntity
  {
    public Guid TournamentId { get; set; }
    public bool ReplayUploadRequired { get; set; }
    public int MapBanCount { get; set; }

    /* Relationships */
    public virtual ICollection<TournamentMap> TournamentMaps { get; set; }
  }
}
