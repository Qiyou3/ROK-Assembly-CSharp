// Decompiled with JetBrains decompiler
// Type: Steamworks.JoinClanChatRoomCompletionResult_t
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks
{
  [CallbackIdentity(342)]
  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct JoinClanChatRoomCompletionResult_t
  {
    public const int k_iCallback = 342;
    public CSteamID m_steamIDClanChat;
    public EChatRoomEnterResponse m_eChatRoomEnterResponse;
  }
}
