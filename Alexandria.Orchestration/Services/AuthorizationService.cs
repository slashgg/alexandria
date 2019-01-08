using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alexandria.EF.Context;
using Alexandria.EF.Models;
using Alexandria.Interfaces.Services;
using Alexandria.Shared.Utils;
using Microsoft.EntityFrameworkCore;

namespace Alexandria.Orchestration.Services
{
  public class AuthorizationService : IAuthorizationService
  {

    private readonly AlexandriaContext alexandriaContext;

    public AuthorizationService(AlexandriaContext alexandriaContext)
    {
      this.alexandriaContext = alexandriaContext;
    }


    public async Task AddPermission(Guid userId, string permission)
    {
      var user = await this.alexandriaContext.UserProfiles.Include(u => u.Permissions).FirstOrDefaultAsync(u => u.Id == userId);
      if (user != null && !user.HasPermission(permission))
      {
        var permissionModel = new Permission(userId, permission);
        this.alexandriaContext.Add(permissionModel);
      }
    }

    public async Task AddPermission(Guid userId, IList<string> permissions)
    {
      var user = await this.alexandriaContext.UserProfiles.Include(u => u.Permissions).FirstOrDefaultAsync(u => u.Id == userId);
      var unusedPermission = permissions.Where(p => !user.HasPermission(p));

      var permissionModels = unusedPermission.Select(p => new Permission(userId, p));
      this.alexandriaContext.AddRange(permissionModels);
    }

    public async Task<bool> Can(Guid userId, string permission)
    {
      var user = await this.alexandriaContext.UserProfiles.Include(u => u.Permissions).FirstOrDefaultAsync(u => u.Id == userId);
      var can = user.HasPermission(permission);

      if (!can)
      {
        var masterPermission = AuthorizationHelper.MasterPermission(permission);
        can = user.HasPermission(masterPermission);
      }

      return can;
    }

    public async Task RemovePermission(Guid userId, string permission)
    {
      var permissionModel = await this.alexandriaContext.Permissions.FirstOrDefaultAsync(p => p.ARN == permission && p.UserProfileId == userId);
      if (permissionModel != null)
      {
        this.alexandriaContext.Remove(permissionModel);
      }
    }

    public async Task RemovePermission(Guid userId, IList<string> permissions)
    {
      var permissonModels = await alexandriaContext.Permissions.Where(p => p.Id == userId && permissions.Contains(p.ARN)).ToListAsync();
      if (permissonModels.Any())
      {
        this.alexandriaContext.RemoveRange(permissonModels);
      }
    }
  }
}
