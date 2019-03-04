using System;
using System.Collections.Generic;
using System.Text;

namespace Alexandria.Interfaces
{
  public interface IBaseEntity
  {
    Guid Id { get; set; }
    string GetPermissionIdentifier();
  }
}
