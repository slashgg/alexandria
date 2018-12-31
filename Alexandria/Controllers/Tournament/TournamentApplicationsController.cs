﻿using System;
using System.Threading.Tasks;
using Alexandria.Infrastructure;
using Alexandria.Infrastructure.Filters;
using Alexandria.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Svalbard;

namespace Alexandria.Controllers.Tournament
{
  [Route("tournaments/{tournamentId}/applications")]
  [ResourceSelectFilter("tournamentId")]
  [ApiController]
  public class TournamentApplicationsController : ResourceBaseController
  {
    private readonly ITournamentService tournamentService;

    public TournamentApplicationsController(ITournamentService tournamentService)
    {
      this.tournamentService = tournamentService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(DTO.Competition.TournamentApplication), 200)]
    [ProducesResponseType(typeof(void), 404)]
    public async Task<OperationResult<DTO.Competition.TournamentApplication>> GetApplication()
    {
      if (this.resourceId != Guid.Empty)
      {
        var result = await tournamentService.GetTournamentApplication(this.resourceId);
        if (result.Success)
        {
          return new OperationResult<DTO.Competition.TournamentApplication>(result.Data);
        }

        return new OperationResult<DTO.Competition.TournamentApplication>(404, result.ErrorKey);
      } else
      {
        var result = await this.tournamentService.GetTournamentApplication(this.Slug);
        if (result.Success)
        {
          return new OperationResult<DTO.Competition.TournamentApplication>(result.Data);
        }
        return new OperationResult<DTO.Competition.TournamentApplication>(404, result.ErrorKey);
      }
    }
  }
}