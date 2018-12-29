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

    public async Task<ServiceResult<DTO.Competition.Detail>> GetCompetitionByName(string name)
    {
      var result = new ServiceResult<DTO.Competition.Detail>();
      name = $"/{name}";

      var competition = await this.alexandriaContext.Competitions.Include(c => c.Game).FirstOrDefaultAsync(c => c.Name == name);

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
  }
}
