﻿// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamHTMLSurface
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace Steamworks
{
  public static class SteamHTMLSurface
  {
    public static bool Init()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamHTMLSurface_Init();
    }

    public static bool Shutdown()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamHTMLSurface_Shutdown();
    }

    public static SteamAPICall_t CreateBrowser(string pchUserAgent, string pchUserCSS)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchUserAgent1 = new InteropHelp.UTF8StringHandle(pchUserAgent))
      {
        using (InteropHelp.UTF8StringHandle pchUserCSS1 = new InteropHelp.UTF8StringHandle(pchUserCSS))
          return (SteamAPICall_t) NativeMethods.ISteamHTMLSurface_CreateBrowser(pchUserAgent1, pchUserCSS1);
      }
    }

    public static void RemoveBrowser(HHTMLBrowser unBrowserHandle)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamHTMLSurface_RemoveBrowser(unBrowserHandle);
    }

    public static void LoadURL(HHTMLBrowser unBrowserHandle, string pchURL, string pchPostData)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchURL1 = new InteropHelp.UTF8StringHandle(pchURL))
      {
        using (InteropHelp.UTF8StringHandle pchPostData1 = new InteropHelp.UTF8StringHandle(pchPostData))
          NativeMethods.ISteamHTMLSurface_LoadURL(unBrowserHandle, pchURL1, pchPostData1);
      }
    }

    public static void SetSize(HHTMLBrowser unBrowserHandle, uint unWidth, uint unHeight)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamHTMLSurface_SetSize(unBrowserHandle, unWidth, unHeight);
    }

    public static void StopLoad(HHTMLBrowser unBrowserHandle)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamHTMLSurface_StopLoad(unBrowserHandle);
    }

    public static void Reload(HHTMLBrowser unBrowserHandle)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamHTMLSurface_Reload(unBrowserHandle);
    }

    public static void GoBack(HHTMLBrowser unBrowserHandle)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamHTMLSurface_GoBack(unBrowserHandle);
    }

    public static void GoForward(HHTMLBrowser unBrowserHandle)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamHTMLSurface_GoForward(unBrowserHandle);
    }

    public static void AddHeader(HHTMLBrowser unBrowserHandle, string pchKey, string pchValue)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchKey1 = new InteropHelp.UTF8StringHandle(pchKey))
      {
        using (InteropHelp.UTF8StringHandle pchValue1 = new InteropHelp.UTF8StringHandle(pchValue))
          NativeMethods.ISteamHTMLSurface_AddHeader(unBrowserHandle, pchKey1, pchValue1);
      }
    }

    public static void ExecuteJavascript(HHTMLBrowser unBrowserHandle, string pchScript)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchScript1 = new InteropHelp.UTF8StringHandle(pchScript))
        NativeMethods.ISteamHTMLSurface_ExecuteJavascript(unBrowserHandle, pchScript1);
    }

    public static void MouseUp(HHTMLBrowser unBrowserHandle, EHTMLMouseButton eMouseButton)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamHTMLSurface_MouseUp(unBrowserHandle, eMouseButton);
    }

    public static void MouseDown(HHTMLBrowser unBrowserHandle, EHTMLMouseButton eMouseButton)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamHTMLSurface_MouseDown(unBrowserHandle, eMouseButton);
    }

    public static void MouseDoubleClick(HHTMLBrowser unBrowserHandle, EHTMLMouseButton eMouseButton)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamHTMLSurface_MouseDoubleClick(unBrowserHandle, eMouseButton);
    }

    public static void MouseMove(HHTMLBrowser unBrowserHandle, int x, int y)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamHTMLSurface_MouseMove(unBrowserHandle, x, y);
    }

    public static void MouseWheel(HHTMLBrowser unBrowserHandle, int nDelta)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamHTMLSurface_MouseWheel(unBrowserHandle, nDelta);
    }

    public static void KeyDown(
      HHTMLBrowser unBrowserHandle,
      uint nNativeKeyCode,
      EHTMLKeyModifiers eHTMLKeyModifiers)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamHTMLSurface_KeyDown(unBrowserHandle, nNativeKeyCode, eHTMLKeyModifiers);
    }

    public static void KeyUp(
      HHTMLBrowser unBrowserHandle,
      uint nNativeKeyCode,
      EHTMLKeyModifiers eHTMLKeyModifiers)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamHTMLSurface_KeyUp(unBrowserHandle, nNativeKeyCode, eHTMLKeyModifiers);
    }

    public static void KeyChar(
      HHTMLBrowser unBrowserHandle,
      uint cUnicodeChar,
      EHTMLKeyModifiers eHTMLKeyModifiers)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamHTMLSurface_KeyChar(unBrowserHandle, cUnicodeChar, eHTMLKeyModifiers);
    }

    public static void SetHorizontalScroll(HHTMLBrowser unBrowserHandle, uint nAbsolutePixelScroll)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamHTMLSurface_SetHorizontalScroll(unBrowserHandle, nAbsolutePixelScroll);
    }

    public static void SetVerticalScroll(HHTMLBrowser unBrowserHandle, uint nAbsolutePixelScroll)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamHTMLSurface_SetVerticalScroll(unBrowserHandle, nAbsolutePixelScroll);
    }

    public static void SetKeyFocus(HHTMLBrowser unBrowserHandle, bool bHasKeyFocus)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamHTMLSurface_SetKeyFocus(unBrowserHandle, bHasKeyFocus);
    }

    public static void ViewSource(HHTMLBrowser unBrowserHandle)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamHTMLSurface_ViewSource(unBrowserHandle);
    }

    public static void CopyToClipboard(HHTMLBrowser unBrowserHandle)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamHTMLSurface_CopyToClipboard(unBrowserHandle);
    }

    public static void PasteFromClipboard(HHTMLBrowser unBrowserHandle)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamHTMLSurface_PasteFromClipboard(unBrowserHandle);
    }

    public static void Find(
      HHTMLBrowser unBrowserHandle,
      string pchSearchStr,
      bool bCurrentlyInFind,
      bool bReverse)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchSearchStr1 = new InteropHelp.UTF8StringHandle(pchSearchStr))
        NativeMethods.ISteamHTMLSurface_Find(unBrowserHandle, pchSearchStr1, bCurrentlyInFind, bReverse);
    }

    public static void StopFind(HHTMLBrowser unBrowserHandle)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamHTMLSurface_StopFind(unBrowserHandle);
    }

    public static void GetLinkAtPosition(HHTMLBrowser unBrowserHandle, int x, int y)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamHTMLSurface_GetLinkAtPosition(unBrowserHandle, x, y);
    }

    public static void SetCookie(
      string pchHostname,
      string pchKey,
      string pchValue,
      string pchPath = "/",
      uint nExpires = 0,
      bool bSecure = false,
      bool bHTTPOnly = false)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchHostname1 = new InteropHelp.UTF8StringHandle(pchHostname))
      {
        using (InteropHelp.UTF8StringHandle pchKey1 = new InteropHelp.UTF8StringHandle(pchKey))
        {
          using (InteropHelp.UTF8StringHandle pchValue1 = new InteropHelp.UTF8StringHandle(pchValue))
          {
            using (InteropHelp.UTF8StringHandle pchPath1 = new InteropHelp.UTF8StringHandle(pchPath))
              NativeMethods.ISteamHTMLSurface_SetCookie(pchHostname1, pchKey1, pchValue1, pchPath1, nExpires, bSecure, bHTTPOnly);
          }
        }
      }
    }

    public static void SetPageScaleFactor(
      HHTMLBrowser unBrowserHandle,
      float flZoom,
      int nPointX,
      int nPointY)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamHTMLSurface_SetPageScaleFactor(unBrowserHandle, flZoom, nPointX, nPointY);
    }

    public static void SetBackgroundMode(HHTMLBrowser unBrowserHandle, bool bBackgroundMode)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamHTMLSurface_SetBackgroundMode(unBrowserHandle, bBackgroundMode);
    }

    public static void AllowStartRequest(HHTMLBrowser unBrowserHandle, bool bAllowed)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamHTMLSurface_AllowStartRequest(unBrowserHandle, bAllowed);
    }

    public static void JSDialogResponse(HHTMLBrowser unBrowserHandle, bool bResult)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamHTMLSurface_JSDialogResponse(unBrowserHandle, bResult);
    }

    public static void FileLoadDialogResponse(HHTMLBrowser unBrowserHandle, IntPtr pchSelectedFiles)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamHTMLSurface_FileLoadDialogResponse(unBrowserHandle, pchSelectedFiles);
    }
  }
}
