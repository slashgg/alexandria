using Svalbard;

namespace Alexandria.Shared.ErrorKey
{
  public static class TeamMembership
  {
    public static ServiceError NotFound = new ServiceError("MEMBERSHIP.NOT_FOUND", 404);
    public static ServiceError LastMember = new ServiceError("MEMBERSHIP.LAST_MEMBER", 422);
    public static ServiceError Protected = new ServiceError("MEMBERSHIP.PROTECTED", 423);
    public static ServiceError AlreadyMember = new ServiceError("MEMBERSIP.ALREADY_MEMBER", 409);
  }
}
