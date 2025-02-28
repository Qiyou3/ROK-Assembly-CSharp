// Decompiled with JetBrains decompiler
// Type: Steamworks.LeaderboardEntry_t
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks
{
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct LeaderboardEntry_t
  {
    public CSteamID m_steamIDUser;
    public int m_nGlobalRank;
    public int m_nScore;
    public int m_cDetails;
    public UGCHandle_t m_hUGC;
  }
}
