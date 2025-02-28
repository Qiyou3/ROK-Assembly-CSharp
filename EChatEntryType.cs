// Decompiled with JetBrains decompiler
// Type: Steamworks.EChatEntryType
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace Steamworks
{
  public enum EChatEntryType
  {
    k_EChatEntryTypeInvalid = 0,
    k_EChatEntryTypeChatMsg = 1,
    k_EChatEntryTypeTyping = 2,
    k_EChatEntryTypeInviteGame = 3,
    k_EChatEntryTypeEmote = 4,
    k_EChatEntryTypeLeftConversation = 6,
    k_EChatEntryTypeEntered = 7,
    k_EChatEntryTypeWasKicked = 8,
    k_EChatEntryTypeWasBanned = 9,
    k_EChatEntryTypeDisconnected = 10, // 0x0000000A
    k_EChatEntryTypeHistoricalChat = 11, // 0x0000000B
    k_EChatEntryTypeReserved1 = 12, // 0x0000000C
    k_EChatEntryTypeReserved2 = 13, // 0x0000000D
    k_EChatEntryTypeLinkBlocked = 14, // 0x0000000E
  }
}
