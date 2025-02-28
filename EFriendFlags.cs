// Decompiled with JetBrains decompiler
// Type: Steamworks.EFriendFlags
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Steamworks
{
  [Flags]
  public enum EFriendFlags
  {
    k_EFriendFlagNone = 0,
    k_EFriendFlagBlocked = 1,
    k_EFriendFlagFriendshipRequested = 2,
    k_EFriendFlagImmediate = 4,
    k_EFriendFlagClanMember = 8,
    k_EFriendFlagOnGameServer = 16, // 0x00000010
    k_EFriendFlagRequestingFriendship = 128, // 0x00000080
    k_EFriendFlagRequestingInfo = 256, // 0x00000100
    k_EFriendFlagIgnored = 512, // 0x00000200
    k_EFriendFlagIgnoredFriend = 1024, // 0x00000400
    k_EFriendFlagSuggested = 2048, // 0x00000800
    k_EFriendFlagAll = 65535, // 0x0000FFFF
  }
}
