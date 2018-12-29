using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NJsonSchema.Generation;

namespace Alexandria.Utils.Swagger
{
  public class AlexandriaReflection : DefaultReflectionService, IReflectionService
  {
    public override JsonTypeDescription GetDescription(Type type, IEnumerable<Attribute> parentAttributes, JsonSchemaGeneratorSettings settings)
    {
      var attr = base.GetDescription(type, parentAttributes, settings);
      return attr;
    }

  }
}
