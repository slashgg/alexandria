using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Alexandria.Shared.Enums
{
  public enum MatchOutcomeState
  {
    [EnumMember(Value = "not-yet-played")]
    NotYetPlayed = 1,
    [EnumMember(Value = "determined")]
    Determined = 2,
    [EnumMember(Value = "draw")]
    Draw = 3,
    [EnumMember(Value = "forfeit")]
    Forfeit = 4,
    [EnumMember(Value = "double-forfeit")]
    DoubleForfeit = 5,
  }
}
