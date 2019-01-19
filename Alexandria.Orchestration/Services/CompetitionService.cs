using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alexandria.EF.Context;
using Alexandria.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Svalbard.Services;

namespace Alexandria.Orchestration.Services
{
  public class CompetitionService : ICompetitionService
  {
    private readonly AlexandriaContext alexandriaContext;
    private readonly IMemoryCache cache;

    public CompetitionService(AlexandriaContext context, IMemoryCache cache)
    {
      this.alexandriaContext = context;
      this.cache = cache;
    }

    public async Task<ServiceResult<IList<DTO.Competition.Detail>>> GetCompetitionsByGame(Guid gameId)
    {
      var result = new ServiceResult<IList<DTO.Competition.Detail>>();

      var competitionDTOs = await this.cache.GetOrCreateAsync(Shared.Cache.Competition.ByGame(gameId), async (cache) =>
      {
        var competitions = await this.alexandriaContext.Competitions.Include(c => c.Game)
                                                            .Include(c => c.Teams)
                                                            .ThenInclude(t => t.TeamMemberships)
                                                            .Where(c => c.GameId == gameId)
                                                            .ToListAsync();

        var competitionResult = competitions.Select(AutoMapper.Mapper.Map<DTO.Competition.Detail>).ToList();
        cache.SetAbsoluteExpiration(DateTimeOffset.UtcNow.AddHours(1));
        return competitionResult;
      });


      result.Succeed(competitionDTOs);
      return result;
    }

    public async Task<ServiceResult<DTO.Competition.Detail>> GetCompetitionDetail(string slug)
    {
      var result = new ServiceResult<DTO.Competition.Detail>();

      var competition = await this.alexandriaContext.Competitions.FirstOrDefaultAsync(c => c.Slug == slug);
      if (competition == null)
      {
        result.Error = Shared.ErrorKey.Competition.NotFound;
        return result;
      }

      return await this.GetCompetitionDetail(competition.Id);
    }

    public async Task<ServiceResult<DTO.Competition.Detail>> GetCompetitionDetail(Guid competitionId)
    {
      var result = new ServiceResult<DTO.Competition.Detail>();

      var competitionDTO = await this.cache.GetOrCreateAsync(Shared.Cache.Competition.Detail(competitionId), async (cache) =>
      {
        var competition = await this.alexandriaContext.Competitions.Include(c => c.Game)
                                                                   .Include(c => c.Teams)
                                                                   .ThenInclude(t => t.TeamMemberships)
                                                                   .FirstOrDefaultAsync(c => c.Id == competitionId);

        if (competition == null)
        {
          cache.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(0);
          return null;
        }

        var dto = AutoMapper.Mapper.Map<DTO.Competition.Detail>(competition);
        cache.SetAbsoluteExpiration(DateTimeOffset.UtcNow.AddHours(1));
        return dto;
      });

      if (competitionDTO == null)
      {
        result.Error = Shared.ErrorKey.Competition.NotFound;
        return null; ;
      }

      result.Succeed(competitionDTO);

      return result;
    }

    public async Task<ServiceResult<IList<DTO.Competition.Detail>>> GetActiveCompetitions()
    {
      var result = new ServiceResult<IList<DTO.Competition.Detail>>();
      var competitionDTOs = await this.cache.GetOrCreateAsync(Shared.Cache.Competition.Active, async (cache) =>
      {
        var competitions = await this.alexandriaContext.Competitions.Include(c => c.Game)
                                                            .Include(c => c.Teams)
                                                            .ThenInclude(t => t.TeamMemberships)
                                                            .Where(c => c.Active).ToListAsync();

        var dtos = competitions.Select(AutoMapper.Mapper.Map<DTO.Competition.Detail>).ToList();
        cache.SetAbsoluteExpiration(DateTimeOffset.UtcNow.AddHours(1));
        return dtos;
      });


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
        return result;
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
        return result;
      }

      result.Data = AutoMapper.Mapper.Map<DTO.Competition.Tournament>(tournament);
      return result;
    }
  }
}
