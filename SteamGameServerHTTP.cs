﻿// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamGameServerHTTP
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace Steamworks
{
  public static class SteamGameServerHTTP
  {
    public static HTTPRequestHandle CreateHTTPRequest(
      EHTTPMethod eHTTPRequestMethod,
      string pchAbsoluteURL)
    {
      InteropHelp.TestIfAvailableGameServer();
      using (InteropHelp.UTF8StringHandle pchAbsoluteURL1 = new InteropHelp.UTF8StringHandle(pchAbsoluteURL))
        return (HTTPRequestHandle) NativeMethods.ISteamGameServerHTTP_CreateHTTPRequest(eHTTPRequestMethod, pchAbsoluteURL1);
    }

    public static bool SetHTTPRequestContextValue(HTTPRequestHandle hRequest, ulong ulContextValue)
    {
      InteropHelp.TestIfAvailableGameServer();
      return NativeMethods.ISteamGameServerHTTP_SetHTTPRequestContextValue(hRequest, ulContextValue);
    }

    public static bool SetHTTPRequestNetworkActivityTimeout(
      HTTPRequestHandle hRequest,
      uint unTimeoutSeconds)
    {
      InteropHelp.TestIfAvailableGameServer();
      return NativeMethods.ISteamGameServerHTTP_SetHTTPRequestNetworkActivityTimeout(hRequest, unTimeoutSeconds);
    }

    public static bool SetHTTPRequestHeaderValue(
      HTTPRequestHandle hRequest,
      string pchHeaderName,
      string pchHeaderValue)
    {
      InteropHelp.TestIfAvailableGameServer();
      using (InteropHelp.UTF8StringHandle pchHeaderName1 = new InteropHelp.UTF8StringHandle(pchHeaderName))
      {
        using (InteropHelp.UTF8StringHandle pchHeaderValue1 = new InteropHelp.UTF8StringHandle(pchHeaderValue))
          return NativeMethods.ISteamGameServerHTTP_SetHTTPRequestHeaderValue(hRequest, pchHeaderName1, pchHeaderValue1);
      }
    }

    public static bool SetHTTPRequestGetOrPostParameter(
      HTTPRequestHandle hRequest,
      string pchParamName,
      string pchParamValue)
    {
      InteropHelp.TestIfAvailableGameServer();
      using (InteropHelp.UTF8StringHandle pchParamName1 = new InteropHelp.UTF8StringHandle(pchParamName))
      {
        using (InteropHelp.UTF8StringHandle pchParamValue1 = new InteropHelp.UTF8StringHandle(pchParamValue))
          return NativeMethods.ISteamGameServerHTTP_SetHTTPRequestGetOrPostParameter(hRequest, pchParamName1, pchParamValue1);
      }
    }

    public static bool SendHTTPRequest(HTTPRequestHandle hRequest, out SteamAPICall_t pCallHandle)
    {
      InteropHelp.TestIfAvailableGameServer();
      return NativeMethods.ISteamGameServerHTTP_SendHTTPRequest(hRequest, out pCallHandle);
    }

    public static bool SendHTTPRequestAndStreamResponse(
      HTTPRequestHandle hRequest,
      out SteamAPICall_t pCallHandle)
    {
      InteropHelp.TestIfAvailableGameServer();
      return NativeMethods.ISteamGameServerHTTP_SendHTTPRequestAndStreamResponse(hRequest, out pCallHandle);
    }

    public static bool DeferHTTPRequest(HTTPRequestHandle hRequest)
    {
      InteropHelp.TestIfAvailableGameServer();
      return NativeMethods.ISteamGameServerHTTP_DeferHTTPRequest(hRequest);
    }

    public static bool PrioritizeHTTPRequest(HTTPRequestHandle hRequest)
    {
      InteropHelp.TestIfAvailableGameServer();
      return NativeMethods.ISteamGameServerHTTP_PrioritizeHTTPRequest(hRequest);
    }

    public static bool GetHTTPResponseHeaderSize(
      HTTPRequestHandle hRequest,
      string pchHeaderName,
      out uint unResponseHeaderSize)
    {
      InteropHelp.TestIfAvailableGameServer();
      using (InteropHelp.UTF8StringHandle pchHeaderName1 = new InteropHelp.UTF8StringHandle(pchHeaderName))
        return NativeMethods.ISteamGameServerHTTP_GetHTTPResponseHeaderSize(hRequest, pchHeaderName1, out unResponseHeaderSize);
    }

    public static bool GetHTTPResponseHeaderValue(
      HTTPRequestHandle hRequest,
      string pchHeaderName,
      byte[] pHeaderValueBuffer,
      uint unBufferSize)
    {
      InteropHelp.TestIfAvailableGameServer();
      using (InteropHelp.UTF8StringHandle pchHeaderName1 = new InteropHelp.UTF8StringHandle(pchHeaderName))
        return NativeMethods.ISteamGameServerHTTP_GetHTTPResponseHeaderValue(hRequest, pchHeaderName1, pHeaderValueBuffer, unBufferSize);
    }

    public static bool GetHTTPResponseBodySize(HTTPRequestHandle hRequest, out uint unBodySize)
    {
      InteropHelp.TestIfAvailableGameServer();
      return NativeMethods.ISteamGameServerHTTP_GetHTTPResponseBodySize(hRequest, out unBodySize);
    }

    public static bool GetHTTPResponseBodyData(
      HTTPRequestHandle hRequest,
      byte[] pBodyDataBuffer,
      uint unBufferSize)
    {
      InteropHelp.TestIfAvailableGameServer();
      return NativeMethods.ISteamGameServerHTTP_GetHTTPResponseBodyData(hRequest, pBodyDataBuffer, unBufferSize);
    }

    public static bool GetHTTPStreamingResponseBodyData(
      HTTPRequestHandle hRequest,
      uint cOffset,
      byte[] pBodyDataBuffer,
      uint unBufferSize)
    {
      InteropHelp.TestIfAvailableGameServer();
      return NativeMethods.ISteamGameServerHTTP_GetHTTPStreamingResponseBodyData(hRequest, cOffset, pBodyDataBuffer, unBufferSize);
    }

    public static bool ReleaseHTTPRequest(HTTPRequestHandle hRequest)
    {
      InteropHelp.TestIfAvailableGameServer();
      return NativeMethods.ISteamGameServerHTTP_ReleaseHTTPRequest(hRequest);
    }

    public static bool GetHTTPDownloadProgressPct(
      HTTPRequestHandle hRequest,
      out float pflPercentOut)
    {
      InteropHelp.TestIfAvailableGameServer();
      return NativeMethods.ISteamGameServerHTTP_GetHTTPDownloadProgressPct(hRequest, out pflPercentOut);
    }

    public static bool SetHTTPRequestRawPostBody(
      HTTPRequestHandle hRequest,
      string pchContentType,
      byte[] pubBody,
      uint unBodyLen)
    {
      InteropHelp.TestIfAvailableGameServer();
      using (InteropHelp.UTF8StringHandle pchContentType1 = new InteropHelp.UTF8StringHandle(pchContentType))
        return NativeMethods.ISteamGameServerHTTP_SetHTTPRequestRawPostBody(hRequest, pchContentType1, pubBody, unBodyLen);
    }

    public static HTTPCookieContainerHandle CreateCookieContainer(bool bAllowResponsesToModify)
    {
      InteropHelp.TestIfAvailableGameServer();
      return (HTTPCookieContainerHandle) NativeMethods.ISteamGameServerHTTP_CreateCookieContainer(bAllowResponsesToModify);
    }

    public static bool ReleaseCookieContainer(HTTPCookieContainerHandle hCookieContainer)
    {
      InteropHelp.TestIfAvailableGameServer();
      return NativeMethods.ISteamGameServerHTTP_ReleaseCookieContainer(hCookieContainer);
    }

    public static bool SetCookie(
      HTTPCookieContainerHandle hCookieContainer,
      string pchHost,
      string pchUrl,
      string pchCookie)
    {
      InteropHelp.TestIfAvailableGameServer();
      using (InteropHelp.UTF8StringHandle pchHost1 = new InteropHelp.UTF8StringHandle(pchHost))
      {
        using (InteropHelp.UTF8StringHandle pchUrl1 = new InteropHelp.UTF8StringHandle(pchUrl))
        {
          using (InteropHelp.UTF8StringHandle pchCookie1 = new InteropHelp.UTF8StringHandle(pchCookie))
            return NativeMethods.ISteamGameServerHTTP_SetCookie(hCookieContainer, pchHost1, pchUrl1, pchCookie1);
        }
      }
    }

    public static bool SetHTTPRequestCookieContainer(
      HTTPRequestHandle hRequest,
      HTTPCookieContainerHandle hCookieContainer)
    {
      InteropHelp.TestIfAvailableGameServer();
      return NativeMethods.ISteamGameServerHTTP_SetHTTPRequestCookieContainer(hRequest, hCookieContainer);
    }

    public static bool SetHTTPRequestUserAgentInfo(
      HTTPRequestHandle hRequest,
      string pchUserAgentInfo)
    {
      InteropHelp.TestIfAvailableGameServer();
      using (InteropHelp.UTF8StringHandle pchUserAgentInfo1 = new InteropHelp.UTF8StringHandle(pchUserAgentInfo))
        return NativeMethods.ISteamGameServerHTTP_SetHTTPRequestUserAgentInfo(hRequest, pchUserAgentInfo1);
    }

    public static bool SetHTTPRequestRequiresVerifiedCertificate(
      HTTPRequestHandle hRequest,
      bool bRequireVerifiedCertificate)
    {
      InteropHelp.TestIfAvailableGameServer();
      return NativeMethods.ISteamGameServerHTTP_SetHTTPRequestRequiresVerifiedCertificate(hRequest, bRequireVerifiedCertificate);
    }

    public static bool SetHTTPRequestAbsoluteTimeoutMS(
      HTTPRequestHandle hRequest,
      uint unMilliseconds)
    {
      InteropHelp.TestIfAvailableGameServer();
      return NativeMethods.ISteamGameServerHTTP_SetHTTPRequestAbsoluteTimeoutMS(hRequest, unMilliseconds);
    }

    public static bool GetHTTPRequestWasTimedOut(HTTPRequestHandle hRequest, out bool pbWasTimedOut)
    {
      InteropHelp.TestIfAvailableGameServer();
      return NativeMethods.ISteamGameServerHTTP_GetHTTPRequestWasTimedOut(hRequest, out pbWasTimedOut);
    }
  }
}
