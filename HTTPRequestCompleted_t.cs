// Decompiled with JetBrains decompiler
// Type: Steamworks.HTTPRequestCompleted_t
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks
{
  [CallbackIdentity(2101)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct HTTPRequestCompleted_t
  {
    public const int k_iCallback = 2101;
    public HTTPRequestHandle m_hRequest;
    public ulong m_ulContextValue;
    [MarshalAs(UnmanagedType.I1)]
    public bool m_bRequestSuccessful;
    public EHTTPStatusCode m_eStatusCode;
    public uint m_unBodySize;
  }
}
