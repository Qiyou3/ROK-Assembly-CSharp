﻿// Decompiled with JetBrains decompiler
// Type: Steamworks.EChatSteamIDInstanceFlags
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Steamworks
{
  [Flags]
  public enum EChatSteamIDInstanceFlags
  {
    k_EChatAccountInstanceMask = 4095, // 0x00000FFF
    k_EChatInstanceFlagClan = 524288, // 0x00080000
    k_EChatInstanceFlagLobby = 262144, // 0x00040000
    k_EChatInstanceFlagMMSLobby = 131072, // 0x00020000
  }
}
