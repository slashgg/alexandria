﻿using Svalbard;

namespace Alexandria.Shared.ErrorKey
{
  public class Tournament
  {
    public static ServiceError NotFound = new ServiceError("TOURNAMENT.NOT_FOUND", 404);
    public static ServiceError ApplicationsClosed = new ServiceError("TOURNAMENT.APPLICATIONS_CLOSED", 423);
    public static ServiceError InvalidStandingType = new ServiceError("TOURNAMENT.INVALID_STANDING_TYPE", 400);
  }
}
