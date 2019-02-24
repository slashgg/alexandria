using System;
using System.Collections.Generic;
using System.Linq;
using Alexandria.Shared.Enums;

namespace Alexandria.Orchestration.Utils.TournamentStandings
{
  public class RoundRobin
  {
    private readonly EF.Models.Tournament tournament;

    public RoundRobin(EF.Models.Tournament tournament)
    {
      this.tournament = tournament;
    }

    public DTO.Tournament.Standing<DTO.Tournament.RoundRobinRecord> GetStandings()
    {
      var teams = tournament.TournamentRounds
                            .SelectMany(tr => tr.MatchSeries)
                            .SelectMany(ms => ms.MatchParticipants)
                            .Select(mp => mp.Team)
                            .Distinct()
                            .ToList();

      var resultList = new List<DTO.Tournament.RoundRobinRecord>();

      foreach (var team in teams)
      {
        var matchesPlayed = team.MatchParticipations
          .Where(mp => mp.MatchSeries.TournamentRound.TournamentId == tournament.Id)
          .Select(mp => mp.MatchSeries)
          .Where(ms => ms.State == MatchState.Complete)
          .ToList();

        var wins = 0;
        var losses = 0;
        var draws = 0;
        var totalMatchWins = 0;
        var totalMatchLosses = 0;
        var totalMatchDraws = 0;
        foreach (var matchSeries in matchesPlayed)
        {
          var matchWins = 0;
          var matchLosses = 0;
          var matchDraws = 0;

          foreach (var match in matchSeries.Matches)
          {
            if (match.IsWinner(team.Id))
            {
              matchWins++;
            }
            else if (match.IsDraw(team.Id))
            {
              matchDraws++;
            }
            else
            {
              matchLosses++;
            }
          }

          totalMatchWins += matchWins;
          totalMatchLosses += matchLosses;
          totalMatchDraws += matchDraws;

          if (matchWins > matchLosses)
          {
            wins++;
          }
          else if (matchWins.Equals(matchLosses))
          {
            draws++;
          }
          else
          {
            losses++;
          }
        }

        var winPointsMultiplier = tournament.Settings?.RoundRobinWinPoints ?? 0;
        var drawPointsMultiplier = tournament.Settings?.RoundRobinDrawPoints ?? 0;
        var lossPointsMultiplier = tournament.Settings?.RoundRobinLossPoints ?? 0;
        var matchSeriesPlayed = wins + draws + losses;
        decimal winPercentage = 1;
        if ((wins + losses) > 0)
        {
          winPercentage = decimal.Round(Convert.ToDecimal(wins) / Convert.ToDecimal(wins + losses), 3);
        }

        var roundRobinResult = new DTO.Tournament.RoundRobinRecord
        {
          Draws = draws,
          Losses = losses,
          Wins = wins,
          MatchWins = totalMatchWins,
          MatchLosses = totalMatchLosses,
          MatchDraws = totalMatchDraws,
          TotalPoints = (wins * winPointsMultiplier) + (draws * drawPointsMultiplier) + (losses * lossPointsMultiplier),
          WinPercentage = Decimal.Round(winPercentage, 2),
          Team = AutoMapper.Mapper.Map<DTO.Team.Info>(team),
          MatchSeriesPlayed = matchSeriesPlayed
        };

        resultList.Add(roundRobinResult);
      }

      resultList = resultList.OrderByDescending(r => r.TotalPoints)
                             .ThenByDescending(r => r.Wins)
                             .ThenByDescending(r => r.MatchWins)
                             .ToList();

      return new DTO.Tournament.Standing<DTO.Tournament.RoundRobinRecord>(TournamentType.RoundRobin, resultList);
    }
  }
}
