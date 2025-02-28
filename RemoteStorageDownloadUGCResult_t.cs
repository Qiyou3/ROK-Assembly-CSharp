// Decompiled with JetBrains decompiler
// Type: Steamworks.RemoteStorageDownloadUGCResult_t
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks
{
  [CallbackIdentity(1317)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct RemoteStorageDownloadUGCResult_t
  {
    public const int k_iCallback = 1317;
    public EResult m_eResult;
    public UGCHandle_t m_hFile;
    public AppId_t m_nAppID;
    public int m_nSizeInBytes;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
    public string m_pchFileName;
    public ulong m_ulSteamIDOwner;
  }
}
