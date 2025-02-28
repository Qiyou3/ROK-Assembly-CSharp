// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamVideo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace Steamworks
{
  public static class SteamVideo
  {
    public static void GetVideoURL(AppId_t unVideoAppID)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamVideo_GetVideoURL(unVideoAppID);
    }

    public static bool IsBroadcasting(out int pnNumViewers)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamVideo_IsBroadcasting(out pnNumViewers);
    }
  }
}
