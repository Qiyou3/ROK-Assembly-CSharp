// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamAppList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks
{
  public static class SteamAppList
  {
    public static uint GetNumInstalledApps()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamAppList_GetNumInstalledApps();
    }

    public static uint GetInstalledApps(AppId_t[] pvecAppID, uint unMaxAppIDs)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamAppList_GetInstalledApps(pvecAppID, unMaxAppIDs);
    }

    public static int GetAppName(AppId_t nAppID, out string pchName, int cchNameMax)
    {
      InteropHelp.TestIfAvailableClient();
      IntPtr num = Marshal.AllocHGlobal(cchNameMax);
      int appName = NativeMethods.ISteamAppList_GetAppName(nAppID, num, cchNameMax);
      pchName = appName == -1 ? (string) null : InteropHelp.PtrToStringUTF8(num);
      Marshal.FreeHGlobal(num);
      return appName;
    }

    public static int GetAppInstallDir(AppId_t nAppID, out string pchDirectory, int cchNameMax)
    {
      InteropHelp.TestIfAvailableClient();
      IntPtr num = Marshal.AllocHGlobal(cchNameMax);
      int appInstallDir = NativeMethods.ISteamAppList_GetAppInstallDir(nAppID, num, cchNameMax);
      pchDirectory = appInstallDir == -1 ? (string) null : InteropHelp.PtrToStringUTF8(num);
      Marshal.FreeHGlobal(num);
      return appInstallDir;
    }

    public static int GetAppBuildId(AppId_t nAppID)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamAppList_GetAppBuildId(nAppID);
    }
  }
}
