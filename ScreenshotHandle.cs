// Decompiled with JetBrains decompiler
// Type: Steamworks.ScreenshotHandle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Steamworks
{
  public struct ScreenshotHandle(uint value) : 
    IEquatable<ScreenshotHandle>,
    IComparable<ScreenshotHandle>
  {
    public static readonly ScreenshotHandle Invalid = new ScreenshotHandle(0U);
    public uint m_ScreenshotHandle = value;

    public override string ToString() => this.m_ScreenshotHandle.ToString();

    public override bool Equals(object other)
    {
      return other is ScreenshotHandle screenshotHandle && this == screenshotHandle;
    }

    public override int GetHashCode() => this.m_ScreenshotHandle.GetHashCode();

    public bool Equals(ScreenshotHandle other)
    {
      return (int) this.m_ScreenshotHandle == (int) other.m_ScreenshotHandle;
    }

    public int CompareTo(ScreenshotHandle other)
    {
      return this.m_ScreenshotHandle.CompareTo(other.m_ScreenshotHandle);
    }

    public static bool operator ==(ScreenshotHandle x, ScreenshotHandle y)
    {
      return (int) x.m_ScreenshotHandle == (int) y.m_ScreenshotHandle;
    }

    public static bool operator !=(ScreenshotHandle x, ScreenshotHandle y) => !(x == y);

    public static explicit operator ScreenshotHandle(uint value) => new ScreenshotHandle(value);

    public static explicit operator uint(ScreenshotHandle that) => that.m_ScreenshotHandle;
  }
}
