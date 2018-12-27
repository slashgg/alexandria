using System;
using System.Runtime.Serialization;


namespace Alexandria.Shared.Extensions
{
  public static class EnumExtension
  {
    public static string GetStringValue(this Enum enumObject)
    {
      return ((EnumMemberAttribute)enumObject.GetType().GetMember(enumObject.ToString())[0].GetCustomAttributes(typeof(EnumMemberAttribute), false)[0]).Value;
    }
  }
}
