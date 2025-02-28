// Decompiled with JetBrains decompiler
// Type: Steamworks.ISteamMatchmakingPingResponse
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks
{
  public class ISteamMatchmakingPingResponse
  {
    private ISteamMatchmakingPingResponse.VTable m_VTable;
    private IntPtr m_pVTable;
    private GCHandle m_pGCHandle;
    private ISteamMatchmakingPingResponse.ServerResponded m_ServerResponded;
    private ISteamMatchmakingPingResponse.ServerFailedToRespond m_ServerFailedToRespond;

    public ISteamMatchmakingPingResponse(
      ISteamMatchmakingPingResponse.ServerResponded onServerResponded,
      ISteamMatchmakingPingResponse.ServerFailedToRespond onServerFailedToRespond)
    {
      this.m_ServerResponded = onServerResponded != null && onServerFailedToRespond != null ? onServerResponded : throw new ArgumentNullException();
      this.m_ServerFailedToRespond = onServerFailedToRespond;
      this.m_VTable = new ISteamMatchmakingPingResponse.VTable()
      {
        m_VTServerResponded = new ISteamMatchmakingPingResponse.InternalServerResponded(this.InternalOnServerResponded),
        m_VTServerFailedToRespond = new ISteamMatchmakingPingResponse.InternalServerFailedToRespond(this.InternalOnServerFailedToRespond)
      };
      this.m_pVTable = Marshal.AllocHGlobal(Marshal.SizeOf(typeof (ISteamMatchmakingPingResponse.VTable)));
      Marshal.StructureToPtr((object) this.m_VTable, this.m_pVTable, false);
      this.m_pGCHandle = GCHandle.Alloc((object) this.m_pVTable, GCHandleType.Pinned);
    }

    ~ISteamMatchmakingPingResponse()
    {
      if (this.m_pVTable != IntPtr.Zero)
        Marshal.FreeHGlobal(this.m_pVTable);
      if (!this.m_pGCHandle.IsAllocated)
        return;
      this.m_pGCHandle.Free();
    }

    private void InternalOnServerResponded(IntPtr thisptr, gameserveritem_t server)
    {
      this.m_ServerResponded(server);
    }

    private void InternalOnServerFailedToRespond(IntPtr thisptr) => this.m_ServerFailedToRespond();

    public static explicit operator IntPtr(ISteamMatchmakingPingResponse that)
    {
      return that.m_pGCHandle.AddrOfPinnedObject();
    }

    [StructLayout(LayoutKind.Sequential)]
    private class VTable
    {
      [MarshalAs(UnmanagedType.FunctionPtr)]
      [NonSerialized]
      public ISteamMatchmakingPingResponse.InternalServerResponded m_VTServerResponded;
      [MarshalAs(UnmanagedType.FunctionPtr)]
      [NonSerialized]
      public ISteamMatchmakingPingResponse.InternalServerFailedToRespond m_VTServerFailedToRespond;
    }

    public delegate void ServerResponded(gameserveritem_t server);

    public delegate void ServerFailedToRespond();

    [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
    private delegate void InternalServerResponded(IntPtr thisptr, gameserveritem_t server);

    [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
    private delegate void InternalServerFailedToRespond(IntPtr thisptr);
  }
}
