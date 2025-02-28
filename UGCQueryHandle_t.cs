﻿// Decompiled with JetBrains decompiler
// Type: Steamworks.UGCQueryHandle_t
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Steamworks
{
  public struct UGCQueryHandle_t(ulong value) : 
    IEquatable<UGCQueryHandle_t>,
    IComparable<UGCQueryHandle_t>
  {
    public static readonly UGCQueryHandle_t Invalid = new UGCQueryHandle_t(ulong.MaxValue);
    public ulong m_UGCQueryHandle = value;

    public override string ToString() => this.m_UGCQueryHandle.ToString();

    public override bool Equals(object other)
    {
      return other is UGCQueryHandle_t ugcQueryHandleT && this == ugcQueryHandleT;
    }

    public override int GetHashCode() => this.m_UGCQueryHandle.GetHashCode();

    public bool Equals(UGCQueryHandle_t other)
    {
      return (long) this.m_UGCQueryHandle == (long) other.m_UGCQueryHandle;
    }

    public int CompareTo(UGCQueryHandle_t other)
    {
      return this.m_UGCQueryHandle.CompareTo(other.m_UGCQueryHandle);
    }

    public static bool operator ==(UGCQueryHandle_t x, UGCQueryHandle_t y)
    {
      return (long) x.m_UGCQueryHandle == (long) y.m_UGCQueryHandle;
    }

    public static bool operator !=(UGCQueryHandle_t x, UGCQueryHandle_t y) => !(x == y);

    public static explicit operator UGCQueryHandle_t(ulong value) => new UGCQueryHandle_t(value);

    public static explicit operator ulong(UGCQueryHandle_t that) => that.m_UGCQueryHandle;
  }
}
