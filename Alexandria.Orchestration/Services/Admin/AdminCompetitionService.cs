using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alexandria.EF.Context;
using Alexandria.EF.Models;
using Alexandria.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Svalbard;
using Svalbard.Services;

namespace Alexandria.Orchestration.Services.Admin
{
  public class AdminCompetitionService
  {
    private readonly IAuthorizationService authorizationService;
    private readonly AlexandriaContext alexandriaContext;
    private readonly IInputValidationService inputValidationService;

    public AdminCompetitionService(AlexandriaContext alexandriaContext, IAuthorizationService authorizationService, IInputValidationService inputValidationService)
    {
      this.alexandriaContext = alexandriaContext;
      this.authorizationService = authorizationService;
      this.inputValidationService = inputValidationService;
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

    public async Task<ServiceResult> CreateCompetition(DTO.Admin.Competition.CreateData competitionData)
    {
      var result = new ServiceResult();
      await this.inputValidationService.Validate(competitionData, result);
      if (result.FieldErrors.Any())
      {
        return result;
      }



      return result;
    }


  }
}
