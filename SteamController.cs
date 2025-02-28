// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace Steamworks
{
  public static class SteamController
  {
    public static bool Init(string pchAbsolutePathToControllerConfigVDF)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchAbsolutePathToControllerConfigVDF1 = new InteropHelp.UTF8StringHandle(pchAbsolutePathToControllerConfigVDF))
        return NativeMethods.ISteamController_Init(pchAbsolutePathToControllerConfigVDF1);
    }

    public static bool Shutdown()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamController_Shutdown();
    }

    public static void RunFrame()
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamController_RunFrame();
    }

    public static bool GetControllerState(uint unControllerIndex, out SteamControllerState_t pState)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamController_GetControllerState(unControllerIndex, out pState);
    }

    public static void TriggerHapticPulse(
      uint unControllerIndex,
      ESteamControllerPad eTargetPad,
      ushort usDurationMicroSec)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamController_TriggerHapticPulse(unControllerIndex, eTargetPad, usDurationMicroSec);
    }

    public static void SetOverrideMode(string pchMode)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchMode1 = new InteropHelp.UTF8StringHandle(pchMode))
        NativeMethods.ISteamController_SetOverrideMode(pchMode1);
    }
  }
}
