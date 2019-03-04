using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Alexandria.Interfaces;
using Alexandria.Shared.Utils;

namespace Alexandria.EF.Models
{
  public class BaseEntity : IBaseEntity
  {
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string GetPermissionIdentifier()
    {
      var resourceAttribute = (ProtectedResourceAttribute)Attribute.GetCustomAttribute(this.GetType(), typeof(ProtectedResourceAttribute));
      return resourceAttribute?.Name;
    }
  }
}
