using System;
using System.Collections.Generic;
using System.Text;
using Alexandria.EF.Models;

namespace Alexandria.Games.HeroesOfTheStorm.EF.Models
{
  public class TournamentMap : BaseEntity
  {
    /* Foreign Keys */
    public Guid TournamentSettingsId { get; set; }
    public Guid MapId { get; set; }

    /* Relationships */
    public virtual Map Map { get; set; }
    public virtual TournamentSettings TournamentSettings { get; set; }
  }
}
