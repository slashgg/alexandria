using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alexandria.EF.Context;
using Alexandria.EF.Models;
using Alexandria.Interfaces;
using Alexandria.Interfaces.Utils;
using Microsoft.EntityFrameworkCore;

namespace Alexandria.Orchestration.Utils
{
  public class UserUtils : IUserUtils
  {
    private readonly AlexandriaContext context;

    public UserUtils(AlexandriaContext context)
    {
      this.context = context;
    }

    public async Task<string> GetEmail(Guid userId)
    {
      var email = await this.context.UserProfiles.Where(u => u.Id == userId).Select(u => u.Email).FirstOrDefaultAsync();
      return email;
    }

    public bool GetEmail(Guid userId, out string email)
    {
      email = this.context.UserProfiles.Where(u => u.Id == userId).Select(u => u.Email).FirstOrDefault();
      return email != null;
    }

    public async Task<Guid?> GetIdFromName(string userName)
    {
      var userId = await this.context.UserProfiles.Where(u => u.UserName == userName).Select(u => u.Id).FirstOrDefaultAsync();
      return userId;
    }
    public async Task<Guid?> GetIdFromEmail(string email)
    {
      var userId = await this.context.UserProfiles.Where(u => u.Email == email).Select(u => u.Id).FirstOrDefaultAsync();
      return userId;
    }

    public bool GetIdFromEmail(string email, out Guid userId)
    {
      userId = this.context.UserProfiles.Where(u => u.Email == email).Select(u => u.Id).FirstOrDefault();
      return userId != null && userId != Guid.Empty;
    }

    public async Task<string> GetEmailFromUserName(string userName)
    {
      var email = await this.context.UserProfiles.Where(u => u.UserName == userName).Select(u => u.Email).FirstOrDefaultAsync();
      return email;
    }

    public async Task GenerateExternalUserName(Guid userProfileId, Guid gameId)
    {
      var user = await this.context.UserProfiles.Include(up => up.ExternalUserNames).FirstOrDefaultAsync(up => up.Id == userProfileId);
      if (user.HasExternalUserName(gameId))
      {
        return;
      }

      var game = await this.context.Games.Include(g => g.GameExternalUserNameGenerator).ThenInclude(geung => geung.ExternalUserNameGenerator).FirstOrDefaultAsync(g => g.Id == gameId);
      var generator = game.GameExternalUserNameGenerator?.ExternalUserNameGenerator;
      if (generator == null)
      {
        return;
      }

      try
      {
        var userNameGenerator = Activator.CreateInstance(Type.GetType(generator.Type), this.context) as IExternalUserNameGenerator;
        var userName = await userNameGenerator.Create(userProfileId);
        var externalUserName = new EF.Models.ExternalUserName(userName.UserName, userName.LogoURL, userName.ServiceName, gameId);
        user.ExternalUserNames.Add(externalUserName);
        this.context.UserProfiles.Update(user);
      }
      catch (Exception ex)
      {
        Sentry.SentrySdk.CaptureException(ex);
      }
    }
  }
}
