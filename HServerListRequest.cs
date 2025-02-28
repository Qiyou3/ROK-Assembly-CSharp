// Decompiled with JetBrains decompiler
// Type: Steamworks.HServerListRequest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Steamworks
{
  public struct HServerListRequest(IntPtr value) : IEquatable<HServerListRequest>
  {
    public static readonly HServerListRequest Invalid = new HServerListRequest(IntPtr.Zero);
    public IntPtr m_HServerListRequest = value;

    public override string ToString() => this.m_HServerListRequest.ToString();

    public override bool Equals(object other)
    {
      return other is HServerListRequest hserverListRequest && this == hserverListRequest;
    }

    public override int GetHashCode() => this.m_HServerListRequest.GetHashCode();

    public bool Equals(HServerListRequest other)
    {
      return this.m_HServerListRequest == other.m_HServerListRequest;
    }

    public static bool operator ==(HServerListRequest x, HServerListRequest y)
    {
      return x.m_HServerListRequest == y.m_HServerListRequest;
    }

    public static bool operator !=(HServerListRequest x, HServerListRequest y) => !(x == y);

    public static explicit operator HServerListRequest(IntPtr value)
    {
      return new HServerListRequest(value);
    }

    public static explicit operator IntPtr(HServerListRequest that) => that.m_HServerListRequest;
  }
}
