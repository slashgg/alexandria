using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alexandria.EF.Context;
using Alexandria.EF.Models;
using Alexandria.Interfaces;
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
      return email == null;
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
      return userId == null;
    }

    public async Task<string> GetEmailFromUserName(string userName)
    {
      var email = await this.context.UserProfiles.Where(u => u.UserName == userName).Select(u => u.Email).FirstOrDefaultAsync();
      return email;
    }
  }
}
