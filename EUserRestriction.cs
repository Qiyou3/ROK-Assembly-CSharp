// Decompiled with JetBrains decompiler
// Type: Steamworks.EUserRestriction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace Steamworks
{
  public enum EUserRestriction
  {
    k_nUserRestrictionNone = 0,
    k_nUserRestrictionUnknown = 1,
    k_nUserRestrictionAnyChat = 2,
    k_nUserRestrictionVoiceChat = 4,
    k_nUserRestrictionGroupChat = 8,
    k_nUserRestrictionRating = 16, // 0x00000010
    k_nUserRestrictionGameInvites = 32, // 0x00000020
    k_nUserRestrictionTrading = 64, // 0x00000040
  }
}
