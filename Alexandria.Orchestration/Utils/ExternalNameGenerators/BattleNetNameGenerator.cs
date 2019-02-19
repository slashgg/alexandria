using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alexandria.EF.Context;
using Alexandria.Interfaces.Utils;
using Microsoft.EntityFrameworkCore;

namespace Alexandria.Orchestration.Utils.ExternalNameGenerators
{
  public class BattleNetNameGenerator : IExternalUserNameGenerator
  {
    private readonly string ExternalUserNameServiceName = "Battle.net";
    private readonly AlexandriaContext alexandriaContext;

    public BattleNetNameGenerator(AlexandriaContext context)
    {
      this.alexandriaContext = context;
    }

    public async Task<DTO.UserProfile.ExternalUserName> Create(Guid userId)
    {
      var user = await this.alexandriaContext.UserProfiles.Include(up => up.ExternalAccounts).FirstOrDefaultAsync(up => up.Id == userId);

      var battleNet = user?.ExternalAccounts?.FirstOrDefault(fa => fa.Provider == Shared.Enums.ExternalProvider.BattleNet);
      if (battleNet == null)
      {
        return null;
      }

      var battleNetGenerator = await this.alexandriaContext.ExternalUserNameGenerators.FirstOrDefaultAsync(eung => eung.Name == this.ExternalUserNameServiceName);
      var externalUserName = new DTO.UserProfile.ExternalUserName(battleNet.Name, battleNetGenerator.LogoURL, this.ExternalUserNameServiceName);

      return externalUserName;
    }
  }
}
