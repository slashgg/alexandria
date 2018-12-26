using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Alexandria.EF.Models
{
  public class TeamMembership : BaseEntity
  {
    /* Foreign Keys */
    [Required]
    public Guid UserProfileId { get; set; }
    [Required]
    public Guid TeamId { get; set; }
    [Required]
    public Guid TeamRoleId { get; set; }

    /* Relations */
    [ForeignKey("TeamId")]
    public virtual Team Team { get; set; }
    [ForeignKey("UserProfileId")]
    public virtual UserProfile UserProfile { get; set; }
    [ForeignKey("TeamRoleId")]
    public virtual TeamRole TeamRole { get; set; }

    public TeamMembership() { }

    public TeamMembership(Guid teamId, Guid userId, Guid roleId)
    {
      this.TeamId = teamId;
      this.UserProfileId = userId;
      this.TeamRoleId = roleId;

    }
  }
}
