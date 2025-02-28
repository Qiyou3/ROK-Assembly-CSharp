﻿// Decompiled with JetBrains decompiler
// Type: Steamworks.FriendsGroupID_t
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Steamworks
{
  public struct FriendsGroupID_t(short value) : 
    IEquatable<FriendsGroupID_t>,
    IComparable<FriendsGroupID_t>
  {
    public static readonly FriendsGroupID_t Invalid = new FriendsGroupID_t((short) -1);
    public short m_FriendsGroupID = value;

    public override string ToString() => this.m_FriendsGroupID.ToString();

    public override bool Equals(object other)
    {
      return other is FriendsGroupID_t friendsGroupIdT && this == friendsGroupIdT;
    }

    public override int GetHashCode() => this.m_FriendsGroupID.GetHashCode();

    public bool Equals(FriendsGroupID_t other)
    {
      return (int) this.m_FriendsGroupID == (int) other.m_FriendsGroupID;
    }

    public int CompareTo(FriendsGroupID_t other)
    {
      return this.m_FriendsGroupID.CompareTo(other.m_FriendsGroupID);
    }

    public static bool operator ==(FriendsGroupID_t x, FriendsGroupID_t y)
    {
      return (int) x.m_FriendsGroupID == (int) y.m_FriendsGroupID;
    }

    public static bool operator !=(FriendsGroupID_t x, FriendsGroupID_t y) => !(x == y);

    public static explicit operator FriendsGroupID_t(short value) => new FriendsGroupID_t(value);

    public static explicit operator short(FriendsGroupID_t that) => that.m_FriendsGroupID;
  }
}
