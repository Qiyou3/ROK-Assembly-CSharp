// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamControllerState_t
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct SteamControllerState_t
  {
    public uint unPacketNum;
    public ulong ulButtons;
    public short sLeftPadX;
    public short sLeftPadY;
    public short sRightPadX;
    public short sRightPadY;
  }
}
