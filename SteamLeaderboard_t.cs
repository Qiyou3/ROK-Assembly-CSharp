// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamLeaderboard_t
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Steamworks
{
  public struct SteamLeaderboard_t(ulong value) : 
    IEquatable<SteamLeaderboard_t>,
    IComparable<SteamLeaderboard_t>
  {
    public ulong m_SteamLeaderboard = value;

    public override string ToString() => this.m_SteamLeaderboard.ToString();

    public override bool Equals(object other)
    {
      return other is SteamLeaderboard_t steamLeaderboardT && this == steamLeaderboardT;
    }

    public override int GetHashCode() => this.m_SteamLeaderboard.GetHashCode();

    public bool Equals(SteamLeaderboard_t other)
    {
      return (long) this.m_SteamLeaderboard == (long) other.m_SteamLeaderboard;
    }

    public int CompareTo(SteamLeaderboard_t other)
    {
      return this.m_SteamLeaderboard.CompareTo(other.m_SteamLeaderboard);
    }

    public static bool operator ==(SteamLeaderboard_t x, SteamLeaderboard_t y)
    {
      return (long) x.m_SteamLeaderboard == (long) y.m_SteamLeaderboard;
    }

    public static bool operator !=(SteamLeaderboard_t x, SteamLeaderboard_t y) => !(x == y);

    public static explicit operator SteamLeaderboard_t(ulong value)
    {
      return new SteamLeaderboard_t(value);
    }

    public static explicit operator ulong(SteamLeaderboard_t that) => that.m_SteamLeaderboard;
  }
}
