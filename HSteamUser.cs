// Decompiled with JetBrains decompiler
// Type: Steamworks.HSteamUser
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Steamworks
{
  public struct HSteamUser(int value) : IEquatable<HSteamUser>, IComparable<HSteamUser>
  {
    public int m_HSteamUser = value;

    public override string ToString() => this.m_HSteamUser.ToString();

    public override bool Equals(object other)
    {
      return other is HSteamUser hsteamUser && this == hsteamUser;
    }

    public override int GetHashCode() => this.m_HSteamUser.GetHashCode();

    public bool Equals(HSteamUser other) => this.m_HSteamUser == other.m_HSteamUser;

    public int CompareTo(HSteamUser other) => this.m_HSteamUser.CompareTo(other.m_HSteamUser);

    public static bool operator ==(HSteamUser x, HSteamUser y) => x.m_HSteamUser == y.m_HSteamUser;

    public static bool operator !=(HSteamUser x, HSteamUser y) => !(x == y);

    public static explicit operator HSteamUser(int value) => new HSteamUser(value);

    public static explicit operator int(HSteamUser that) => that.m_HSteamUser;
  }
}
