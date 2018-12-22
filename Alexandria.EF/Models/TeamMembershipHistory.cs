using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Alexandria.EF.Models
{
  public class TeamMembershipHistory : BaseEntity
  {
    public string NOtes { get; set; }

    /* Foreign Keys */
    public Guid UserProfileId { get; set; }
    public Guid TeamId { get; set; }

    /* Relations */
    [ForeignKey("TeamId")]
    public virtual Team Team { get; set; }
    [ForeignKey("UserProfileId")]
    public virtual UserProfile UserProfile { get; set; }

  }
}
