using System.Runtime.Serialization;

namespace Alexandria.Shared.Enums
{
  public enum ScheduleRequestState
  {
    [EnumMember(Value = "pending")]
    Pending = 1,
    [EnumMember(Value = "accepted")]
    Accepted = 2,
    [EnumMember(Value = "decliend")]
    Declined = 3,
    [EnumMember(Value = "rescinded")]
    Rescinded = 4,
  }
}
