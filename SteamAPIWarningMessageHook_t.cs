// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamAPIWarningMessageHook_t
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace Steamworks
{
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void SteamAPIWarningMessageHook_t(int nSeverity, StringBuilder pchDebugText);
}
