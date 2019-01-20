using System;
using System.Collections.Generic;
using System.Text;

namespace Alexandria.EF.Models
{
  public class CompetitionLevel : BaseEntity
  {
    public string Name { get; set; }
    public int Level { get; set; }

    /* Relationships */
    public virtual ICollection<Competition> Competitions { get; set; } = new List<Competition>();
  }
}
