using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Alexandria.Shared.Utils
{
  public static class AuthorizationHelper
  {
    public static string GenerateARN(Type type, string resourceId, string permission)
    {
      var resourceAttribute = (ProtectedResourceAttribute)Attribute.GetCustomAttribute(type, typeof(ProtectedResourceAttribute));
      if (resourceAttribute == null)
      {
        throw new NoNullAllowedException("This Class is not a protected resource");
      }

      return $"{resourceAttribute.Name}::{resourceId}::{permission}";
    }

    public static string MasterPermission(string permission)
    {
      var permissionParts = permission.Split("::");
      permissionParts[permissionParts.Length - 1] = "*";
      var masterPermission = string.Join("::", permissionParts);

      return masterPermission;
    }
  }
}
