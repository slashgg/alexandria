using System;
using System.Collections.Generic;
using System.Text;
using Alexandria.EF.Models;
using GraphQL.Types;

namespace Alexandria.GQL.Competition.Types
{
  public class TeamType : ObjectGraphType<Team>
  {
    public TeamType()
    {
      Field(t => t.Id);
      Field(t => t.Name);
      Field(t => t.Abbreviation);
      Field(t => t.LogoURL, true);
      Field(t => t.Slug);
      Field(t => t.TeamState);
    }
  }
}
