using System;
namespace Alexandria.Shared.Cache
{
  public static class Tournament
  {
    public static string Participants(Guid tournamentId)
    {
      return $"tournament::{tournamentId}::participants";
    }

    public static string Rounds(Guid tournamentId)
    {
      return $"tournament::{tournamentId}::rounds";
    }
  }
}
