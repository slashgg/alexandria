using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Alexandria.EF.Models
{
  public class TeamRole : BaseEntity
  {
    [Required]
    public string Name { get; set; }

    /* Foreign Key */
    public Guid CompetitionId { get; set; }

    /* Relations */
    public virtual Competition Competition { get; set; }
    public virtual ICollection<TeamMembership> TeamMemberships { get; set; } = new List<TeamMembership>();
    
  }
}
