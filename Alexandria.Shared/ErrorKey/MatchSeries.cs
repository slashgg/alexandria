using System;
using System.Collections.Generic;
using System.Text;
using Svalbard;

namespace Alexandria.Shared.ErrorKey
{
  public static class MatchSeries
  {
    public static ServiceError NotFound = new ServiceError("MATCH_SERIES.NOT_FOUND", 404);
    public static ServiceError AlreadyReported = new ServiceError("MATCH_SERIES.ALREADY_REPORTED", 409);
    public static ServiceError InvalidParticipant = new ServiceError("MATCH_SERIES.INVALID_PARTICIPANT", 400);
  }
}
