using System;
using System.Collections.Generic;
using System.Text;
using Svalbard;

namespace Alexandria.Shared.ErrorKey
{
  public static class FavoriteCompetition
  {
    public static ServiceError NotFound = new ServiceError("FAVORITE_COMPETITION.NOT_FOUND", 404);
    public static ServiceError AlreadyExists = new ServiceError("FAVORITE_COMPETITION.ALREADY_EXISTS", 409);
  }
}
