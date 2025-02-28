// Decompiled with JetBrains decompiler
// Type: Steamworks.RemoteStorageAppSyncProgress_t
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks
{
  [CallbackIdentity(1303)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct RemoteStorageAppSyncProgress_t
  {
    public const int k_iCallback = 1303;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
    public string m_rgchCurrentFile;
    public AppId_t m_nAppID;
    public uint m_uBytesTransferredThisChunk;
    public double m_dAppPercentComplete;
    [MarshalAs(UnmanagedType.I1)]
    public bool m_bUploading;
  }
}
