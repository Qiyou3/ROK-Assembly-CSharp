// Decompiled with JetBrains decompiler
// Type: Steamworks.HAuthTicket
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Steamworks
{
  public struct HAuthTicket(uint value) : IEquatable<HAuthTicket>, IComparable<HAuthTicket>
  {
    public static readonly HAuthTicket Invalid = new HAuthTicket(0U);
    public uint m_HAuthTicket = value;

    public override string ToString() => this.m_HAuthTicket.ToString();

    public override bool Equals(object other)
    {
      return other is HAuthTicket hauthTicket && this == hauthTicket;
    }

    public override int GetHashCode() => this.m_HAuthTicket.GetHashCode();

    public bool Equals(HAuthTicket other) => (int) this.m_HAuthTicket == (int) other.m_HAuthTicket;

    public int CompareTo(HAuthTicket other) => this.m_HAuthTicket.CompareTo(other.m_HAuthTicket);

    public static bool operator ==(HAuthTicket x, HAuthTicket y)
    {
      return (int) x.m_HAuthTicket == (int) y.m_HAuthTicket;
    }

    public static bool operator !=(HAuthTicket x, HAuthTicket y) => !(x == y);

    public static explicit operator HAuthTicket(uint value) => new HAuthTicket(value);

    public static explicit operator uint(HAuthTicket that) => that.m_HAuthTicket;
  }
}
