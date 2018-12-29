using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NJsonSchema;

namespace Alexandria.Utils.Swagger
{
  public class AlexandriaSchemaNameGenerator : ISchemaNameGenerator
  {
    public string Generate(Type type)
    {
      var name = type.ToString();
      var nameParts = name.Split('.');

      var className = nameParts[nameParts.Count() - 1];
      if (nameParts.Count() > 1)
      {
        className = $"{nameParts[nameParts.Count() - 2]}{nameParts[nameParts.Count() - 1]}";
      }

      return className;
    }
  }
}
