using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Alexandria.Shared.Enums
{
  public enum MatchOutcome
  {
    [EnumMember(Value = "unknown")]
    Unknown = 0,
    [EnumMember(Value = "win")]
    Win = 1,
    [EnumMember(Value = "loss")]
    Loss = 2,
    [EnumMember(Value = "forfeit")]
    Forfeit = 3,
    [EnumMember(Value = "disqualified")]
    Disqualified = 4,
    [EnumMember(Value = "draw")]
    Draw = 5,
  }
}
