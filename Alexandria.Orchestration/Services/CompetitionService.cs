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
  public class CompetitionService : ICompetitionService
  {
    private readonly AlexandriaContext alexandriaContext;

    public CompetitionService(AlexandriaContext context)
    {
      this.alexandriaContext = context;
    }

    public async Task<ServiceResult<IList<DTO.Competition.Detail>>> GetCompetitionsByGame(Guid gameId)
    {
      var result = new ServiceResult<IList<DTO.Competition.Detail>>();
      var competitions = await this.alexandriaContext.Competitions.Include(c => c.Game).Where(c => c.GameId == gameId).ToListAsync();

      var competitionDTOs = competitions.Select(AutoMapper.Mapper.Map<DTO.Competition.Detail>).ToList();

      result.Succeed(competitionDTOs);
      return result;
    }

    public async Task<ServiceResult<DTO.Competition.Detail>> GetCompetitionBySlug(string slug)
    {
      var result = new ServiceResult<DTO.Competition.Detail>();

      var competition = await this.alexandriaContext.Competitions.Include(c => c.Game).FirstOrDefaultAsync(c => c.Slug == slug);

      result.Data = AutoMapper.Mapper.Map<DTO.Competition.Detail>(competition);
      result.Succeed();
      return result;
    }

    public async Task<ServiceResult<DTO.Competition.Detail>> GetCompetitionDetail(Guid competitionId)
    {
      var result = new ServiceResult<DTO.Competition.Detail>();
      var competition = await this.alexandriaContext.Competitions.Include(c => c.Game).FirstOrDefaultAsync(c => c.Id == competitionId);

      result.Data = AutoMapper.Mapper.Map<DTO.Competition.Detail>(competition);
      result.Succeed();

      return result;
    }

    public async Task<ServiceResult<IList<DTO.Competition.Detail>>> GetActiveCompetitions()
    {
      var result = new ServiceResult<IList<DTO.Competition.Detail>>();
      var competitions = await this.alexandriaContext.Competitions.Include(c => c.Game).Where(c => c.Active).ToListAsync();

      var competitionDTOs = competitions.Select(AutoMapper.Mapper.Map<DTO.Competition.Detail>).ToList();

      result.Succeed(competitionDTOs);
      return result;
    }


    public async Task<ServiceResult<IList<DTO.Competition.Tournament>>> GetTournaments(Guid competitionId)
    {
      var result = new ServiceResult<IList<DTO.Competition.Tournament>>();

      var tournaments = await alexandriaContext.Tournaments.Where(t => t.CompetitionId == competitionId).ToListAsync();
      var tournamentDTOs = tournaments.Select(AutoMapper.Mapper.Map<DTO.Competition.Tournament>).ToList();

      result.Succeed(tournamentDTOs);
      return result;
    }

    public async Task<ServiceResult<IList<DTO.Competition.Tournament>>> GetTournaments(string competitionSlug)
    {
      var result = new ServiceResult<IList<DTO.Competition.Tournament>>();

      var tournaments = await alexandriaContext.Tournaments.Include(t => t.Competition).Where(t => t.Competition.Slug == competitionSlug).ToListAsync();
      var tournamentDTOs = tournaments.Select(AutoMapper.Mapper.Map<DTO.Competition.Tournament>).ToList();

      result.Succeed(tournamentDTOs);
      return result;
    }

    public async Task<ServiceResult<DTO.Competition.Tournament>> GetTournament(Guid tournamentId)
    {
      var result = new ServiceResult<DTO.Competition.Tournament>();
      var tournament = await alexandriaContext.Tournaments.FirstOrDefaultAsync(t => t.Id == tournamentId);

      if (tournament == null)
      {
        result.Error = Shared.ErrorKey.Tournament.NotFound;
      }

      result.Data = AutoMapper.Mapper.Map<DTO.Competition.Tournament>(tournament);
      return result;
    }

    public async Task<ServiceResult<DTO.Competition.Tournament>> GetTournament(string slug)
    {
      var result = new ServiceResult<DTO.Competition.Tournament>();
      var tournament = await alexandriaContext.Tournaments.FirstOrDefaultAsync(t => t.Slug == slug);

      if (tournament == null)
      {
        result.Error = Shared.ErrorKey.Tournament.NotFound;
      }

      result.Data = AutoMapper.Mapper.Map<DTO.Competition.Tournament>(tournament);
      return result;
    }
  }
}
