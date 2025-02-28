// Decompiled with JetBrains decompiler
// Type: Steamworks.SNetListenSocket_t
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Steamworks
{
  public struct SNetListenSocket_t(uint value) : 
    IEquatable<SNetListenSocket_t>,
    IComparable<SNetListenSocket_t>
  {
    public uint m_SNetListenSocket = value;

    public override string ToString() => this.m_SNetListenSocket.ToString();

    public override bool Equals(object other)
    {
      return other is SNetListenSocket_t snetListenSocketT && this == snetListenSocketT;
    }

    public override int GetHashCode() => this.m_SNetListenSocket.GetHashCode();

    public bool Equals(SNetListenSocket_t other)
    {
      return (int) this.m_SNetListenSocket == (int) other.m_SNetListenSocket;
    }

    public int CompareTo(SNetListenSocket_t other)
    {
      return this.m_SNetListenSocket.CompareTo(other.m_SNetListenSocket);
    }

    public static bool operator ==(SNetListenSocket_t x, SNetListenSocket_t y)
    {
      return (int) x.m_SNetListenSocket == (int) y.m_SNetListenSocket;
    }

    public static bool operator !=(SNetListenSocket_t x, SNetListenSocket_t y) => !(x == y);

    public static explicit operator SNetListenSocket_t(uint value) => new SNetListenSocket_t(value);

    public static explicit operator uint(SNetListenSocket_t that) => that.m_SNetListenSocket;
  }
}
