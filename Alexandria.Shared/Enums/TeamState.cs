using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Alexandria.Shared.Enums
{
  public enum TeamState
  {
    [EnumMember(Value = "active")]
    Active = 1,
    [EnumMember(Value = "disbanded")]
    Disbanded = 2
  }
}
