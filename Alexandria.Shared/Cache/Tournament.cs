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

    public static string Schedule(Guid tournamentId)
    {
      return $"tournament::{tournamentId}::schedule";
    }
  }
}
