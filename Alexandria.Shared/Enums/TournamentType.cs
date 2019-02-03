using System.Runtime.Serialization;

namespace Alexandria.Shared.Enums
{
  public enum TournamentType
  {
    [EnumMember(Value = "round-robin")]
    RoundRobin = 1,
    [EnumMember(Value = "double-round-robin")]
    DoubleRoundRobin = 2,
    [EnumMember(Value = "single-elimination")]
    SingleElimination = 2,
    [EnumMember(Value = "double-elimination")]
    DoubleElimination = 3,
    [EnumMember(Value = "swiss")]
    Swiss = 4,
  }
}
