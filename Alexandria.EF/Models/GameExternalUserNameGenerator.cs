using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Alexandria.EF.Models
{
  public class GameExternalUserNameGenerator : BaseEntity
  {
    /* Foreign Key */
    public Guid GameId { get; set; }
    public Guid ExternalUserNameGeneratorId { get; set; }

    /* Relationships */
    [ForeignKey("GameId")]
    public virtual Game Game { get; set; }
    [ForeignKey("ExternalUserNameGeneratorId")]
    public virtual ExternalUserNameGenerator ExternalUserNameGenerator { get; set; }
  }
}
