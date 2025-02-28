// Decompiled with JetBrains decompiler
// Type: Steamworks.UserStatsReceived_t
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks
{
  [CallbackIdentity(1101)]
  [StructLayout(LayoutKind.Explicit, Pack = 8)]
  public struct UserStatsReceived_t
  {
    public const int k_iCallback = 1101;
    [FieldOffset(0)]
    public ulong m_nGameID;
    [FieldOffset(8)]
    public EResult m_eResult;
    [FieldOffset(12)]
    public CSteamID m_steamIDUser;
  }
}
