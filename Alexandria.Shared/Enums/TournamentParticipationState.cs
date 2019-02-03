using System.Runtime.Serialization;

namespace Alexandria.Shared.Enums
{
  public enum TournamentParticipationState
  {
    [EnumMember(Value = "participating")]
    Participating = 1,
    [EnumMember(Value = "withdrawn")]
    Withdrawn = 2,
    [EnumMember(Value = "disqualified")]
    Disqualified = 3,
  }
}
