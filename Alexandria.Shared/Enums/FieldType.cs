using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Alexandria.Shared.Enums
{
  public enum FieldType
  {
    [EnumMember(Value = "string")]
    String = 1,
    [EnumMember(Value = "select")]
    Select = 2,
    [EnumMember(Value = "boolean")]
    Boolean = 3,
    [EnumMember(Value = "integer")]
    Integer = 4,
    [EnumMember(Value = "decimal")]
    Decimal = 5,
  }
}
