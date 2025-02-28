// Decompiled with JetBrains decompiler
// Type: Steamworks.HSteamPipe
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Steamworks
{
  public struct HSteamPipe(int value) : IEquatable<HSteamPipe>, IComparable<HSteamPipe>
  {
    public int m_HSteamPipe = value;

    public override string ToString() => this.m_HSteamPipe.ToString();

    public override bool Equals(object other)
    {
      return other is HSteamPipe hsteamPipe && this == hsteamPipe;
    }

    public override int GetHashCode() => this.m_HSteamPipe.GetHashCode();

    public bool Equals(HSteamPipe other) => this.m_HSteamPipe == other.m_HSteamPipe;

    public int CompareTo(HSteamPipe other) => this.m_HSteamPipe.CompareTo(other.m_HSteamPipe);

    public static bool operator ==(HSteamPipe x, HSteamPipe y) => x.m_HSteamPipe == y.m_HSteamPipe;

    public static bool operator !=(HSteamPipe x, HSteamPipe y) => !(x == y);

    public static explicit operator HSteamPipe(int value) => new HSteamPipe(value);

    public static explicit operator int(HSteamPipe that) => that.m_HSteamPipe;
  }
}
