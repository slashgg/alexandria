using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace Alexandria.Shared.Utils
{
  public static class TypeUtils
  {
    public static string GetName(PropertyInfo property)
    {
      try
      {
        var dataMember = (DataMemberAttribute)Attribute.GetCustomAttribute(property, typeof(DataMemberAttribute));
        return dataMember != null ? dataMember.Name : property.Name;
      }
      catch (Exception ex)
      {
        return property.Name;
      }
    }
  }
}
