using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NJsonSchema;

namespace Alexandria.Utils.Swagger
{
  public class AlexandriaNameGenerator : ITypeNameGenerator
  {
    public string Generate(JsonSchema4 schema, string typeNameHint, IEnumerable<string> reservedTypeNames)
    {
      return "whatthefuck";
    }
  }
}
