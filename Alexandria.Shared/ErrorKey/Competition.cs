using System;
using System.Collections.Generic;
using System.Text;
using Svalbard;

namespace Alexandria.Shared.ErrorKey
{
  public static class Competition
  {
    public static ServiceError NotFound = new ServiceError("COMPETITION.NOT_FOUND", 404);
    public static ServiceError NoDefaultRoleSet = new ServiceError("COMPETITION.NO_DEFAULT_ROLE", 409);
    public static ServiceError MaxTeamSize = new ServiceError("COMPETITION.MAX_TEAM_SIZE", 409);
  }
}
