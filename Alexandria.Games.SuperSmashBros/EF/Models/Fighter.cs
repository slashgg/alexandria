using System;
using System.Collections.Generic;
using System.Text;
using Alexandria.EF.Models;

namespace Alexandria.Games.SuperSmashBros.EF.Models
{
  public class Fighter : BaseEntity
  {
    public string Name { get; set; }

    public virtual ICollection<FighterPick> FighterPicks { get; set; }
  }
}
