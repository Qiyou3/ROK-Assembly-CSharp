// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamItemInstanceID_t
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Steamworks
{
  public struct SteamItemInstanceID_t(ulong value) : 
    IEquatable<SteamItemInstanceID_t>,
    IComparable<SteamItemInstanceID_t>
  {
    public static readonly SteamItemInstanceID_t Invalid = new SteamItemInstanceID_t(ulong.MaxValue);
    public ulong m_SteamItemInstanceID = value;

    public override string ToString() => this.m_SteamItemInstanceID.ToString();

    public override bool Equals(object other)
    {
      return other is SteamItemInstanceID_t steamItemInstanceIdT && this == steamItemInstanceIdT;
    }

    public override int GetHashCode() => this.m_SteamItemInstanceID.GetHashCode();

    public bool Equals(SteamItemInstanceID_t other)
    {
      return (long) this.m_SteamItemInstanceID == (long) other.m_SteamItemInstanceID;
    }

    public int CompareTo(SteamItemInstanceID_t other)
    {
      return this.m_SteamItemInstanceID.CompareTo(other.m_SteamItemInstanceID);
    }

    public static bool operator ==(SteamItemInstanceID_t x, SteamItemInstanceID_t y)
    {
      return (long) x.m_SteamItemInstanceID == (long) y.m_SteamItemInstanceID;
    }

    public static bool operator !=(SteamItemInstanceID_t x, SteamItemInstanceID_t y) => !(x == y);

    public static explicit operator SteamItemInstanceID_t(ulong value)
    {
      return new SteamItemInstanceID_t(value);
    }

    public static explicit operator ulong(SteamItemInstanceID_t that) => that.m_SteamItemInstanceID;
  }
}
