
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Alexandria.EF.Context;
using Alexandria.Shared.Extensions;
using Alexandria.Shared.Utils;
using Alexandria.Shared.Validation;
using Microsoft.EntityFrameworkCore;
using Svalbard;

namespace Alexandria.Orchestration.Extensions
{
  public class EFUniqueExists
  {
    public bool Exists { get; set; }
  }

  public static class EFUniqueAttributeExtension
  {

    public static async Task<Svalbard.FieldError> Validate(this EFUniqueAttribute attribute, PropertyInfo property,
      object propertyValue, AlexandriaContext alexandriaContext)
    {

      if (propertyValue is string stringValue)
      {
        if (string.IsNullOrEmpty(stringValue))
        {
          return null;
        }

        var result = await alexandriaContext.LoadStoredProc("ValidateUniqueStringValue")
          .WithSqlParam("@Value", stringValue)
          .WithSqlParam("@Table", attribute.Table)
          .WithSqlParam("@Column", attribute.Column)
          .ExecuteStoredProc<EFUniqueExists>();

        if (result.Any(r => r.Exists))
        {
          return new FieldError(TypeUtils.GetName(property), stringValue, Shared.FieldError.Key.Unique);
        }

        return null;
      }

      return null;
    }
  }
}
