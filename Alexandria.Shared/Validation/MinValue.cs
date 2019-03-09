using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Alexandria.Shared.FieldError;
using Alexandria.Shared.Utils;

namespace Alexandria.Shared.Validation
{
  public class MinValueAttribute : Attribute
  {
    public int MinValue { get; set; }

    public MinValueAttribute(int minValue)
    {
      this.MinValue = minValue;
    }

    public Svalbard.FieldError Validate(PropertyInfo property, object propertyValue)
    {
      if (propertyValue is null)
      {
        return new Svalbard.FieldError(TypeUtils.GetName(property), "", Key.MinNumber);
      }

      if (propertyValue is int intValue)
      {
        if (intValue < MinValue)
        {
          return new Svalbard.FieldError(TypeUtils.GetName(property), propertyValue.ToString(), Key.MinNumber);
        }
      }
      else if (propertyValue is decimal decimalValue)
      {
        if (decimalValue < MinValue)
        {
          return new Svalbard.FieldError(TypeUtils.GetName(property), propertyValue.ToString(), Key.MinNumber);
        }
      }
      else if (propertyValue is string stringValue)
      {
        if (int.TryParse(stringValue, out var parsedString))
        {
          if (parsedString < MinValue)
          {
            return new Svalbard.FieldError(TypeUtils.GetName(property), propertyValue.ToString(), Key.MinNumber);
          }
        }
      }

      return null;
    }
  }
}
