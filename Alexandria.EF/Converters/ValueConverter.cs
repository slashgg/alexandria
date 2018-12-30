using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Alexandria.EF.Converters
{
  public static class AlexandriaValueConverter
  {
    public static ValueConverter<IList<string>, string> SplitStringConverter { get; } = new ValueConverter<IList<string>, string>(x => String.Join(';', x.ToArray()), x => x.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList());
  }
}
