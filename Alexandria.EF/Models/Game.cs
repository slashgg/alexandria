using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Alexandria.EF.Models
{
  public class Game : BaseEntity
  {
    [MaxLength(500)]
    public string Name { get; set; }

    /* Relations */
    public virtual ICollection<Competition> Competitions { get; set; } = new List<Competition>();
  }
}
