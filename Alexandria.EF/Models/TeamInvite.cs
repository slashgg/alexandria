﻿using System;
using System.ComponentModel.DataAnnotations.Schema;
using Alexandria.Shared.Enums;
using Alexandria.Shared.Utils;

namespace Alexandria.EF.Models
{
  [ProtectedResource("team-invite")]
  public class TeamInvite : BaseEntity
  {
    public string Email { get; set; }
    public InviteState State { get; set; } = InviteState.Pending;

    /* Foreign Key */
    public Guid TeamId { get; set; }
    public Guid? UserProfileId { get; set; }

    /* Relations */
    [ForeignKey("TeamId")]
    public virtual Team Team { get; set; }
    [ForeignKey("UserProfileId")]
    public virtual UserProfile UserProfile { get; set; }

    public TeamInvite(Guid teamId)
    {
      this.TeamId = teamId;
    }

    public TeamInvite(Guid teamId, string email, Guid? userId)
    {
      this.TeamId = teamId;
      this.Email = email;
      this.UserProfileId = userId;
    }

    public bool CanBeRedeemed()
    {
      return State == InviteState.Pending;
    }

    public void Mark(InviteState state)
    {
      this.State = state;
    }
  }
}
