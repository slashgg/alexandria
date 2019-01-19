using System;

namespace Alexandria.Shared.Cache
{
  public static class Competition
  {
    public static string Active = "competitions::active";
    public static string Detail(Guid id)
    {
      return $"competitions::by-id::{id}";
    }

    public static string ByGame(Guid gameId)
    {
      return $"competitions::by-game::{gameId}";
    }
  }
}
