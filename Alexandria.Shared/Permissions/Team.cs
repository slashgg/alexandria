using System.Runtime.Serialization;

namespace Alexandria.Shared.Permissions
{
  public static class Team
  {
    public const string All = "*";
    public const string ScheduleMatch = "match--schedule";
    public const string MatchParticipation = "match--participation";
    public const string SendInvite = "invite--send";
    public const string RevokeInvite = "invite--revoke";
    public const string RemoveMember = "member--remove";
    public const string MemberSetRole = "member--set-role";
    public const string JoinTournament = "tournament--join";
  }

  public enum TeamPermission
  {
    [EnumMember(Value = "*")]
    All,
    [EnumMember(Value = "disband")]
    Disband,
    [EnumMember(Value = "match--schedule")]
    ScheduleMatch,
    [EnumMember(Value = "match--participation")]
    MatchParticipation,
    [EnumMember(Value = "invite--send")]
    InviteSend,
    [EnumMember(Value = "invite--revoke")]
    InviteRevoke,
    [EnumMember(Value = "member--remove")]
    MemberRemove,
    [EnumMember(Value = "member--set-role")]
    MemberSetRole,
    [EnumMember(Value = "tournament--join")]
    JoinTournament,
  }
}
