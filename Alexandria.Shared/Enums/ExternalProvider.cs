using System.Runtime.Serialization;

namespace Alexandria.Shared.Enums
{
  public enum ExternalProvider
  {
    [EnumMember(Value = "discord")]
    Discord = 1,
    [EnumMember(Value = "twitch")]
    Twitch = 3,
    [EnumMember(Value = "battle.net")]
    BattleNet = 2,
  }
}
