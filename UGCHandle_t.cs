// Decompiled with JetBrains decompiler
// Type: Steamworks.UGCHandle_t
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Steamworks
{
  public struct UGCHandle_t(ulong value) : IEquatable<UGCHandle_t>, IComparable<UGCHandle_t>
  {
    public static readonly UGCHandle_t Invalid = new UGCHandle_t(ulong.MaxValue);
    public ulong m_UGCHandle = value;

    public override string ToString() => this.m_UGCHandle.ToString();

    public override bool Equals(object other)
    {
      return other is UGCHandle_t ugcHandleT && this == ugcHandleT;
    }

    public override int GetHashCode() => this.m_UGCHandle.GetHashCode();

    public bool Equals(UGCHandle_t other) => (long) this.m_UGCHandle == (long) other.m_UGCHandle;

    public int CompareTo(UGCHandle_t other) => this.m_UGCHandle.CompareTo(other.m_UGCHandle);

    public static bool operator ==(UGCHandle_t x, UGCHandle_t y)
    {
      return (long) x.m_UGCHandle == (long) y.m_UGCHandle;
    }

    public static bool operator !=(UGCHandle_t x, UGCHandle_t y) => !(x == y);

    public static explicit operator UGCHandle_t(ulong value) => new UGCHandle_t(value);

    public static explicit operator ulong(UGCHandle_t that) => that.m_UGCHandle;
  }
}
