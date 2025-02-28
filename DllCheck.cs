// Decompiled with JetBrains decompiler
// Type: Steamworks.DllCheck
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace Steamworks
{
  public class DllCheck
  {
    [DllImport("kernel32.dll")]
    public static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    private static extern int GetModuleFileName(
      IntPtr hModule,
      StringBuilder strFullPath,
      int nSize);

    public static bool Test() => true;

    private static bool CheckSteamAPIDLL()
    {
      string lpModuleName;
      int num;
      if (IntPtr.Size == 4)
      {
        lpModuleName = "steam_api.dll";
        num = 186560;
      }
      else
      {
        lpModuleName = "steam_api64.dll";
        num = 206760;
      }
      IntPtr moduleHandle = DllCheck.GetModuleHandle(lpModuleName);
      if (moduleHandle == IntPtr.Zero)
        return true;
      StringBuilder strFullPath = new StringBuilder(256);
      DllCheck.GetModuleFileName(moduleHandle, strFullPath, strFullPath.Capacity);
      string str = strFullPath.ToString();
      return !File.Exists(str) || new FileInfo(str).Length == (long) num && !(FileVersionInfo.GetVersionInfo(str).FileVersion != "02.89.45.04");
    }
  }
}
