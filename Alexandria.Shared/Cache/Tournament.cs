using System;
using System.Collections.Generic;
using System.Text;

namespace Alexandria.Shared.Cache
{
  public static class Tournament
  {
    public static string Participants(Guid tournamentId)
    {
      return $"tournament::{tournamentId}::participants";
    }
  }
}
