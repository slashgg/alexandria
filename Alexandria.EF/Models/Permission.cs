using System;
using System.Collections.Generic;
using System.Text;

namespace Alexandria.EF.Models
{
  public class Permission : BaseEntity
  {
    public string Name { get; set; }
    public Shared.Enums.Permission Lookup { get; set; }
  }
}
