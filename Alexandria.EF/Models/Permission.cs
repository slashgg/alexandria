using System;
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
  }
}
