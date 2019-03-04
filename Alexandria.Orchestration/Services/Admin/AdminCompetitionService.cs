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

namespace Alexandria.Orchestration.Services.Admin
{
  public class AdminCompetitionService
  {
    private readonly IAuthorizationService authorizationService;
    private readonly AlexandriaContext alexandriaContext;

    public AdminCompetitionService(AlexandriaContext alexandriaContext, IAuthorizationService authorizationService)
    {
      this.alexandriaContext = alexandriaContext;
      this.authorizationService = authorizationService;
    }


    public async Task<ServiceResult<IList<DTO.Competition.Info>>> GetCompetitionsAvailableToUser(Guid userId)
    {
      var result = new ServiceResult<IList<DTO.Competition.Info>>();

      var competitionIds = await this.authorizationService.GetAvailableResources<Competition>(userId, "*", Shared.Permissions.Namespace.Admin, true);
      var competitions =
        await this.alexandriaContext.Competitions.Where(c => competitionIds.Contains(c.Id)).ToListAsync();

      var competitionDTOs = competitions.Select(AutoMapper.Mapper.Map<DTO.Competition.Info>).ToList();
      result.Succeed(competitionDTOs);

      return result;
    }

  }
}
