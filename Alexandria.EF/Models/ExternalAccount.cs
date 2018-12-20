using Alexandria.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Alexandria.EF.Models
{
  public class ExternalAccount : BaseEntity
  {
    public ExternalProvider Provider { get; set; }
    public string Name { get; set; }
    public bool Verified { get; set; } = false;
    public string Token { get; set; }
    public string ExternalId { get; set; }

    /* Foreign Keys */
    public Guid UserProfileId { get; set; }

    /* Relations */
    [ForeignKey("UserProfileId")]
    public virtual UserProfile UserProfile { get; set; }
  }
}
