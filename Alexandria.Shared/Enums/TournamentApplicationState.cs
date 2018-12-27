using System.Runtime.Serialization;

namespace Alexandria.Shared.Enums
{
  public enum TournamentApplicationState
  {
    [EnumMember(Value ="pending")]
    Pending = 1,
    [EnumMember(Value = "accepted")]
    Accepted = 2,
    [EnumMember(Value = "declined")]
    Declined = 3,
    [EnumMember(Value = "withdrawn")]
    Withdrawn = 4
  }
}
