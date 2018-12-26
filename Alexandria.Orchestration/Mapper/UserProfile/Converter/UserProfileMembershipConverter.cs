using System;
using System.Collections.Generic;
using AutoMapper;

namespace Alexandria.Orchestration.Mapper.UserProfile.Converter
{
  public class UserProfileMembershipConverter : IValueConverter<IEnumerable<EF.Models.TeamMembership>, Dictionary<Guid, DTO.Team.Membership>>
  {
    public Dictionary<Guid, DTO.Team.Membership> Convert(IEnumerable<EF.Models.TeamMembership> sourceMember, ResolutionContext context)
    {
      var memberships = new Dictionary<Guid, DTO.Team.Membership>();

      foreach (var membership in sourceMember)
      {
        var dtoMembership = context.Mapper.Map<DTO.Team.Membership>(membership);
        memberships.Add(membership.Team.CompetitionId, dtoMembership);
      }

      return memberships;
    }
  }
}
