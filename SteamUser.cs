﻿// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamUser
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks
{
  public static class SteamUser
  {
    public static HSteamUser GetHSteamUser()
    {
      InteropHelp.TestIfAvailableClient();
      return (HSteamUser) NativeMethods.ISteamUser_GetHSteamUser();
    }

    public static bool BLoggedOn()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUser_BLoggedOn();
    }

    public static CSteamID GetSteamID()
    {
      InteropHelp.TestIfAvailableClient();
      return (CSteamID) NativeMethods.ISteamUser_GetSteamID();
    }

    public static int InitiateGameConnection(
      byte[] pAuthBlob,
      int cbMaxAuthBlob,
      CSteamID steamIDGameServer,
      uint unIPServer,
      ushort usPortServer,
      bool bSecure)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUser_InitiateGameConnection(pAuthBlob, cbMaxAuthBlob, steamIDGameServer, unIPServer, usPortServer, bSecure);
    }

    public static void TerminateGameConnection(uint unIPServer, ushort usPortServer)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamUser_TerminateGameConnection(unIPServer, usPortServer);
    }

    public static void TrackAppUsageEvent(CGameID gameID, int eAppUsageEvent, string pchExtraInfo = "")
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchExtraInfo1 = new InteropHelp.UTF8StringHandle(pchExtraInfo))
        NativeMethods.ISteamUser_TrackAppUsageEvent(gameID, eAppUsageEvent, pchExtraInfo1);
    }

    public static bool GetUserDataFolder(out string pchBuffer, int cubBuffer)
    {
      InteropHelp.TestIfAvailableClient();
      IntPtr num = Marshal.AllocHGlobal(cubBuffer);
      bool userDataFolder = NativeMethods.ISteamUser_GetUserDataFolder(num, cubBuffer);
      pchBuffer = !userDataFolder ? (string) null : InteropHelp.PtrToStringUTF8(num);
      Marshal.FreeHGlobal(num);
      return userDataFolder;
    }

    public static void StartVoiceRecording()
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamUser_StartVoiceRecording();
    }

    public static void StopVoiceRecording()
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamUser_StopVoiceRecording();
    }

    public static EVoiceResult GetAvailableVoice(
      out uint pcbCompressed,
      out uint pcbUncompressed,
      uint nUncompressedVoiceDesiredSampleRate)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUser_GetAvailableVoice(out pcbCompressed, out pcbUncompressed, nUncompressedVoiceDesiredSampleRate);
    }

    public static EVoiceResult GetVoice(
      bool bWantCompressed,
      byte[] pDestBuffer,
      uint cbDestBufferSize,
      out uint nBytesWritten,
      bool bWantUncompressed,
      byte[] pUncompressedDestBuffer,
      uint cbUncompressedDestBufferSize,
      out uint nUncompressBytesWritten,
      uint nUncompressedVoiceDesiredSampleRate)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUser_GetVoice(bWantCompressed, pDestBuffer, cbDestBufferSize, out nBytesWritten, bWantUncompressed, pUncompressedDestBuffer, cbUncompressedDestBufferSize, out nUncompressBytesWritten, nUncompressedVoiceDesiredSampleRate);
    }

    public static EVoiceResult DecompressVoice(
      byte[] pCompressed,
      uint cbCompressed,
      byte[] pDestBuffer,
      uint cbDestBufferSize,
      out uint nBytesWritten,
      uint nDesiredSampleRate)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUser_DecompressVoice(pCompressed, cbCompressed, pDestBuffer, cbDestBufferSize, out nBytesWritten, nDesiredSampleRate);
    }

    public static uint GetVoiceOptimalSampleRate()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUser_GetVoiceOptimalSampleRate();
    }

    public static HAuthTicket GetAuthSessionTicket(
      byte[] pTicket,
      int cbMaxTicket,
      out uint pcbTicket)
    {
      InteropHelp.TestIfAvailableClient();
      return (HAuthTicket) NativeMethods.ISteamUser_GetAuthSessionTicket(pTicket, cbMaxTicket, out pcbTicket);
    }

    public static EBeginAuthSessionResult BeginAuthSession(
      byte[] pAuthTicket,
      int cbAuthTicket,
      CSteamID steamID)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUser_BeginAuthSession(pAuthTicket, cbAuthTicket, steamID);
    }

    public static void EndAuthSession(CSteamID steamID)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamUser_EndAuthSession(steamID);
    }

    public static void CancelAuthTicket(HAuthTicket hAuthTicket)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamUser_CancelAuthTicket(hAuthTicket);
    }

    public static EUserHasLicenseForAppResult UserHasLicenseForApp(CSteamID steamID, AppId_t appID)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUser_UserHasLicenseForApp(steamID, appID);
    }

    public static bool BIsBehindNAT()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUser_BIsBehindNAT();
    }

    public static void AdvertiseGame(
      CSteamID steamIDGameServer,
      uint unIPServer,
      ushort usPortServer)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamUser_AdvertiseGame(steamIDGameServer, unIPServer, usPortServer);
    }

    public static SteamAPICall_t RequestEncryptedAppTicket(
      byte[] pDataToInclude,
      int cbDataToInclude)
    {
      InteropHelp.TestIfAvailableClient();
      return (SteamAPICall_t) NativeMethods.ISteamUser_RequestEncryptedAppTicket(pDataToInclude, cbDataToInclude);
    }

    public static bool GetEncryptedAppTicket(byte[] pTicket, int cbMaxTicket, out uint pcbTicket)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUser_GetEncryptedAppTicket(pTicket, cbMaxTicket, out pcbTicket);
    }

    public static int GetGameBadgeLevel(int nSeries, bool bFoil)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUser_GetGameBadgeLevel(nSeries, bFoil);
    }

    public static int GetPlayerSteamLevel()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUser_GetPlayerSteamLevel();
    }

    public static SteamAPICall_t RequestStoreAuthURL(string pchRedirectURL)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchRedirectURL1 = new InteropHelp.UTF8StringHandle(pchRedirectURL))
        return (SteamAPICall_t) NativeMethods.ISteamUser_RequestStoreAuthURL(pchRedirectURL1);
    }
  }
}
