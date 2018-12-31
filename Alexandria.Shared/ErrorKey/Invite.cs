using Svalbard;

namespace Alexandria.Shared.ErrorKey
{
  public static class Invite
  {
    public static ServiceError NotFound = new ServiceError("INVITE.NOT_FOUND", 404);
    public static ServiceError AlreadyInvited = new ServiceError("INVITE.ALREADY_INVITED", 409);
    public static ServiceError InvalidRecipient = new ServiceError("INVITE.INVALID_RECIPIENT", 422);
    public static ServiceError InvalidUser = new ServiceError("INVITE.INVALID_USER", 404);
    public static ServiceError AlreadyUsed = new ServiceError("INVITE.ALREADY_USED", 422);
  }
}
