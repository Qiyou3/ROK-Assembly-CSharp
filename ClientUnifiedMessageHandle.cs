// Decompiled with JetBrains decompiler
// Type: Steamworks.ClientUnifiedMessageHandle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Steamworks
{
  public struct ClientUnifiedMessageHandle(ulong value) : 
    IEquatable<ClientUnifiedMessageHandle>,
    IComparable<ClientUnifiedMessageHandle>
  {
    public static readonly ClientUnifiedMessageHandle Invalid = new ClientUnifiedMessageHandle(0UL);
    public ulong m_ClientUnifiedMessageHandle = value;

    public override string ToString() => this.m_ClientUnifiedMessageHandle.ToString();

    public override bool Equals(object other)
    {
      return other is ClientUnifiedMessageHandle unifiedMessageHandle && this == unifiedMessageHandle;
    }

    public override int GetHashCode() => this.m_ClientUnifiedMessageHandle.GetHashCode();

    public bool Equals(ClientUnifiedMessageHandle other)
    {
      return (long) this.m_ClientUnifiedMessageHandle == (long) other.m_ClientUnifiedMessageHandle;
    }

    public int CompareTo(ClientUnifiedMessageHandle other)
    {
      return this.m_ClientUnifiedMessageHandle.CompareTo(other.m_ClientUnifiedMessageHandle);
    }

    public static bool operator ==(ClientUnifiedMessageHandle x, ClientUnifiedMessageHandle y)
    {
      return (long) x.m_ClientUnifiedMessageHandle == (long) y.m_ClientUnifiedMessageHandle;
    }

    public static bool operator !=(ClientUnifiedMessageHandle x, ClientUnifiedMessageHandle y)
    {
      return !(x == y);
    }

    public static explicit operator ClientUnifiedMessageHandle(ulong value)
    {
      return new ClientUnifiedMessageHandle(value);
    }

    public static explicit operator ulong(ClientUnifiedMessageHandle that)
    {
      return that.m_ClientUnifiedMessageHandle;
    }
  }
}
