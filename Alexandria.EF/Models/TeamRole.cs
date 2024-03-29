﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Alexandria.EF.Models
{
  public class TeamRole : BaseEntity
  {
    [Required]
    public string Name { get; set; }
    public IList<string> Permissions { get; set; } = new List<string>();
    public bool RemoveProtection { get; set; } = false;

    /* Foreign Key */
    public Guid CompetitionId { get; set; }

    /* Relations */
    public virtual Competition Competition { get; set; }
    public virtual ICollection<TeamMembership> TeamMemberships { get; set; } = new List<TeamMembership>();
  }
}
