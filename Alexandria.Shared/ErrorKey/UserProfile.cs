using Svalbard;

namespace Alexandria.Shared.ErrorKey
{
  public static class UserProfile
  {
    public static ServiceError UserNotFound = new ServiceError("USER_PROFILE.NOT_FOUND", 404);
    public static ServiceError ProfileExists = new ServiceError("USER_PROFILE.ALREADY_EXISTS", 409);
    public static ServiceError AlreadyInCompetitionTeam = new ServiceError("USER_PROFILE.ALREADY_IN_COMPETITION_TEAM", 409);
  }
}
