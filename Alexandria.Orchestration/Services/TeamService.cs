using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alexandria.EF.Context;
using Alexandria.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Svalbard.Services;

namespace Alexandria.Orchestration.Services
{
  public class TeamService : ITeamService
  {
    private readonly HttpContext httpContext;
    private readonly AlexandriaContext context;

    public TeamService(IHttpContextAccessor httpContext, AlexandriaContext context)
    {
      this.httpContext = httpContext.HttpContext;
      this.context = context;
    }

    public async Task<ServiceResult> CreateTeam(DTO.Team.Create teamData)
    {
      var result = new ServiceResult();

      var usr = this.httpContext.User;
      return result;
    }
  }
}
