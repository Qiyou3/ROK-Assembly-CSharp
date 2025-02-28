// Decompiled with JetBrains decompiler
// Type: Steamworks.PublishedFileUpdateHandle_t
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Steamworks
{
  public struct PublishedFileUpdateHandle_t(ulong value) : 
    IEquatable<PublishedFileUpdateHandle_t>,
    IComparable<PublishedFileUpdateHandle_t>
  {
    public static readonly PublishedFileUpdateHandle_t Invalid = new PublishedFileUpdateHandle_t(ulong.MaxValue);
    public ulong m_PublishedFileUpdateHandle = value;

    public override string ToString() => this.m_PublishedFileUpdateHandle.ToString();

    public override bool Equals(object other)
    {
      return other is PublishedFileUpdateHandle_t fileUpdateHandleT && this == fileUpdateHandleT;
    }

    public override int GetHashCode() => this.m_PublishedFileUpdateHandle.GetHashCode();

    public bool Equals(PublishedFileUpdateHandle_t other)
    {
      return (long) this.m_PublishedFileUpdateHandle == (long) other.m_PublishedFileUpdateHandle;
    }

    public int CompareTo(PublishedFileUpdateHandle_t other)
    {
      return this.m_PublishedFileUpdateHandle.CompareTo(other.m_PublishedFileUpdateHandle);
    }

    public static bool operator ==(PublishedFileUpdateHandle_t x, PublishedFileUpdateHandle_t y)
    {
      return (long) x.m_PublishedFileUpdateHandle == (long) y.m_PublishedFileUpdateHandle;
    }

    public static bool operator !=(PublishedFileUpdateHandle_t x, PublishedFileUpdateHandle_t y)
    {
      return !(x == y);
    }

    public static explicit operator PublishedFileUpdateHandle_t(ulong value)
    {
      return new PublishedFileUpdateHandle_t(value);
    }

    public static explicit operator ulong(PublishedFileUpdateHandle_t that)
    {
      return that.m_PublishedFileUpdateHandle;
    }
  }
}
