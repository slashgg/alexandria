using System;
using System.Collections.Generic;
using System.Text;
using Svalbard;

namespace Alexandria.Shared.ErrorKey
{
  public static class MatchSeries
  {
    public static ServiceError NotFound = new ServiceError("MATCH_SERIES.NOT_FOUND", 404);
  }
}
