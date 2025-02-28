﻿// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamAPI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace Steamworks
{
  public static class SteamAPI
  {
    public static bool RestartAppIfNecessary(AppId_t unOwnAppID)
    {
      InteropHelp.TestIfPlatformSupported();
      return NativeMethods.SteamAPI_RestartAppIfNecessary(unOwnAppID);
    }

    public static bool InitSafe() => SteamAPI.Init();

    public static bool Init()
    {
      InteropHelp.TestIfPlatformSupported();
      return NativeMethods.SteamAPI_InitSafe();
    }

    public static void Shutdown()
    {
      InteropHelp.TestIfPlatformSupported();
      NativeMethods.SteamAPI_Shutdown();
    }

    public static void RunCallbacks()
    {
      InteropHelp.TestIfPlatformSupported();
      NativeMethods.SteamAPI_RunCallbacks();
    }

    public static bool IsSteamRunning()
    {
      InteropHelp.TestIfPlatformSupported();
      return NativeMethods.SteamAPI_IsSteamRunning();
    }

    public static HSteamUser GetHSteamUserCurrent()
    {
      InteropHelp.TestIfPlatformSupported();
      return (HSteamUser) NativeMethods.Steam_GetHSteamUserCurrent();
    }

    public static HSteamPipe GetHSteamPipe()
    {
      InteropHelp.TestIfPlatformSupported();
      return (HSteamPipe) NativeMethods.SteamAPI_GetHSteamPipe();
    }

    public static HSteamUser GetHSteamUser()
    {
      InteropHelp.TestIfPlatformSupported();
      return (HSteamUser) NativeMethods.SteamAPI_GetHSteamUser();
    }
  }
}
