// Decompiled with JetBrains decompiler
// Type: Steamworks.SNetSocket_t
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Steamworks
{
  public struct SNetSocket_t(uint value) : IEquatable<SNetSocket_t>, IComparable<SNetSocket_t>
  {
    public uint m_SNetSocket = value;

    public override string ToString() => this.m_SNetSocket.ToString();

    public override bool Equals(object other)
    {
      return other is SNetSocket_t snetSocketT && this == snetSocketT;
    }

    public override int GetHashCode() => this.m_SNetSocket.GetHashCode();

    public bool Equals(SNetSocket_t other) => (int) this.m_SNetSocket == (int) other.m_SNetSocket;

    public int CompareTo(SNetSocket_t other) => this.m_SNetSocket.CompareTo(other.m_SNetSocket);

    public static bool operator ==(SNetSocket_t x, SNetSocket_t y)
    {
      return (int) x.m_SNetSocket == (int) y.m_SNetSocket;
    }

    public static bool operator !=(SNetSocket_t x, SNetSocket_t y) => !(x == y);

    public static explicit operator SNetSocket_t(uint value) => new SNetSocket_t(value);

    public static explicit operator uint(SNetSocket_t that) => that.m_SNetSocket;
  }
}
