﻿using System;
using System.Collections.Generic;

namespace Alexandria.EF.Models
{
  public class UserProfile : BaseEntity
  {
    public string DisplayName { get; set; }
    public string Email { get; set; }
    public DateTime? Birthday { get; set; }
    public string AvatarURL { get; set; }

    /* Relations */
    public virtual ICollection<ExternalAccount> ExternalAccounts { get; set; } = new List<ExternalAccount>();
    public virtual ICollection<TeamMembership> TeamMemberships { get; set; } = new List<TeamMembership>();

    public UserProfile(Guid id, string displayName, string email)
    {
      this.Id = id;
      this.DisplayName = displayName;
      this.Email = email;
    }
  }
}
