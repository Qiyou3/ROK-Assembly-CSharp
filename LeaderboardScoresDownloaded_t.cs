// Decompiled with JetBrains decompiler
// Type: Steamworks.LeaderboardScoresDownloaded_t
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks
{
  [CallbackIdentity(1105)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct LeaderboardScoresDownloaded_t
  {
    public const int k_iCallback = 1105;
    public SteamLeaderboard_t m_hSteamLeaderboard;
    public SteamLeaderboardEntries_t m_hSteamLeaderboardEntries;
    public int m_cEntryCount;
  }
}
