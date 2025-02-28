// Decompiled with JetBrains decompiler
// Type: Steamworks.Packsize
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks
{
  public static class Packsize
  {
    public const int value = 8;

    public static bool Test()
    {
      int num1 = Marshal.SizeOf(typeof (Packsize.ValvePackingSentinel_t));
      int num2 = Marshal.SizeOf(typeof (RemoteStorageEnumerateUserSubscribedFilesResult_t));
      return num1 == 32 && num2 == 616;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    private struct ValvePackingSentinel_t
    {
      private uint m_u32;
      private ulong m_u64;
      private ushort m_u16;
      private double m_d;
    }
  }
}
