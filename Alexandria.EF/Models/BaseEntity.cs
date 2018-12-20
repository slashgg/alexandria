using System;
using System.ComponentModel.DataAnnotations;

namespace Alexandria.EF.Models
{
  public class BaseEntity
  {
    [Key]
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  }
}
