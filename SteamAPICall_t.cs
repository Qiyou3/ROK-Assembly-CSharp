// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamAPICall_t
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Steamworks
{
  public struct SteamAPICall_t(ulong value) : IEquatable<SteamAPICall_t>, IComparable<SteamAPICall_t>
  {
    public static readonly SteamAPICall_t Invalid = new SteamAPICall_t(0UL);
    public ulong m_SteamAPICall = value;

    public override string ToString() => this.m_SteamAPICall.ToString();

    public override bool Equals(object other)
    {
      return other is SteamAPICall_t steamApiCallT && this == steamApiCallT;
    }

    public override int GetHashCode() => this.m_SteamAPICall.GetHashCode();

    public bool Equals(SteamAPICall_t other)
    {
      return (long) this.m_SteamAPICall == (long) other.m_SteamAPICall;
    }

    public int CompareTo(SteamAPICall_t other)
    {
      return this.m_SteamAPICall.CompareTo(other.m_SteamAPICall);
    }

    public static bool operator ==(SteamAPICall_t x, SteamAPICall_t y)
    {
      return (long) x.m_SteamAPICall == (long) y.m_SteamAPICall;
    }

    public static bool operator !=(SteamAPICall_t x, SteamAPICall_t y) => !(x == y);

    public static explicit operator SteamAPICall_t(ulong value) => new SteamAPICall_t(value);

    public static explicit operator ulong(SteamAPICall_t that) => that.m_SteamAPICall;
  }
}
