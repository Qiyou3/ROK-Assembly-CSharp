// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamUnifiedMessages
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace Steamworks
{
  public static class SteamUnifiedMessages
  {
    public static ClientUnifiedMessageHandle SendMethod(
      string pchServiceMethod,
      byte[] pRequestBuffer,
      uint unRequestBufferSize,
      ulong unContext)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchServiceMethod1 = new InteropHelp.UTF8StringHandle(pchServiceMethod))
        return (ClientUnifiedMessageHandle) NativeMethods.ISteamUnifiedMessages_SendMethod(pchServiceMethod1, pRequestBuffer, unRequestBufferSize, unContext);
    }

    public static bool GetMethodResponseInfo(
      ClientUnifiedMessageHandle hHandle,
      out uint punResponseSize,
      out EResult peResult)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUnifiedMessages_GetMethodResponseInfo(hHandle, out punResponseSize, out peResult);
    }

    public static bool GetMethodResponseData(
      ClientUnifiedMessageHandle hHandle,
      byte[] pResponseBuffer,
      uint unResponseBufferSize,
      bool bAutoRelease)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUnifiedMessages_GetMethodResponseData(hHandle, pResponseBuffer, unResponseBufferSize, bAutoRelease);
    }

    public static bool ReleaseMethod(ClientUnifiedMessageHandle hHandle)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamUnifiedMessages_ReleaseMethod(hHandle);
    }

    public static bool SendNotification(
      string pchServiceNotification,
      byte[] pNotificationBuffer,
      uint unNotificationBufferSize)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchServiceNotification1 = new InteropHelp.UTF8StringHandle(pchServiceNotification))
        return NativeMethods.ISteamUnifiedMessages_SendNotification(pchServiceNotification1, pNotificationBuffer, unNotificationBufferSize);
    }
  }
}
