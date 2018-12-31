using System;
using System.Collections.Generic;
using System.Text;
using Svalbard;

namespace Alexandria.Shared.ErrorKey
{
  public static class TournamentApplication
  {
    public static ServiceError NotFound = new ServiceError("TOURNAMENT_APPLICATION.NOT_FOUND", 404);
  }
}
