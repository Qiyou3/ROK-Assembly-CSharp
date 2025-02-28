// Decompiled with JetBrains decompiler
// Type: Steamworks.EMarketingMessageFlags
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Steamworks
{
  [Flags]
  public enum EMarketingMessageFlags
  {
    k_EMarketingMessageFlagsNone = 0,
    k_EMarketingMessageFlagsHighPriority = 1,
    k_EMarketingMessageFlagsPlatformWindows = 2,
    k_EMarketingMessageFlagsPlatformMac = 4,
    k_EMarketingMessageFlagsPlatformLinux = 8,
    k_EMarketingMessageFlagsPlatformRestrictions = k_EMarketingMessageFlagsPlatformLinux | k_EMarketingMessageFlagsPlatformMac | k_EMarketingMessageFlagsPlatformWindows, // 0x0000000E
  }
}
