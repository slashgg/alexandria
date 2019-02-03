using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Alexandria.Shared.Enums
{
  public enum MatchSeriesType
  {
    [EnumMember(Value = "competitive")]
    Competitive = 1,
    [EnumMember(Value = "scrim")]
    Scrim = 2,
    [EnumMember(Value = "exhibition")]
    Exhibition = 3,
  }
}
