// Decompiled with JetBrains decompiler
// Type: Steamworks.AccountID_t
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Steamworks
{
  public struct AccountID_t(uint value) : IEquatable<AccountID_t>, IComparable<AccountID_t>
  {
    public uint m_AccountID = value;

    public override string ToString() => this.m_AccountID.ToString();

    public override bool Equals(object other)
    {
      return other is AccountID_t accountIdT && this == accountIdT;
    }

    public override int GetHashCode() => this.m_AccountID.GetHashCode();

    public bool Equals(AccountID_t other) => (int) this.m_AccountID == (int) other.m_AccountID;

    public int CompareTo(AccountID_t other) => this.m_AccountID.CompareTo(other.m_AccountID);

    public static bool operator ==(AccountID_t x, AccountID_t y)
    {
      return (int) x.m_AccountID == (int) y.m_AccountID;
    }

    public static bool operator !=(AccountID_t x, AccountID_t y) => !(x == y);

    public static explicit operator AccountID_t(uint value) => new AccountID_t(value);

    public static explicit operator uint(AccountID_t that) => that.m_AccountID;
  }
}
