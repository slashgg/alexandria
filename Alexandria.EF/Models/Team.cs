using System;
using System.Collections.Generic;
using System.Text;
using Alexandria.Shared.Enums;

namespace Alexandria.EF.Models
{
  public class Team : BaseEntity
  {
    public string Name { get; set; }
    public string Abbreviation { get; set; }
    public string LogoURL { get; set; }
    public TeamState TeamState { get; set; }

    /* Foreign Keys */
    public Guid CompetitionId { get; set; }

    /* Relations */
    public virtual Competition Competition { get; set; }
    public virtual ICollection<TeamMembership> TeamMemberships { get; set; } = new List<TeamMembership>();
  }
}
