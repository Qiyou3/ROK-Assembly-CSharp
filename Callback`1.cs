// Decompiled with JetBrains decompiler
// Type: Steamworks.Callback`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks
{
  public sealed class Callback<T>
  {
    private CCallbackBaseVTable VTable;
    private IntPtr m_pVTable = IntPtr.Zero;
    private CCallbackBase m_CCallbackBase;
    private GCHandle m_pCCallbackBase;
    private bool m_bGameServer;
    private readonly int m_size = Marshal.SizeOf(typeof (T));

    public Callback(Callback<T>.DispatchDelegate func, bool bGameServer = false)
    {
      this.m_bGameServer = bGameServer;
      this.BuildCCallbackBase();
      this.Register(func);
    }

    private event Callback<T>.DispatchDelegate m_Func;

    public static Callback<T> Create(Callback<T>.DispatchDelegate func) => new Callback<T>(func);

    public static Callback<T> CreateGameServer(Callback<T>.DispatchDelegate func)
    {
      return new Callback<T>(func, true);
    }

    ~Callback()
    {
      this.Unregister();
      if (this.m_pVTable != IntPtr.Zero)
        Marshal.FreeHGlobal(this.m_pVTable);
      if (!this.m_pCCallbackBase.IsAllocated)
        return;
      this.m_pCCallbackBase.Free();
    }

    public void Register(Callback<T>.DispatchDelegate func)
    {
      if (func == null)
        throw new Exception("Callback function must not be null.");
      if (((int) this.m_CCallbackBase.m_nCallbackFlags & 1) == 1)
        this.Unregister();
      if (this.m_bGameServer)
        this.SetGameserverFlag();
      this.m_Func = func;
      NativeMethods.SteamAPI_RegisterCallback(this.m_pCCallbackBase.AddrOfPinnedObject(), CallbackIdentities.GetCallbackIdentity(typeof (T)));
    }

    public void Unregister()
    {
      NativeMethods.SteamAPI_UnregisterCallback(this.m_pCCallbackBase.AddrOfPinnedObject());
    }

    public void SetGameserverFlag() => this.m_CCallbackBase.m_nCallbackFlags |= (byte) 2;

    private void OnRunCallback(IntPtr thisptr, IntPtr pvParam)
    {
      try
      {
        this.m_Func((T) Marshal.PtrToStructure(pvParam, typeof (T)));
      }
      catch (Exception ex)
      {
        CallbackDispatcher.ExceptionHandler(ex);
      }
    }

    private void OnRunCallResult(
      IntPtr thisptr,
      IntPtr pvParam,
      bool bFailed,
      ulong hSteamAPICall)
    {
      try
      {
        this.m_Func((T) Marshal.PtrToStructure(pvParam, typeof (T)));
      }
      catch (Exception ex)
      {
        CallbackDispatcher.ExceptionHandler(ex);
      }
    }

    private int OnGetCallbackSizeBytes(IntPtr thisptr) => this.m_size;

    private void BuildCCallbackBase()
    {
      this.VTable = new CCallbackBaseVTable()
      {
        m_RunCallResult = new CCallbackBaseVTable.RunCRDel(this.OnRunCallResult),
        m_RunCallback = new CCallbackBaseVTable.RunCBDel(this.OnRunCallback),
        m_GetCallbackSizeBytes = new CCallbackBaseVTable.GetCallbackSizeBytesDel(this.OnGetCallbackSizeBytes)
      };
      this.m_pVTable = Marshal.AllocHGlobal(Marshal.SizeOf(typeof (CCallbackBaseVTable)));
      Marshal.StructureToPtr((object) this.VTable, this.m_pVTable, false);
      this.m_CCallbackBase = new CCallbackBase()
      {
        m_vfptr = this.m_pVTable,
        m_iCallback = CallbackIdentities.GetCallbackIdentity(typeof (T))
      };
      this.m_pCCallbackBase = GCHandle.Alloc((object) this.m_CCallbackBase, GCHandleType.Pinned);
    }

    public delegate void DispatchDelegate(T param);
  }
}
