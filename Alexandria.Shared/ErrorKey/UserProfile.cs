using Svalbard;

namespace Alexandria.Shared.ErrorKey
{
  public static class UserProfile
  {
    public static ServiceError UserNotFound = new ServiceError("USER_PROFILE.NOT_FOUND", 404);
    public static ServiceError InvalidUserSettings = new ServiceError("USER_PROFILE.INVALID_SETTINGS", 400);
    public static ServiceError UserTooOld = new ServiceError("USER_PROFILE.TOO_OLD", 400);
    public static ServiceError UserTooYoung = new ServiceError("USER_PROFILE.TOO_YOUNG", 400);
    public static ServiceError ResendVerificationFailed = new ServiceError("USER_PROFILE.RESEND_VERIFICATION_FAILED", 500);
    public static ServiceError UserUpdateFailed = new ServiceError("USER_PROFILE.USER_UPDATE_FAILED", 500);
    public static ServiceError ProfileExists = new ServiceError("USER_PROFILE.ALREADY_EXISTS", 409);
    public static ServiceError AlreadyInCompetitionTeam = new ServiceError("USER_PROFILE.ALREADY_IN_COMPETITION_TEAM", 409);
    public static ServiceError InvalidExternalId = new ServiceError("USER_PROFILE.INVALID_EXTERNAL_ID", 400);
    public static ServiceError InvalidExternalName = new ServiceError("USER_PROIFLE.INVALID_EXTERNAL_NAME", 400);
    public static ServiceError ExternalAccountExists = new ServiceError("USER_PROFILE.EXTERNAL_ACCOUNT_EXISTS", 409);
    public static ServiceError ExternalAccountNotFound = new ServiceError("USER_PROFILE.EXTERNAL_ACCOUNT_NOT_FOUND", 404);
  }
}
