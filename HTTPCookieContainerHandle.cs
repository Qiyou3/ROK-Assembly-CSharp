// Decompiled with JetBrains decompiler
// Type: Steamworks.HTTPCookieContainerHandle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Steamworks
{
  public struct HTTPCookieContainerHandle(uint value) : 
    IEquatable<HTTPCookieContainerHandle>,
    IComparable<HTTPCookieContainerHandle>
  {
    public static readonly HTTPCookieContainerHandle Invalid = new HTTPCookieContainerHandle(0U);
    public uint m_HTTPCookieContainerHandle = value;

    public override string ToString() => this.m_HTTPCookieContainerHandle.ToString();

    public override bool Equals(object other)
    {
      return other is HTTPCookieContainerHandle cookieContainerHandle && this == cookieContainerHandle;
    }

    public override int GetHashCode() => this.m_HTTPCookieContainerHandle.GetHashCode();

    public bool Equals(HTTPCookieContainerHandle other)
    {
      return (int) this.m_HTTPCookieContainerHandle == (int) other.m_HTTPCookieContainerHandle;
    }

    public int CompareTo(HTTPCookieContainerHandle other)
    {
      return this.m_HTTPCookieContainerHandle.CompareTo(other.m_HTTPCookieContainerHandle);
    }

    public static bool operator ==(HTTPCookieContainerHandle x, HTTPCookieContainerHandle y)
    {
      return (int) x.m_HTTPCookieContainerHandle == (int) y.m_HTTPCookieContainerHandle;
    }

    public static bool operator !=(HTTPCookieContainerHandle x, HTTPCookieContainerHandle y)
    {
      return !(x == y);
    }

    public static explicit operator HTTPCookieContainerHandle(uint value)
    {
      return new HTTPCookieContainerHandle(value);
    }

    public static explicit operator uint(HTTPCookieContainerHandle that)
    {
      return that.m_HTTPCookieContainerHandle;
    }
  }
}
