using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alexandria.EF.Context;
using Alexandria.EF.Models;
using Alexandria.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Svalbard.Services;

namespace Alexandria.Orchestration.Services
{
  public class GameCastingService : IGameCastingService
  {
    private readonly AlexandriaContext alexandriaContext;
    private readonly IAuthorizationService authorizationService;

    public GameCastingService(AlexandriaContext alexandriaContext, IAuthorizationService authorizationService)
    {
      this.alexandriaContext = alexandriaContext;
      this.authorizationService = authorizationService;
    }

    public async Task<ServiceResult<IList<DTO.Competition.Info>>> GetCastableCompetitions(Guid userId)
    {
      var result = new ServiceResult<IList<DTO.Competition.Info>>();

      var resourceIds = await this.authorizationService.GetAvailableResources<Competition>(userId, Shared.Permissions.Competition.CastGame);

      var competitions = await this.alexandriaContext.Competitions.Where(c => resourceIds.Contains(c.Id)).ToListAsync();
      var competitionDTOs = competitions.Select(AutoMapper.Mapper.Map<DTO.Competition.Info>).ToList();
      result.Succeed(competitionDTOs);

      return result;
    }
  }
}
