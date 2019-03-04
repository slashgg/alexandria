using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alexandria.EF.Context;
using Alexandria.EF.Models;
using Alexandria.Interfaces.Services;
using Alexandria.Shared.Enums;
using Alexandria.Shared.Permissions;
using Alexandria.Shared.Utils;
using Microsoft.EntityFrameworkCore;
using SlackAPI;
using Svalbard.Services;
using Competition = Alexandria.EF.Models.Competition;
using ExternalAccount = Alexandria.EF.Models.ExternalAccount;

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

    public async Task<ServiceResult<DTO.Casting.CasterMetaData>> GetCasterMetaData(Guid userId)
    {
      var result = new ServiceResult<DTO.Casting.CasterMetaData>();
      var user = await this.alexandriaContext.UserProfiles.Include(up => up.ExternalAccounts)
        .FirstOrDefaultAsync(up => up.Id == userId);

      if (user == null)
      {
        result.Error = Shared.ErrorKey.UserProfile.UserNotFound;
        return result;
      }
      
      var metaData = new DTO.Casting.CasterMetaData();

      ExternalAccount account = null;
      if (user.TryGetExternalAccount(ExternalProvider.Twitch, out account))
      {
        metaData.TwitchChannel = $"https://twitch.tv/{account.Name}";
      }
      result.Succeed(metaData);
      return result;
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

    public async Task<ServiceResult<IList<DTO.Casting.CastableMatchSeries>>> GetCastableMatchSeries(Guid competitionId)
    {
      var result = new ServiceResult<IList<DTO.Casting.CastableMatchSeries>>();

      var matchSeries = await this.alexandriaContext.MatchSeries.Include(ms => ms.Matches)
        .ThenInclude(m => m.Results)
        .ThenInclude(r => r.MatchParticipant)
        .Include(ms => ms.MatchParticipants)
        .ThenInclude(mp => mp.Team)
        .Include(ms => ms.TournamentRound)
        .ThenInclude(tr => tr.Tournament)
        .ThenInclude(t => t.Competition)
        .Where(ms => ms.State == MatchState.Pending)
        .Where(ms => ms.ScheduledAt != null)
        .Where(ms => ms.TournamentRound.Tournament.CompetitionId == competitionId)
        .Where(ms => !ms.MatchSeriesCastings.Any())
        .ToListAsync();

      var matchSeriesDTO = matchSeries.Select(AutoMapper.Mapper.Map<DTO.Casting.CastableMatchSeries>).ToList();
      result.Succeed(matchSeriesDTO);

      return result;
    }

    public async Task<ServiceResult<IList<DTO.Casting.CastableMatchSeries>>> GetClaimedMatches(Guid userId)
    {
      var result = new ServiceResult<IList<DTO.Casting.CastableMatchSeries>>();

      var claims = await this.alexandriaContext.MatchSeriesCastingClaims
        .Include(mscc => mscc.MatchSeries)
        .ThenInclude(ms => ms.Matches)
        .ThenInclude(m => m.Results)
        .ThenInclude(r => r.MatchParticipant)
        .Include(mscc => mscc.MatchSeries)
        .ThenInclude(ms => ms.MatchParticipants)
        .ThenInclude(mp => mp.Team)
        .Include(mscc => mscc.MatchSeries)
        .ThenInclude(ms => ms.TournamentRound)
        .ThenInclude(tr => tr.Tournament)
        .ThenInclude(t => t.Competition)
        .Where(mscc => mscc.UserProfileId == userId)
        .Where(ms => ms.MatchSeries.State == MatchState.Pending)
        .ToListAsync();

      var matches = claims.Select(c => c.MatchSeries);
      var matchDTOs = matches.Select(AutoMapper.Mapper.Map<DTO.Casting.CastableMatchSeries>).ToList();

      result.Succeed(matchDTOs);

      return result;
    }

    public async Task<ServiceResult<IList<DTO.Casting.Cast>>> GetScheduledCastsForUser(Guid userId)
    {
      var result = new ServiceResult<IList<DTO.Casting.Cast>>();

      var castings = await this.alexandriaContext.MatchSeriesCastings
        .Include(msc => msc.MatchSeries)
        .ThenInclude(ms => ms.Matches)
        .ThenInclude(m => m.Results)
        .ThenInclude(r => r.MatchParticipant)
        .Include(msc => msc.MatchSeries)
        .ThenInclude(ms => ms.MatchParticipants)
        .ThenInclude(mp => mp.Team)
        .Include(msc => msc.MatchSeries)
        .ThenInclude(ms => ms.TournamentRound)
        .ThenInclude(tr => tr.Tournament)
        .ThenInclude(t => t.Competition)
        .Include(msc => msc.MatchSeriesCastingParticipants)
        .ThenInclude(mscp => mscp.UserProfile)
        .Where(msc => msc.MatchSeriesCastingParticipants.Any(mscp => mscp.UserProfileId == userId))
        .OrderByDescending(msc => msc.StartsAt)
        .ToListAsync();

      var castingDTOs = castings.Select(AutoMapper.Mapper.Map<DTO.Casting.Cast>).ToList();
      result.Succeed(castingDTOs);
      return result;
    }

    public async Task<ServiceResult> ClaimGame(Guid userId, Guid matchSeriesId)
    {
      var result = new ServiceResult();

      var user = await this.alexandriaContext.UserProfiles.FirstOrDefaultAsync(up => up.Id == userId);
      if (user == null)
      {
        result.Error = Shared.ErrorKey.UserProfile.UserNotFound;
        return result;
      }

      var matchSeries = await this.alexandriaContext.MatchSeries
        .Include(ms => ms.MatchSeriesCastings)
        .Include(ms => ms.MatchSeriesCastingClaims)
        .FirstOrDefaultAsync(ms => ms.Id == matchSeriesId);

      if (matchSeries == null)
      {
        result.Error = Shared.ErrorKey.MatchSeries.NotFound;
        return result;
      }

      if (matchSeries.MatchSeriesCastings.Any())
      {
        result.Error = Shared.ErrorKey.Casting.AlreadyClaimed;
        return result;
      }

      if (matchSeries.MatchSeriesCastingClaims.Any(mscc => mscc.UserProfileId == userId))
      {
        result.Error = Shared.ErrorKey.Casting.ClaimAlreadyExists;
        return result;
      }

      await this.DangerouslyAddCastingClaim(matchSeries, userId);

      result.Succeed();

      return result;
    }

    public async Task<ServiceResult> RemoveClaim(Guid claimId)
    {
      var result = new ServiceResult();
      var claim = await this.alexandriaContext.MatchSeriesCastingClaims.FirstOrDefaultAsync(c => c.Id == claimId);

      if (claim == null)
      {
        result.Error = Shared.ErrorKey.Casting.ClaimNotFound;
        return result;
      }

      await this.DangerouslyRemoveCastingClaim(claim);

      result.Succeed();

      return result;
    }

    public async Task<ServiceResult> CreateGameCast(Guid userId, DTO.Casting.CreateCastData castData)
    {
      var result = new ServiceResult();

      var user = await this.alexandriaContext.UserProfiles.FirstOrDefaultAsync(up => up.Id == userId);
      if (user == null)
      {
        result.Error = Shared.ErrorKey.UserProfile.UserNotFound;
        return result;
      }

      var matchSeries = await this.alexandriaContext.MatchSeries
        .Include(ms => ms.MatchSeriesCastings)
        .Include(ms => ms.MatchSeriesCastingClaims)
        .FirstOrDefaultAsync(ms => ms.Id == castData.MatchSeriesId);

      if (matchSeries == null)
      {
        result.Error = Shared.ErrorKey.MatchSeries.NotFound;
        return result;
      }

      if (matchSeries.MatchSeriesCastings.Any())
      {
        result.Error = Shared.ErrorKey.Casting.AlreadyClaimed;
        return result;
      }

      if (!castData.StartsAt.HasValue)
      {
        castData.StartsAt = matchSeries.ScheduledAt;
      }

      var cast = this.DangerouslyCreateGameCast(userId, castData);
      result.Succeed();

      return result;
    }

    public async Task<ServiceResult> UpdateGameCast(Guid gameCastId, DTO.Casting.UpdateGameCast castData)
    {
      var result = new ServiceResult();

      var cast = await this.alexandriaContext.MatchSeriesCastings.FirstOrDefaultAsync(msc => msc.Id == gameCastId);
      if (cast == null)
      {
        result.Error = Shared.ErrorKey.Casting.CastNotFound;
        return result;
      }

      this.DangerouslyUpdateGameCast(cast, castData);
      result.Succeed();

      return result;
    }

    public async Task<ServiceResult> DeleteGameCast(Guid gameCastId)
    {
      var result = new ServiceResult();
      var cast = await this.alexandriaContext.MatchSeriesCastings.Include(msc => msc.MatchSeriesCastingParticipants)
        .FirstOrDefaultAsync(msc => msc.Id == gameCastId);
      if (cast == null)
      {
        result.Error = Shared.ErrorKey.Casting.CastNotFound;
        return result;
      }

      await DangerouslyDeleteGameCast(cast);
      result.Succeed();

      return result;
    }


    private async Task DangerouslyAddCastingClaim(MatchSeries series, Guid userId)
    {
      var claim = new MatchSeriesCastingClaim(userId);
      series.StakeCastingClaim(claim);
      await this.authorizationService.AddPermission(userId,
        AuthorizationHelper.GenerateARN(typeof(MatchSeriesCastingClaim), claim.Id.ToString(),
          Shared.Permissions.CastingClaim.All));

      this.alexandriaContext.MatchSeries.Update(series);
    }

    private async Task DangerouslyRemoveCastingClaim(MatchSeriesCastingClaim claim)
    {
      await this.authorizationService.RemovePermission(claim.UserProfileId, AuthorizationHelper.GenerateARN(
        typeof(MatchSeriesCastingClaim), claim.Id.ToString(),
        Shared.Permissions.CastingClaim.All));
      this.alexandriaContext.MatchSeriesCastingClaims.Remove(claim);
    }

    private async Task<MatchSeriesCasting> DangerouslyCreateGameCast(Guid userId, DTO.Casting.CreateCastData castData)
    {
      var cast = new MatchSeriesCasting(castData.StreamURL, castData.StartsAt);
      cast.AddParticipant(userId, CastingRole.PlayByPlay);
      cast.MatchSeriesId = castData.MatchSeriesId;

      this.alexandriaContext.MatchSeriesCastings.Add(cast);
      await this.authorizationService.AddPermission(userId,
        AuthorizationHelper.GenerateARN(typeof(MatchSeriesCasting), cast.Id.ToString(), GameCast.All));


      return cast;
    }

    private MatchSeriesCasting DangerouslyUpdateGameCast(MatchSeriesCasting cast, DTO.Casting.UpdateGameCast castData)
    {
      cast.StartsAt = castData.StartsAt;
      cast.StreamingURL = castData.StreamURL;
      cast.VODURL = castData.VODURL;

      this.alexandriaContext.MatchSeriesCastings.Update(cast);

      return cast;
    }

    private async Task DangerouslyDeleteGameCast(MatchSeriesCasting cast)
    {
      await this.authorizationService.RemovePermissionForResource(cast);
      alexandriaContext.MatchSeriesCastingParticipants.RemoveRange(cast.MatchSeriesCastingParticipants);
      alexandriaContext.MatchSeriesCastings.Remove(cast);
    }
  }
}
