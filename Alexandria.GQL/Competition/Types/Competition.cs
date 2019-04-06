using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Language.AST;
using GraphQL.Types;

namespace Alexandria.GQL.Competition.Types
{
  public class CompetitionType : ObjectGraphType<EF.Models.Competition>
  {
    public CompetitionType()
    {
      Field(c => c.Name);
      Field(c => c.Title);
      Field(c => c.Slug);
      Field(c => c.Description);
      Field<TeamType>(nameof(EF.Models.Competition.Teams));
    }
  }
}
