// Decompiled with JetBrains decompiler
// Type: Steamworks.UserAchievementIconFetched_t
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks
{
  [CallbackIdentity(1109)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct UserAchievementIconFetched_t
  {
    public const int k_iCallback = 1109;
    public CGameID m_nGameID;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
    public string m_rgchAchievementName;
    [MarshalAs(UnmanagedType.I1)]
    public bool m_bAchieved;
    public int m_nIconHandle;
  }
}
