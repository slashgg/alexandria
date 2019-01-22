using System;
using System.Collections.Generic;
using System.Text;
using Alexandria.Shared.Enums;

namespace Alexandria.EF.Models
{
  public class ProfanityFilter : BaseEntity
  {
    public string Word { get; set; }
    public ProfanityFilterSeverity Severity { get; set; }
  }
}
