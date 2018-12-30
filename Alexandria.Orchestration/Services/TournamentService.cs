using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alexandria.EF.Context;
using Alexandria.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Svalbard.Services;

namespace Alexandria.Orchestration.Services
{
  public class TournamentService : ITournamentService
  {
    private readonly AlexandriaContext alexandriaContext;

    public TournamentService(AlexandriaContext alexandriaContext)
    {
      this.alexandriaContext = alexandriaContext;
    }

    public async Task<ServiceResult<IList<DTO.Competition.TournamentApplication>>> GetOpenTournamentApplications(string competitionSlug)
    {
      var result = new ServiceResult<IList<DTO.Competition.TournamentApplication>>();

      var tournaments = await alexandriaContext.Tournaments.Include(t => t.TournamentApplicationQuestions)
                                                           .Include(t => t.Competition)
                                                           .Where(t => t.CanSignup && t.Competition.Slug == competitionSlug)
                                                           .ToListAsync();

      var tournamentDTOs = tournaments.Select(AutoMapper.Mapper.Map<DTO.Competition.TournamentApplication>).ToList();

      result.Succeed(tournamentDTOs);
      return result;
    }

    public async Task<ServiceResult<IList<DTO.Competition.TournamentApplication>>> GetOpenTournamentApplications(Guid competitionId)
    {
      var result = new ServiceResult<IList<DTO.Competition.TournamentApplication>>();

      var tournaments = await alexandriaContext.Tournaments.Include(t => t.TournamentApplicationQuestions).Where(t => t.CanSignup && t.CompetitionId == competitionId).ToListAsync();
      var tournamentDTOs = tournaments.Select(AutoMapper.Mapper.Map<DTO.Competition.TournamentApplication>).ToList();

      result.Succeed(tournamentDTOs);
      return result;
    }

    public async Task<ServiceResult<DTO.Competition.TournamentApplication>> GetTournamentApplication(Guid tournamentId)
    {
      var result = new ServiceResult<DTO.Competition.TournamentApplication>();
      var application = await alexandriaContext.TournamentApplications.Include(ta => ta.Tournament).FirstOrDefaultAsync(ta => ta.TournamentId == tournamentId);

      if (application == null)
      {
        result.ErrorKey = Shared.ErrorKey.Tournament.NoApplicationFound;
        return result;
      }

      var applicationDTO = AutoMapper.Mapper.Map<DTO.Competition.TournamentApplication>(application);

      result.Succeed(applicationDTO);
      return result;
    }

    public async Task<ServiceResult<DTO.Competition.TournamentApplication>> GetTournamentApplication(string tournamentSlug)
    {
      var result = new ServiceResult<DTO.Competition.TournamentApplication>();
      var application = await this.alexandriaContext.TournamentApplications.Include(ta => ta.Tournament).FirstOrDefaultAsync(ta => ta.Tournament.Slug == tournamentSlug);
      if (application == null)
      {
        result.ErrorKey = Shared.ErrorKey.Tournament.NoApplicationFound;
        return result;
      }

      var applicationDTO = AutoMapper.Mapper.Map<DTO.Competition.TournamentApplication>(application);

      result.Succeed(applicationDTO);
      return result;
    }
  }
}
