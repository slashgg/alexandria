﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Alexandria.EF.Models
{
  public class Permission : BaseEntity
  {
    [Required]
    public string ARN { get; set; }

    /* Foreign Key */
    public Guid UserProfileId { get; set; }

    /* Relations */
    [ForeignKey("UserProfileId")]
    public virtual UserProfile UserProfile { get; set; }

    public Permission()
    {

    }

    public Permission(Guid userProfileId, string ARN)
    {
      this.ARN = ARN;
      this.UserProfileId = userProfileId;
    }

    public Guid GetResourceId()
    {
      var parts = this.ARN.Split("::", StringSplitOptions.RemoveEmptyEntries);
      var resourceId = Guid.Empty;

      foreach (var part in parts)
      {
        if (Guid.TryParse(part, out resourceId))
        {
          break;
        }
      }

      return resourceId;
    }
  }
}
