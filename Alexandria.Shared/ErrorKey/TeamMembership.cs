using Svalbard;

namespace Alexandria.Shared.ErrorKey
{
  public static class TeamMembership
  {
    public static ServiceError NotFound = new ServiceError("TEAM_MEMBERSHIP.NOT_FOUND", 404);
    public static ServiceError LastMember = new ServiceError("TEAM_MEMBERSHIP.LAST_MEMBER", 422);
    public static ServiceError Protected = new ServiceError("TEAM_MEMBERSHIP.PROTECTED", 423);
  }
}
