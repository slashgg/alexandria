using Svalbard;

namespace Alexandria.Shared.ErrorKey
{
  public static class ProfanityFilter
  {
    public static ServiceError BlacklistedWord = new ServiceError("PROFANITY.BLACKLISTED", 400);
  }
}
