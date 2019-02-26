using System;
using System.Data;
using Alexandria.Shared.Extensions;

namespace Alexandria.Shared.Utils
{
  public static class AuthorizationHelper
  {
    public static string GetResourceName(Type type)
    {
      var resourceAttribute = (ProtectedResourceAttribute)Attribute.GetCustomAttribute(type, typeof(ProtectedResourceAttribute));
      return resourceAttribute?.Name;
    }

    public static string GenerateARN(Type type, string resourceId, string permission)
    {
      var resourceAttribute = (ProtectedResourceAttribute)Attribute.GetCustomAttribute(type, typeof(ProtectedResourceAttribute));
      if (resourceAttribute == null)
      {
        throw new NoNullAllowedException("This Class is not a protected resource");
      }

      return $"{resourceAttribute.Name}::{resourceId}::{permission}";
    }

    public static string GenerateARN(Type type, string resourceId, Enum permission)
    {
      var resourceAttribute = (ProtectedResourceAttribute)Attribute.GetCustomAttribute(type, typeof(ProtectedResourceAttribute));
      if (resourceAttribute == null)
      {
        throw new NoNullAllowedException("This Class is not a protected resource");
      }

      var permissionValue = permission.GetStringValue();

      return $"{resourceAttribute.Name}::{resourceId}::{permissionValue}";
    }

    public static string GenerateARN(object obj, string permission)
    {
      var resourceAttribute = (ProtectedResourceAttribute)Attribute.GetCustomAttribute(obj.GetType(), typeof(ProtectedResourceAttribute));
      if (resourceAttribute == null)
      {
        throw new NoNullAllowedException("This Class is not a protected resource");
      }

      var props = obj.GetType();

      return $"{resourceAttribute.Name}::{obj.GetType().GetProperty("Id")}::{permission}";
    }

    public static string GenerateARN(object obj, Enum permission)
    {
      var resourceAttribute = (ProtectedResourceAttribute)Attribute.GetCustomAttribute(obj.GetType(), typeof(ProtectedResourceAttribute));
      if (resourceAttribute == null)
      {
        throw new NoNullAllowedException("This Class is not a protected resource");
      }

      var props = obj.GetType();
      var permissionValue = permission.GetStringValue();

      return $"{resourceAttribute.Name}::{obj.GetType().GetProperty("Id")}::{permissionValue}";
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
