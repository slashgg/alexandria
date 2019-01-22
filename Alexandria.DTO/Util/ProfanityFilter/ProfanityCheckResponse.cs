using System;
using System.Collections.Generic;
using System.Text;
using Alexandria.Shared.Enums;

namespace Alexandria.DTO.Util.ProfanityFilter
{
  public struct ProfanityCheckResponse
  {
    public string OffendingWord { get; set; }
    public ProfanityFilterSeverity Severity { get; set; }

    public ProfanityCheckResponse(ProfanityFilterSeverity severity, string offendingWord = null)
    {
      this.Severity = severity;
      this.OffendingWord = offendingWord;
    }
  }
}
