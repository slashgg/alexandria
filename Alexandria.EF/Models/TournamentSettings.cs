using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Alexandria.EF.Models
{
  [Owned]
  public class TournamentSettings
  {
    public int? RoundRobinWinPoints { get; set; }
    public int? RoundRobinDrawPoints { get; set; }
    public int? RoundRobinLossPoints { get; set; }
    public int? RoundRobinConsolidationPoint { get; set; }
    public int? RoundRobinConsolidationPointMinimumWins { get; set; }
  }
}
