using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;

namespace Alexandria.Orchestration.Mapper.Marketing.Converters
{
  public class ContactConverter : ITypeConverter<EF.Models.UserProfile, DTO.Marketing.Contact>
  {
    public DTO.Marketing.Contact Convert(EF.Models.UserProfile source, DTO.Marketing.Contact destination, ResolutionContext context)
    {
      if (source == null)
      {
        return null;
      }

      var contact = new DTO.Marketing.Contact();
      contact.Email = source.Email;
      contact.UserName = source.DisplayName;
      contact.Competitions = source.TeamMemberships.Select(m => m.Team).Select(t => t.Competition.Slug).ToList();
      contact.TeamLeader = System.Convert.ToInt32(source.TeamMemberships.Any(m => m.IsLeader()));

      return contact;
    }
  }
}
