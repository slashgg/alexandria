using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Alexandria.Interfaces.Services
{
  public interface IAuthorizationService
  {
    Task<bool> Can(Guid userId, string permission);
    Task AddPermission(Guid userId, string permission);
    Task RemovePermission(Guid userId, string permission);
    Task AddPermission(Guid userId, IList<string> permissions);
    Task RemovePermission(Guid userId, IList<string> permissions);
    Task RemovePermissionForResource(IBaseEntity resource);
    Task RemovePermissionForResource(IBaseEntity resource, Guid userId);

    Task<List<Guid>> GetAvailableResources<T>(Guid userId, string permission, bool includeWildcard = false)
      where T : class, IBaseEntity;

    Task<List<Guid>> GetAvailableResources<T>(Guid userId, string permission, string permissionNamespace,
      bool includeWildcard = false) where T : class, IBaseEntity;
  }
}
