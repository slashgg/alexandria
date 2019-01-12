using System;
using System.Collections.Generic;
using System.Text;

namespace Alexandria.Shared.Configuration
{
  public static class UserProfileValidations
  {
    public static readonly int MaxAgeInDays = 120 * 365;
    public static readonly int MinAgeInDays = 13 * 365;
  }
}
