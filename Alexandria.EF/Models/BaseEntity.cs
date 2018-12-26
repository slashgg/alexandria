using System;
using System.ComponentModel.DataAnnotations;

namespace Alexandria.EF.Models
{
  public class BaseEntity
  {
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  }
}
