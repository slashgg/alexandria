using System;
using System.Collections.Generic;
using System.Text;
using Svalbard;

namespace Alexandria.Shared.ErrorKey
{
  public static class Match
  {
    public static ServiceError NotFound = new ServiceError("MATCH.NOT_FOUND", 404);
    public static ServiceError AlreadyReported = new ServiceError("MATCH.ALREADY_REPORTED", 409);
    public static ServiceError InvalidParticipant = new ServiceError("MATCH.INVALID_PARTICIPANT", 400);
  }
}
