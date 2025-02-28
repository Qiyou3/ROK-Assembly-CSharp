﻿// Decompiled with JetBrains decompiler
// Type: Steamworks.CSteamID
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Steamworks
{
  public struct CSteamID : IEquatable<CSteamID>, IComparable<CSteamID>
  {
    public static readonly CSteamID Nil = new CSteamID();
    public static readonly CSteamID OutofDateGS = new CSteamID(new AccountID_t(0U), 0U, EUniverse.k_EUniverseInvalid, EAccountType.k_EAccountTypeInvalid);
    public static readonly CSteamID LanModeGS = new CSteamID(new AccountID_t(0U), 0U, EUniverse.k_EUniversePublic, EAccountType.k_EAccountTypeInvalid);
    public static readonly CSteamID NotInitYetGS = new CSteamID(new AccountID_t(1U), 0U, EUniverse.k_EUniverseInvalid, EAccountType.k_EAccountTypeInvalid);
    public static readonly CSteamID NonSteamGS = new CSteamID(new AccountID_t(2U), 0U, EUniverse.k_EUniverseInvalid, EAccountType.k_EAccountTypeInvalid);
    public ulong m_SteamID;

    public CSteamID(AccountID_t unAccountID, EUniverse eUniverse, EAccountType eAccountType)
    {
      this.m_SteamID = 0UL;
      this.Set(unAccountID, eUniverse, eAccountType);
    }

    public CSteamID(
      AccountID_t unAccountID,
      uint unAccountInstance,
      EUniverse eUniverse,
      EAccountType eAccountType)
    {
      this.m_SteamID = 0UL;
      this.InstancedSet(unAccountID, unAccountInstance, eUniverse, eAccountType);
    }

    public CSteamID(ulong ulSteamID) => this.m_SteamID = ulSteamID;

    public void Set(AccountID_t unAccountID, EUniverse eUniverse, EAccountType eAccountType)
    {
      this.SetAccountID(unAccountID);
      this.SetEUniverse(eUniverse);
      this.SetEAccountType(eAccountType);
      if (eAccountType == EAccountType.k_EAccountTypeClan || eAccountType == EAccountType.k_EAccountTypeGameServer)
        this.SetAccountInstance(0U);
      else
        this.SetAccountInstance(1U);
    }

    public void InstancedSet(
      AccountID_t unAccountID,
      uint unInstance,
      EUniverse eUniverse,
      EAccountType eAccountType)
    {
      this.SetAccountID(unAccountID);
      this.SetEUniverse(eUniverse);
      this.SetEAccountType(eAccountType);
      this.SetAccountInstance(unInstance);
    }

    public void Clear() => this.m_SteamID = 0UL;

    public void CreateBlankAnonLogon(EUniverse eUniverse)
    {
      this.SetAccountID(new AccountID_t(0U));
      this.SetEUniverse(eUniverse);
      this.SetEAccountType(EAccountType.k_EAccountTypeAnonGameServer);
      this.SetAccountInstance(0U);
    }

    public void CreateBlankAnonUserLogon(EUniverse eUniverse)
    {
      this.SetAccountID(new AccountID_t(0U));
      this.SetEUniverse(eUniverse);
      this.SetEAccountType(EAccountType.k_EAccountTypeAnonUser);
      this.SetAccountInstance(0U);
    }

    public bool BBlankAnonAccount()
    {
      return this.GetAccountID() == new AccountID_t(0U) && this.BAnonAccount() && this.GetUnAccountInstance() == 0U;
    }

    public bool BGameServerAccount()
    {
      return this.GetEAccountType() == EAccountType.k_EAccountTypeGameServer || this.GetEAccountType() == EAccountType.k_EAccountTypeAnonGameServer;
    }

    public bool BPersistentGameServerAccount()
    {
      return this.GetEAccountType() == EAccountType.k_EAccountTypeGameServer;
    }

    public bool BAnonGameServerAccount()
    {
      return this.GetEAccountType() == EAccountType.k_EAccountTypeAnonGameServer;
    }

    public bool BContentServerAccount()
    {
      return this.GetEAccountType() == EAccountType.k_EAccountTypeContentServer;
    }

    public bool BClanAccount() => this.GetEAccountType() == EAccountType.k_EAccountTypeClan;

    public bool BChatAccount() => this.GetEAccountType() == EAccountType.k_EAccountTypeChat;

    public bool IsLobby()
    {
      return this.GetEAccountType() == EAccountType.k_EAccountTypeChat && ((int) this.GetUnAccountInstance() & 262144) != 0;
    }

    public bool BIndividualAccount()
    {
      return this.GetEAccountType() == EAccountType.k_EAccountTypeIndividual || this.GetEAccountType() == EAccountType.k_EAccountTypeConsoleUser;
    }

    public bool BAnonAccount()
    {
      return this.GetEAccountType() == EAccountType.k_EAccountTypeAnonUser || this.GetEAccountType() == EAccountType.k_EAccountTypeAnonGameServer;
    }

    public bool BAnonUserAccount() => this.GetEAccountType() == EAccountType.k_EAccountTypeAnonUser;

    public bool BConsoleUserAccount()
    {
      return this.GetEAccountType() == EAccountType.k_EAccountTypeConsoleUser;
    }

    public void SetAccountID(AccountID_t other)
    {
      this.m_SteamID = (ulong) ((long) this.m_SteamID & -4294967296L | (long) (uint) other & (long) uint.MaxValue);
    }

    public void SetAccountInstance(uint other)
    {
      this.m_SteamID = (ulong) ((long) this.m_SteamID & -4503595332403201L | ((long) other & 1048575L) << 32);
    }

    public void SetEAccountType(EAccountType other)
    {
      this.m_SteamID = (ulong) ((long) this.m_SteamID & -67553994410557441L | ((long) other & 15L) << 52);
    }

    public void SetEUniverse(EUniverse other)
    {
      this.m_SteamID = (ulong) ((long) this.m_SteamID & 72057594037927935L | ((long) other & (long) byte.MaxValue) << 56);
    }

    public void ClearIndividualInstance()
    {
      if (!this.BIndividualAccount())
        return;
      this.SetAccountInstance(0U);
    }

    public bool HasNoIndividualInstance()
    {
      return this.BIndividualAccount() && this.GetUnAccountInstance() == 0U;
    }

    public AccountID_t GetAccountID()
    {
      return new AccountID_t((uint) (this.m_SteamID & (ulong) uint.MaxValue));
    }

    public uint GetUnAccountInstance() => (uint) (this.m_SteamID >> 32 & 1048575UL);

    public EAccountType GetEAccountType() => (EAccountType) ((long) (this.m_SteamID >> 52) & 15L);

    public EUniverse GetEUniverse()
    {
      return (EUniverse) ((long) (this.m_SteamID >> 56) & (long) byte.MaxValue);
    }

    public bool IsValid()
    {
      return this.GetEAccountType() > EAccountType.k_EAccountTypeInvalid && this.GetEAccountType() < EAccountType.k_EAccountTypeMax && this.GetEUniverse() > EUniverse.k_EUniverseInvalid && this.GetEUniverse() < EUniverse.k_EUniverseMax && (this.GetEAccountType() != EAccountType.k_EAccountTypeIndividual || !(this.GetAccountID() == new AccountID_t(0U)) && this.GetUnAccountInstance() <= 4U) && (this.GetEAccountType() != EAccountType.k_EAccountTypeClan || !(this.GetAccountID() == new AccountID_t(0U)) && this.GetUnAccountInstance() == 0U) && (this.GetEAccountType() != EAccountType.k_EAccountTypeGameServer || !(this.GetAccountID() == new AccountID_t(0U)));
    }

    public override string ToString() => this.m_SteamID.ToString();

    public override bool Equals(object other) => other is CSteamID csteamId && this == csteamId;

    public override int GetHashCode() => this.m_SteamID.GetHashCode();

    public bool Equals(CSteamID other) => (long) this.m_SteamID == (long) other.m_SteamID;

    public int CompareTo(CSteamID other) => this.m_SteamID.CompareTo(other.m_SteamID);

    public static bool operator ==(CSteamID x, CSteamID y)
    {
      return (long) x.m_SteamID == (long) y.m_SteamID;
    }

    public static bool operator !=(CSteamID x, CSteamID y) => !(x == y);

    public static explicit operator CSteamID(ulong value) => new CSteamID(value);

    public static explicit operator ulong(CSteamID that) => that.m_SteamID;
  }
}
