// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamAPI_PostAPIResultInProcess_t
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks
{
  [UnmanagedFunctionPointer(CallingConvention.StdCall)]
  public delegate void SteamAPI_PostAPIResultInProcess_t(
    SteamAPICall_t callHandle,
    IntPtr pUnknown,
    uint unCallbackSize,
    int iCallbackNum);
}
