using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Svalbard.Services;

namespace Alexandria.Interfaces.Services
{
  public interface IGameCastingService
  {
    Task<ServiceResult<DTO.Casting.CasterMetaData>> GetCasterMetaData(Guid userId);
    Task<ServiceResult<IList<DTO.Competition.Info>>> GetCastableCompetitions(Guid userId);
    Task<ServiceResult<IList<DTO.Casting.CastableMatchSeries>>> GetCastableMatchSeries(Guid competitionId);
    Task<ServiceResult<IList<DTO.Casting.CastableMatchSeries>>> GetClaimedMatches(Guid userId);
    Task<ServiceResult<IList<DTO.Casting.Cast>>> GetScheduledCastsForUser(Guid userId);
    Task<ServiceResult> ClaimGame(Guid userId, Guid matchSeriesId);
    Task<ServiceResult> CreateGameCast(Guid userId, DTO.Casting.CreateCastData castData);
    Task<ServiceResult> UpdateGameCast(Guid gameCastId, DTO.Casting.UpdateGameCast castData);
    Task<ServiceResult> RemoveClaim(Guid claimId);
    Task<ServiceResult> DeleteGameCast(Guid gameCastId);
  }
}
