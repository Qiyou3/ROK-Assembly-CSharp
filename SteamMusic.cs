﻿// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamMusic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace Steamworks
{
  public static class SteamMusic
  {
    public static bool BIsEnabled()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamMusic_BIsEnabled();
    }

    public static bool BIsPlaying()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamMusic_BIsPlaying();
    }

    public static AudioPlayback_Status GetPlaybackStatus()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamMusic_GetPlaybackStatus();
    }

    public static void Play()
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamMusic_Play();
    }

    public static void Pause()
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamMusic_Pause();
    }

    public static void PlayPrevious()
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamMusic_PlayPrevious();
    }

    public static void PlayNext()
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamMusic_PlayNext();
    }

    public static void SetVolume(float flVolume)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamMusic_SetVolume(flVolume);
    }

    public static float GetVolume()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamMusic_GetVolume();
    }
  }
}
