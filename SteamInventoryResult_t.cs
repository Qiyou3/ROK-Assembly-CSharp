// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamInventoryResult_t
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Steamworks
{
  public struct SteamInventoryResult_t(int value) : 
    IEquatable<SteamInventoryResult_t>,
    IComparable<SteamInventoryResult_t>
  {
    public static readonly SteamInventoryResult_t Invalid = new SteamInventoryResult_t(-1);
    public int m_SteamInventoryResult = value;

    public override string ToString() => this.m_SteamInventoryResult.ToString();

    public override bool Equals(object other)
    {
      return other is SteamInventoryResult_t inventoryResultT && this == inventoryResultT;
    }

    public override int GetHashCode() => this.m_SteamInventoryResult.GetHashCode();

    public bool Equals(SteamInventoryResult_t other)
    {
      return this.m_SteamInventoryResult == other.m_SteamInventoryResult;
    }

    public int CompareTo(SteamInventoryResult_t other)
    {
      return this.m_SteamInventoryResult.CompareTo(other.m_SteamInventoryResult);
    }

    public static bool operator ==(SteamInventoryResult_t x, SteamInventoryResult_t y)
    {
      return x.m_SteamInventoryResult == y.m_SteamInventoryResult;
    }

    public static bool operator !=(SteamInventoryResult_t x, SteamInventoryResult_t y) => !(x == y);

    public static explicit operator SteamInventoryResult_t(int value)
    {
      return new SteamInventoryResult_t(value);
    }

    public static explicit operator int(SteamInventoryResult_t that) => that.m_SteamInventoryResult;
  }
}
