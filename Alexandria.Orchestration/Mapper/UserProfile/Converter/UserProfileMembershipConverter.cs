using System;
using System.Collections.Generic;
using AutoMapper;

namespace Alexandria.Orchestration.Mapper.UserProfile.Converter
{
  public class UserProfileMembershipConverter : IValueConverter<IEnumerable<EF.Models.TeamMembership>, Dictionary<Guid, DTO.UserProfile.TeamMembership>>
  {
    public Dictionary<Guid, DTO.UserProfile.TeamMembership> Convert(IEnumerable<EF.Models.TeamMembership> sourceMember, ResolutionContext context)
    {
      var memberships = new Dictionary<Guid, DTO.UserProfile.TeamMembership>();

      foreach (var membership in sourceMember)
      {
        var dtoMembership = context.Mapper.Map<DTO.UserProfile.TeamMembership>(membership);
        memberships.Add(membership.Team.CompetitionId, dtoMembership);
      }

      return memberships;
    }
  }
}
