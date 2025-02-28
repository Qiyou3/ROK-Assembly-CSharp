﻿// Decompiled with JetBrains decompiler
// Type: Steamworks.CGameID
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Steamworks
{
  public struct CGameID : IEquatable<CGameID>, IComparable<CGameID>
  {
    public ulong m_GameID;

    public CGameID(ulong GameID) => this.m_GameID = GameID;

    public CGameID(AppId_t nAppID)
    {
      this.m_GameID = 0UL;
      this.SetAppID(nAppID);
    }

    public CGameID(AppId_t nAppID, uint nModID)
    {
      this.m_GameID = 0UL;
      this.SetAppID(nAppID);
      this.SetType(CGameID.EGameIDType.k_EGameIDTypeGameMod);
      this.SetModID(nModID);
    }

    public bool IsSteamApp() => this.Type() == CGameID.EGameIDType.k_EGameIDTypeApp;

    public bool IsMod() => this.Type() == CGameID.EGameIDType.k_EGameIDTypeGameMod;

    public bool IsShortcut() => this.Type() == CGameID.EGameIDType.k_EGameIDTypeShortcut;

    public bool IsP2PFile() => this.Type() == CGameID.EGameIDType.k_EGameIDTypeP2P;

    public AppId_t AppID() => new AppId_t((uint) (this.m_GameID & 16777215UL));

    public CGameID.EGameIDType Type()
    {
      return (CGameID.EGameIDType) ((long) (this.m_GameID >> 24) & (long) byte.MaxValue);
    }

    public uint ModID() => (uint) (this.m_GameID >> 32 & (ulong) uint.MaxValue);

    public bool IsValid()
    {
      switch (this.Type())
      {
        case CGameID.EGameIDType.k_EGameIDTypeApp:
          return this.AppID() != AppId_t.Invalid;
        case CGameID.EGameIDType.k_EGameIDTypeGameMod:
          return this.AppID() != AppId_t.Invalid && ((int) this.ModID() & int.MinValue) != 0;
        case CGameID.EGameIDType.k_EGameIDTypeShortcut:
          return ((int) this.ModID() & int.MinValue) != 0;
        case CGameID.EGameIDType.k_EGameIDTypeP2P:
          return this.AppID() == AppId_t.Invalid && ((int) this.ModID() & int.MinValue) != 0;
        default:
          return false;
      }
    }

    public void Reset() => this.m_GameID = 0UL;

    public void Set(ulong GameID) => this.m_GameID = GameID;

    private void SetAppID(AppId_t other)
    {
      this.m_GameID = (ulong) ((long) this.m_GameID & -16777216L | (long) (uint) other & 16777215L);
    }

    private void SetType(CGameID.EGameIDType other)
    {
      this.m_GameID = (ulong) ((long) this.m_GameID & -4278190081L | ((long) other & (long) byte.MaxValue) << 24);
    }

    private void SetModID(uint other)
    {
      this.m_GameID = (ulong) ((long) this.m_GameID & (long) uint.MaxValue | ((long) other & (long) uint.MaxValue) << 32);
    }

    public override string ToString() => this.m_GameID.ToString();

    public override bool Equals(object other) => other is CGameID cgameId && this == cgameId;

    public override int GetHashCode() => this.m_GameID.GetHashCode();

    public bool Equals(CGameID other) => (long) this.m_GameID == (long) other.m_GameID;

    public int CompareTo(CGameID other) => this.m_GameID.CompareTo(other.m_GameID);

    public static bool operator ==(CGameID x, CGameID y) => (long) x.m_GameID == (long) y.m_GameID;

    public static bool operator !=(CGameID x, CGameID y) => !(x == y);

    public static explicit operator CGameID(ulong value) => new CGameID(value);

    public static explicit operator ulong(CGameID that) => that.m_GameID;

    public enum EGameIDType
    {
      k_EGameIDTypeApp,
      k_EGameIDTypeGameMod,
      k_EGameIDTypeShortcut,
      k_EGameIDTypeP2P,
    }
  }
}
