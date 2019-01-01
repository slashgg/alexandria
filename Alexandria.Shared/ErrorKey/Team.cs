using Svalbard;

namespace Alexandria.Shared.ErrorKey
{
  public static class Team
  {
    public static ServiceError TeamNotFound = new ServiceError("TEAM.NOT_FOUND", 404);
    public static ServiceError AlreadyInTeam = new ServiceError("TEAM.ALREADY_IN_TEAM", 409);
    public static ServiceError NameTaken = new ServiceError("TEAM.NAME_TAKEN", 400);
    public static ServiceError Disbanded = new ServiceError("TEAM.DISBANDED", 410);
  }
}
