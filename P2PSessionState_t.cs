// Decompiled with JetBrains decompiler
// Type: Steamworks.P2PSessionState_t
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks
{
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct P2PSessionState_t
  {
    public byte m_bConnectionActive;
    public byte m_bConnecting;
    public byte m_eP2PSessionError;
    public byte m_bUsingRelay;
    public int m_nBytesQueuedForSend;
    public int m_nPacketsQueuedForSend;
    public uint m_nRemoteIP;
    public ushort m_nRemotePort;
  }
}
