// Decompiled with JetBrains decompiler
// Type: Steamworks.CCallbackBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks
{
  [StructLayout(LayoutKind.Sequential)]
  internal class CCallbackBase
  {
    public const byte k_ECallbackFlagsRegistered = 1;
    public const byte k_ECallbackFlagsGameServer = 2;
    public IntPtr m_vfptr;
    public byte m_nCallbackFlags;
    public int m_iCallback;
  }
}
