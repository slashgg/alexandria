using System;
using System.Collections.Generic;
using System.Text;

namespace Alexandria.Shared.Permissions
{
  public static class Team
  {
    public static string All { get; } = "*";
    public static string SendInvite { get; } = "invite-send";
    public static string RevokeInvite { get; } = "invite-revoke";
    public static string RemoveMember { get; } = "member-remove";
    public static string PromoteMember { get; } = "member-promote";
  }
}
