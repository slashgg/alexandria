using System;
using System.Collections.Generic;
using System.Text;

namespace Alexandria.EF.Models
{
  public class TeamRolePermissions : BaseEntity
  {
    /* Foreign Key */
    public Guid TeamRoleId { get; set; }
    public Guid PermissionId { get; set; }

    /* Relation */
    
  }
}
