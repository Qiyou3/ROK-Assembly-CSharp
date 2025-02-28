// Decompiled with JetBrains decompiler
// Type: Steamworks.ESteamItemFlags
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Steamworks
{
  [Flags]
  public enum ESteamItemFlags
  {
    k_ESteamItemNoTrade = 1,
    k_ESteamItemRemoved = 256, // 0x00000100
    k_ESteamItemConsumed = 512, // 0x00000200
  }
}
