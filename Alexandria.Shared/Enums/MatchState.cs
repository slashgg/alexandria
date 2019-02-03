using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Alexandria.Shared.Enums
{
  public enum MatchState
  {
    [EnumMember(Value = "pending")]
    Pending = 1,
    [EnumMember(Value = "in-progress")]
    InProgress = 2,
    [EnumMember(Value = "complete")]
    Complete = 3,
    [EnumMember(Value = "cancelled")]
    Cancelled = 4,
  }
}
