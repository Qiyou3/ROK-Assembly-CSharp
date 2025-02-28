// Decompiled with JetBrains decompiler
// Type: Steamworks.HHTMLBrowser
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Steamworks
{
  public struct HHTMLBrowser(uint value) : IEquatable<HHTMLBrowser>, IComparable<HHTMLBrowser>
  {
    public static readonly HHTMLBrowser Invalid = new HHTMLBrowser(0U);
    public uint m_HHTMLBrowser = value;

    public override string ToString() => this.m_HHTMLBrowser.ToString();

    public override bool Equals(object other)
    {
      return other is HHTMLBrowser hhtmlBrowser && this == hhtmlBrowser;
    }

    public override int GetHashCode() => this.m_HHTMLBrowser.GetHashCode();

    public bool Equals(HHTMLBrowser other)
    {
      return (int) this.m_HHTMLBrowser == (int) other.m_HHTMLBrowser;
    }

    public int CompareTo(HHTMLBrowser other) => this.m_HHTMLBrowser.CompareTo(other.m_HHTMLBrowser);

    public static bool operator ==(HHTMLBrowser x, HHTMLBrowser y)
    {
      return (int) x.m_HHTMLBrowser == (int) y.m_HHTMLBrowser;
    }

    public static bool operator !=(HHTMLBrowser x, HHTMLBrowser y) => !(x == y);

    public static explicit operator HHTMLBrowser(uint value) => new HHTMLBrowser(value);

    public static explicit operator uint(HHTMLBrowser that) => that.m_HHTMLBrowser;
  }
}
