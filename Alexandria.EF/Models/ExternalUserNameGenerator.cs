using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Alexandria.EF.Models
{
  public class ExternalUserNameGenerator : BaseEntity
  {
    public string Name { get; set; }
    [Required]
    public string Type { get; set; }
    public string LogoURL { get; set; }

    /* Relationships */
    public virtual ICollection<GameExternalUserNameGenerator> GameExternalUserNameGenerators { get; set; }
  }
}
