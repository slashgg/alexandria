using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using Alexandria.Shared.FieldError;
using Alexandria.Shared.Utils;

namespace Alexandria.Shared.Extensions
{
  public static class RequiredAttributeExtension
  {
    public static Svalbard.FieldError Validate(this RequiredAttribute attribute, PropertyInfo property, object propertyValue)
    {
      if (propertyValue is null)
      {
        return new Svalbard.FieldError(TypeUtils.GetName(property), "", Key.Required);
      }

      if (propertyValue is int)
      {
        return null;
      }

      if (propertyValue is Guid guid)
      {
        if (guid == Guid.Empty)
        {
          return new Svalbard.FieldError(TypeUtils.GetName(property), propertyValue.ToString(), Key.Required);
        }
      }
      else if (propertyValue is string value)
      {
        if (value.Trim().Length < 1)
        {
          return new Svalbard.FieldError(TypeUtils.GetName(property), propertyValue.ToString(), Key.Required);
        }
      }
      return null;
    }
  }
}
