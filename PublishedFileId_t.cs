// Decompiled with JetBrains decompiler
// Type: Steamworks.PublishedFileId_t
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Steamworks
{
  public struct PublishedFileId_t(ulong value) : 
    IEquatable<PublishedFileId_t>,
    IComparable<PublishedFileId_t>
  {
    public static readonly PublishedFileId_t Invalid = new PublishedFileId_t(0UL);
    public ulong m_PublishedFileId = value;

    public override string ToString() => this.m_PublishedFileId.ToString();

    public override bool Equals(object other)
    {
      return other is PublishedFileId_t publishedFileIdT && this == publishedFileIdT;
    }

    public override int GetHashCode() => this.m_PublishedFileId.GetHashCode();

    public bool Equals(PublishedFileId_t other)
    {
      return (long) this.m_PublishedFileId == (long) other.m_PublishedFileId;
    }

    public int CompareTo(PublishedFileId_t other)
    {
      return this.m_PublishedFileId.CompareTo(other.m_PublishedFileId);
    }

    public static bool operator ==(PublishedFileId_t x, PublishedFileId_t y)
    {
      return (long) x.m_PublishedFileId == (long) y.m_PublishedFileId;
    }

    public static bool operator !=(PublishedFileId_t x, PublishedFileId_t y) => !(x == y);

    public static explicit operator PublishedFileId_t(ulong value) => new PublishedFileId_t(value);

    public static explicit operator ulong(PublishedFileId_t that) => that.m_PublishedFileId;
  }
}
