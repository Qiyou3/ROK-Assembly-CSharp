﻿// Decompiled with JetBrains decompiler
// Type: Steamworks.HTML_NeedsPaint_t
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Steamworks
{
  [CallbackIdentity(4502)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct HTML_NeedsPaint_t
  {
    public const int k_iCallback = 4502;
    public HHTMLBrowser unBrowserHandle;
    public IntPtr pBGRA;
    public uint unWide;
    public uint unTall;
    public uint unUpdateX;
    public uint unUpdateY;
    public uint unUpdateWide;
    public uint unUpdateTall;
    public uint unScrollX;
    public uint unScrollY;
    public float flPageScale;
    public uint unPageSerial;
  }
}
