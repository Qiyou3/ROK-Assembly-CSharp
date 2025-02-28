// Decompiled with JetBrains decompiler
// Type: Steamworks.HServerQuery
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Steamworks
{
  public struct HServerQuery(int value) : IEquatable<HServerQuery>, IComparable<HServerQuery>
  {
    public static readonly HServerQuery Invalid = new HServerQuery(-1);
    public int m_HServerQuery = value;

    public override string ToString() => this.m_HServerQuery.ToString();

    public override bool Equals(object other)
    {
      return other is HServerQuery hserverQuery && this == hserverQuery;
    }

    public override int GetHashCode() => this.m_HServerQuery.GetHashCode();

    public bool Equals(HServerQuery other) => this.m_HServerQuery == other.m_HServerQuery;

    public int CompareTo(HServerQuery other) => this.m_HServerQuery.CompareTo(other.m_HServerQuery);

    public static bool operator ==(HServerQuery x, HServerQuery y)
    {
      return x.m_HServerQuery == y.m_HServerQuery;
    }

    public static bool operator !=(HServerQuery x, HServerQuery y) => !(x == y);

    public static explicit operator HServerQuery(int value) => new HServerQuery(value);

    public static explicit operator int(HServerQuery that) => that.m_HServerQuery;
  }
}
