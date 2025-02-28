// Decompiled with JetBrains decompiler
// Type: ConsoleProRemoteServer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;

#nullable disable
public class ConsoleProRemoteServer : MonoBehaviour
{
  public int port = 51000;
  private static HttpListener listener = new HttpListener();
  private static List<ConsoleProRemoteServer.QueuedLog> logs = new List<ConsoleProRemoteServer.QueuedLog>();

  private void Awake()
  {
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) this.gameObject);
    Debug.Log((object) ("Starting Console Pro Server on port : " + (object) this.port));
    ConsoleProRemoteServer.listener.Prefixes.Add("http://*:" + (object) this.port + "/");
    ConsoleProRemoteServer.listener.Start();
    ConsoleProRemoteServer.listener.BeginGetContext(new AsyncCallback(this.ListenerCallback), (object) null);
  }

  private void OnEnable()
  {
    Application.logMessageReceived += new Application.LogCallback(ConsoleProRemoteServer.LogCallback);
  }

  private void OnDisable()
  {
    Application.logMessageReceived -= new Application.LogCallback(ConsoleProRemoteServer.LogCallback);
  }

  public static void LogCallback(string logString, string stackTrace, UnityEngine.LogType type)
  {
    if (logString.StartsWith("CPIGNORE"))
      return;
    ConsoleProRemoteServer.QueueLog(logString, stackTrace, type);
  }

  private static void QueueLog(string logString, string stackTrace, UnityEngine.LogType type)
  {
    ConsoleProRemoteServer.logs.Add(new ConsoleProRemoteServer.QueuedLog()
    {
      message = logString,
      stackTrace = stackTrace,
      type = type
    });
  }

  private void ListenerCallback(IAsyncResult result)
  {
    this.HandleRequest(new ConsoleProRemoteServer.HTTPContext(ConsoleProRemoteServer.listener.EndGetContext(result)));
    ConsoleProRemoteServer.listener.BeginGetContext(new AsyncCallback(this.ListenerCallback), (object) null);
  }

  private void HandleRequest(ConsoleProRemoteServer.HTTPContext context)
  {
    bool flag = false;
    string command = context.Command;
    if (command != null)
    {
      // ISSUE: reference to a compiler-generated field
      if (ConsoleProRemoteServer.\u003C\u003Ef__switch\u0024map32 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ConsoleProRemoteServer.\u003C\u003Ef__switch\u0024map32 = new Dictionary<string, int>(1)
        {
          {
            "/NewLogs",
            0
          }
        };
      }
      int num;
      // ISSUE: reference to a compiler-generated field
      if (ConsoleProRemoteServer.\u003C\u003Ef__switch\u0024map32.TryGetValue(command, out num) && num == 0)
      {
        flag = true;
        if (ConsoleProRemoteServer.logs.Count > 0)
        {
          string inString = string.Empty;
          foreach (ConsoleProRemoteServer.QueuedLog log in ConsoleProRemoteServer.logs)
          {
            inString = inString + "::::" + (object) log.type;
            inString = inString + "||||" + log.message;
            inString = inString + ">>>>" + log.stackTrace + ">>>>";
          }
          context.RespondWithString(inString);
          ConsoleProRemoteServer.logs.Clear();
        }
      }
    }
    if (!flag)
    {
      context.Response.StatusCode = 404;
      context.Response.StatusDescription = "Not Found";
    }
    context.Response.OutputStream.Close();
  }

  public class HTTPContext
  {
    public HttpListenerContext context;
    public string path;

    public HTTPContext(HttpListenerContext inContext) => this.context = inContext;

    public string Command => WWW.UnEscapeURL(this.context.Request.Url.AbsolutePath);

    public HttpListenerRequest Request => this.context.Request;

    public HttpListenerResponse Response => this.context.Response;

    public void RespondWithString(string inString)
    {
      this.Response.StatusDescription = "OK";
      this.Response.StatusCode = 200;
      if (string.IsNullOrEmpty(inString))
        return;
      this.Response.ContentType = "text/plain";
      byte[] bytes = Encoding.UTF8.GetBytes(inString);
      this.Response.ContentLength64 = (long) bytes.Length;
      this.Response.OutputStream.Write(bytes, 0, bytes.Length);
    }
  }

  public class QueuedLog
  {
    public string message;
    public string stackTrace;
    public UnityEngine.LogType type;
  }
}
