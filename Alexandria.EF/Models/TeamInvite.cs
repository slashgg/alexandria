using System;
using Alexandria.Shared.Enums;

namespace Alexandria.EF.Models
{
  public class TeamInvite : BaseEntity
  {
    public string Email { get; set; }
    public InviteState State { get; set; }

    /* Foreign Key */
    public Guid TeamId { get; set; }
    public Guid? UserProfileId { get; set; }

    /* Relations */
    public virtual Team Team { get; set; }
    public virtual UserProfile UserProfile { get; set; }
  }
}
