﻿// Decompiled with JetBrains decompiler
// Type: Steamworks.ManifestId_t
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Steamworks
{
  public struct ManifestId_t(ulong value) : IEquatable<ManifestId_t>, IComparable<ManifestId_t>
  {
    public static readonly ManifestId_t Invalid = new ManifestId_t(0UL);
    public ulong m_ManifestId = value;

    public override string ToString() => this.m_ManifestId.ToString();

    public override bool Equals(object other)
    {
      return other is ManifestId_t manifestIdT && this == manifestIdT;
    }

    public override int GetHashCode() => this.m_ManifestId.GetHashCode();

    public bool Equals(ManifestId_t other) => (long) this.m_ManifestId == (long) other.m_ManifestId;

    public int CompareTo(ManifestId_t other) => this.m_ManifestId.CompareTo(other.m_ManifestId);

    public static bool operator ==(ManifestId_t x, ManifestId_t y)
    {
      return (long) x.m_ManifestId == (long) y.m_ManifestId;
    }

    public static bool operator !=(ManifestId_t x, ManifestId_t y) => !(x == y);

    public static explicit operator ManifestId_t(ulong value) => new ManifestId_t(value);

    public static explicit operator ulong(ManifestId_t that) => that.m_ManifestId;
  }
}
