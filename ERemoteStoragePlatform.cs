// Decompiled with JetBrains decompiler
// Type: Steamworks.ERemoteStoragePlatform
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Steamworks
{
  [Flags]
  public enum ERemoteStoragePlatform
  {
    k_ERemoteStoragePlatformNone = 0,
    k_ERemoteStoragePlatformWindows = 1,
    k_ERemoteStoragePlatformOSX = 2,
    k_ERemoteStoragePlatformPS3 = 4,
    k_ERemoteStoragePlatformLinux = 8,
    k_ERemoteStoragePlatformReserved2 = 16, // 0x00000010
    k_ERemoteStoragePlatformAll = -1, // 0xFFFFFFFF
  }
}
