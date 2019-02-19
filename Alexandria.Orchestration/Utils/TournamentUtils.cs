using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alexandria.EF.Context;
using Microsoft.EntityFrameworkCore;

namespace Alexandria.Orchestration.Utils
{
  public class TournamentUtils
  {
    private readonly AlexandriaContext alexandriaContext;
    public TournamentUtils(AlexandriaContext alexandriaContext)
    {
      this.alexandriaContext = alexandriaContext;
    }

    public async Task<DTO.Tournament.TournamentRecord> GetTournamentRecordForTeam(Guid teamId, Guid tournamentId)
    {
      var team = await this.alexandriaContext.Teams.Include(t => t.MatchParticipations)
                                                   .ThenInclude(mp => mp.MatchSeries)
                                                   .ThenInclude(ms => ms.MatchParticipants)
                                                   .Include(t => t.MatchParticipations)
                                                   .ThenInclude(mp => mp.MatchSeries)
                                                   .ThenInclude(ms => ms.Matches)
                                                   .ThenInclude(m => m.Results)
                                                   .Include(t => t.MatchParticipations)
                                                   .ThenInclude(mp => mp.MatchSeries)
                                                   .ThenInclude(ms => ms.TournamentRound)
                                                   .ThenInclude(tr => tr.Tournament)
                                                   .FirstOrDefaultAsync(t => t.Id == teamId);


      var tournamentMatches = team.MatchParticipations.Where(mp => mp.MatchSeries.TournamentRound.TournamentId == tournamentId).Select(mp => mp.MatchSeries).Where(ms => ms.State != Shared.Enums.MatchState.Pending).ToList();
      var wins = tournamentMatches.Where(m => m.Winner != null).Select(ms => ms.Winner).Count(mp => mp.TeamId == teamId);
      var losses = tournamentMatches.Where(m => m.Loser != null).Select(ms => ms.Loser).Count(mp => mp.TeamId == teamId);
      var draws = tournamentMatches.Count() - (wins + losses);

      return new DTO.Tournament.TournamentRecord(wins, losses, draws);
    }

    public async Task<IList<EF.Models.Tournament>> GetTournamentParents(Guid tournamentId, int? steps = null, int? currentStep = null)
    {
      if (steps.HasValue && currentStep.HasValue && steps.Value >= currentStep.Value)
      {
        return null;
      }

      currentStep++;
      var list = new List<EF.Models.Tournament>();
      var tournament = await alexandriaContext.Tournaments.FirstOrDefaultAsync(t => t.Id == tournamentId);
      if (tournament == null)
      {
        return list;
      }

      if (tournament.ParentTournamentId.HasValue)
      {
        var parentTournaments = await GetTournamentParents(tournament.ParentTournamentId.Value, steps, currentStep);
        list.AddRange(parentTournaments);
      }

      return list;
    }

    public async Task<IList<EF.Models.Tournament>> GetTournamentTree(Guid tournamentId, int? steps = null, int? currentStep = null)
    {
      if (steps.HasValue && currentStep.HasValue && steps.Value >= currentStep.Value)
      {
        return null;
      }

      currentStep++;

      var list = new List<EF.Models.Tournament>();
      var tournament = await alexandriaContext.Tournaments.Include(t => t.Tournaments).ThenInclude(t => t.Tournaments).FirstOrDefaultAsync(t => t.Id == tournamentId);

      if (tournament == null)
      {
        return new List<EF.Models.Tournament>();
      }

      if (tournament.Tournaments.Any())
      {
        foreach (var childTournament in tournament.Tournaments.ToList())
        {
          if (childTournament.Tournaments.Any())
          {
            list.AddRange(await GetTournamentTree(childTournament.Id, steps, currentStep));
          }
          else
          {
            list.Add(childTournament);
          }
        }
      }
      else
      {
        list.Add(tournament);
      }

      return list;
    }
  }
}
