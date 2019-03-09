using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Alexandria.EF.Context;
using Alexandria.Interfaces.Services;
using Alexandria.Orchestration.Extensions;
using Alexandria.Shared.Extensions;
using Alexandria.Shared.FieldError;
using Alexandria.Shared.Validation;
using Svalbard;
using Svalbard.Services;

namespace Alexandria.Orchestration.Utils
{
  public class InputValidationService : IInputValidationService
  {
    private readonly AlexandriaContext alexandriaContext;

    public InputValidationService(AlexandriaContext alexandriaContext)
    {
      this.alexandriaContext = alexandriaContext;
    }

    public async Task<IList<FieldError>> Validate(object data, ServiceResult serviceResult)
    {
      var result = await this.Validate(data);
      foreach (var fieldError in result)
      {
        serviceResult.AddFieldError(fieldError);
      }
      return result;
    }

    public async Task<IList<FieldError>> Validate(object data)
    {
      var properties = data.GetType().GetProperties();
      var errorList = new List<FieldError>();
      foreach (var property in properties)
      {
        var errors = await Validate(property, property.GetValue(data));
        errorList.AddRange(errors);
      }

      return errorList;
    }

    public async Task<IList<FieldError>> Validate(PropertyInfo property, object propertyValue)
    {
      var attributes = Attribute.GetCustomAttributes(property);
      var errors = new List<FieldError>();
      foreach (var attribute in attributes)
      {
        switch (attribute)
        {
          case RequiredAttribute requiredAttribute:
          {
            var error = requiredAttribute.Validate(property, propertyValue);
            if (error != null)
            {
              errors.Add(error);
            }

            break;
          }
          case MinValueAttribute minValueAttribute:
          {
            var error = minValueAttribute.Validate(property, propertyValue);
            if (error != null)
            {
              errors.Add(error);
            }
            break;
          }
          case EFUniqueAttribute uniqueAttribute:
          {
            var error = await uniqueAttribute.Validate(property, propertyValue, this.alexandriaContext);
            if (error != null)
            {
              errors.Add(error);
            }

            break;
          }
        }
      }

      return errors;
    }
  }
}
