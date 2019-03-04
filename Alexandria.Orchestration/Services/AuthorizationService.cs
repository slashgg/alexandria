using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alexandria.EF.Context;
using Alexandria.EF.Models;
using Alexandria.Interfaces;
using Alexandria.Interfaces.Services;
using Alexandria.Shared.Utils;
using Microsoft.EntityFrameworkCore;
using Remotion.Linq.Clauses;

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
      var permissionModel = await alexandriaContext.Permissions.Where(p => p.Id == userId && permissions.Contains(p.ARN)).ToListAsync();
      if (permissionModel.Any())
      {
        this.alexandriaContext.RemoveRange(permissionModel);
      }
    }

    public async Task RemovePermissionForResource(IBaseEntity resource)
    {
      var resourceName = resource.GetPermissionIdentifier();
      if (resourceName == null)
      {
        return;
      }

      var permissions = await alexandriaContext.Permissions.Where(p =>
          Microsoft.EntityFrameworkCore.EF.Functions.Like(p.ARN, $"{resourceName}::{resource.Id.ToString()}::%"))
        .ToListAsync();

      this.alexandriaContext.Permissions.RemoveRange(permissions);
    }

    public async Task RemovePermissionForResource(IBaseEntity resource, Guid userId)
    {
      var resourceName = resource.GetPermissionIdentifier();
      if (resourceName == null)
      {
        return;
      }

      var permissions = await alexandriaContext.Permissions.Where(p =>
          Microsoft.EntityFrameworkCore.EF.Functions.Like(p.ARN, $"{resourceName}::{resource.Id.ToString()}::%"))
        .Where(p => p.UserProfileId == userId)
        .ToListAsync();

      this.alexandriaContext.Permissions.RemoveRange(permissions);
    }

    public async Task<List<Guid>> GetAvailableResources<T>(Guid userId, string permission, bool includeWildcard = false) where T : class, IBaseEntity
    {
      var resources = new List<Guid>();
      var resourceName = AuthorizationHelper.GetResourceName(typeof(T));
      if (resourceName == null)
      {
        return resources;
      }

      var permissionQuery = this.alexandriaContext.Permissions
        .Where(p => p.UserProfileId == userId)
        .Where(p => Microsoft.EntityFrameworkCore.EF.Functions.Like(p.ARN, $"{resourceName}::%::{permission}") ||
                    Microsoft.EntityFrameworkCore.EF.Functions.Like(p.ARN, $"{resourceName}::%::*"))
        .Where(p =>
          !Microsoft.EntityFrameworkCore.EF.Functions.Like(p.ARN, $"{resourceName}::*::{permission}") &&
          !Microsoft.EntityFrameworkCore.EF.Functions.Like(p.ARN, $"{resourceName}::*::*"));

      if (includeWildcard)
      {
        var hasWildcard = await this.alexandriaContext.Permissions.Where(p => p.UserProfileId == userId).Where(p =>
            Microsoft.EntityFrameworkCore.EF.Functions.Like(p.ARN, $"{resourceName}::*::*") ||
            Microsoft.EntityFrameworkCore.EF.Functions.Like(p.ARN,
              $"{resourceName}::*::{permission}"))
          .AnyAsync();

        if (hasWildcard)
        {
          var allIds = await this.alexandriaContext.Set<T>().Select(t => t.Id).ToListAsync();
          resources.AddRange(allIds);
        }
      }

      var permissions = await permissionQuery.ToListAsync();

      resources.AddRange(permissions.Select(p => p.GetResourceId()).ToList());
      return resources;
    }

    public async Task<List<Guid>> GetAvailableResources<T>(Guid userId, string permission, string permissionNamespace, bool includeWildcard = false) where T : class, IBaseEntity
    {
      var resources = new List<Guid>();
      var resourceName = AuthorizationHelper.GetResourceName(typeof(T));
      if (resourceName == null)
      {
        return resources;
      }

      var permissionQuery = this.alexandriaContext.Permissions
        .Where(p => p.UserProfileId == userId)
        .Where(p =>
          Microsoft.EntityFrameworkCore.EF.Functions.Like(p.ARN,
            $"{permissionNamespace}::{resourceName}::%::{permission}") ||
          Microsoft.EntityFrameworkCore.EF.Functions.Like(p.ARN, $"{permissionNamespace}::{resourceName}::%::*"))
        .Where(p =>
          !Microsoft.EntityFrameworkCore.EF.Functions.Like(p.ARN,
            $"{permissionNamespace}::{resourceName}::*::{permission}") &&
          !Microsoft.EntityFrameworkCore.EF.Functions.Like(p.ARN, $"{permissionNamespace}::{resourceName}::*::*"));

      if (includeWildcard)
      {
        var hasWildcard = await this.alexandriaContext.Permissions.Where(p => p.UserProfileId == userId).Where(p =>
            Microsoft.EntityFrameworkCore.EF.Functions.Like(p.ARN, $"{permissionNamespace}::{resourceName}::*::*") ||
            Microsoft.EntityFrameworkCore.EF.Functions.Like(p.ARN,
              $"{permissionNamespace}::{resourceName}::*::{permission}"))
          .AnyAsync();

        if (hasWildcard)
        {
          var allIds = await this.alexandriaContext.Set<T>().Select(t => t.Id).ToListAsync();
          resources.AddRange(allIds);
        }
      }

      var permissions = await permissionQuery.ToListAsync();

      resources.AddRange(permissions.Select(p => p.GetResourceId()).ToList());
      return resources;
    }
  }
}
