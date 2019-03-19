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

    public async Task<ServiceResult<DTO.Admin.Competition.Detail>> GetCompetitionDetail(Guid competitionId)
    {
      var result = new ServiceResult<DTO.Admin.Competition.Detail>();

      var competition = await this.alexandriaContext.Competitions.Include(c => c.Game).Include(c => c.CompetitionLevel)
        .FirstOrDefaultAsync(c => c.Id == competitionId);

      if (competition == null)
      {
        result.Error = Shared.ErrorKey.Competition.NotFound;
        return result;
      }

      var competitionDTO = AutoMapper.Mapper.Map<DTO.Admin.Competition.Detail>(competition);
      result.Succeed(competitionDTO);

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

      this.DangerouslyCreateCompetition(competitionData);

      return result;
    }

    public async Task<ServiceResult<IList<DTO.Competition.Tournament>>> GetTournaments(Guid competitionId, int? page = null, int? limit = null)
    {
      var result = new ServiceResult<IList<DTO.Competition.Tournament>>();
      IQueryable<Tournament> tournamentQuery = this.alexandriaContext.Tournaments.Include(t => t.ParentTournament)
        .Where(t => t.CompetitionId == competitionId).OrderByDescending(t => t.CreatedAt);
      if (page.HasValue)
      {
        var paginationLimit = limit ?? 25;
        tournamentQuery = tournamentQuery.Skip(page.Value * paginationLimit).Take(paginationLimit);
      }

      var tournaments = await tournamentQuery.ToListAsync();
      var tournamentDTOs = tournaments.Select(AutoMapper.Mapper.Map<DTO.Competition.Tournament>).ToList();

      result.Succeed(tournamentDTOs);

      return result;
    }


    private void DangerouslyCreateCompetition(DTO.Admin.Competition.CreateData competitionData)
    {
      var competition = new Competition
      {
        Name = competitionData.Name,
        Slug = competitionData.Slug,
        Title = competitionData.Title,
        Description = competitionData.Description,
        MaxTeamSize = competitionData.MaxTeamSize,
        MinTeamSize = competitionData.MinTeamSize,
        RulesSlug = competitionData.RuleSlug,
        CompetitionLevelId = competitionData.CompetitionLevelId
      };

      this.alexandriaContext.Competitions.Add(competition);
    }
  }
}
