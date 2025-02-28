// Decompiled with JetBrains decompiler
// Type: Steamworks.CCallbackBaseVTable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks
{
  [StructLayout(LayoutKind.Sequential)]
  internal class CCallbackBaseVTable
  {
    private const CallingConvention cc = CallingConvention.Cdecl;
    [MarshalAs(UnmanagedType.FunctionPtr)]
    [NonSerialized]
    public CCallbackBaseVTable.RunCRDel m_RunCallResult;
    [MarshalAs(UnmanagedType.FunctionPtr)]
    [NonSerialized]
    public CCallbackBaseVTable.RunCBDel m_RunCallback;
    [MarshalAs(UnmanagedType.FunctionPtr)]
    [NonSerialized]
    public CCallbackBaseVTable.GetCallbackSizeBytesDel m_GetCallbackSizeBytes;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void RunCBDel(IntPtr thisptr, IntPtr pvParam);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void RunCRDel(
      IntPtr thisptr,
      IntPtr pvParam,
      [MarshalAs(UnmanagedType.I1)] bool bIOFailure,
      ulong hSteamAPICall);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int GetCallbackSizeBytesDel(IntPtr thisptr);
  }
}
