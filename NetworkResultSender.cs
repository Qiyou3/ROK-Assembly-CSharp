// Decompiled with JetBrains decompiler
// Type: UnityTest.NetworkResultSender
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;
using UnityTest.IntegrationTestRunner;

#nullable disable
namespace UnityTest
{
  public class NetworkResultSender : ITestRunnerCallback
  {
    private readonly TimeSpan m_ConnectionTimeout = TimeSpan.FromSeconds(5.0);
    private readonly string m_Ip;
    private readonly int m_Port;
    private bool m_LostConnection;

    public NetworkResultSender(string ip, int port)
    {
      this.m_Ip = ip;
      this.m_Port = port;
    }

    private bool SendDTO(ResultDTO dto)
    {
      if (this.m_LostConnection)
        return false;
      try
      {
        using (TcpClient tcpClient = new TcpClient())
        {
          IAsyncResult asyncResult = tcpClient.BeginConnect(this.m_Ip, this.m_Port, (AsyncCallback) null, (object) null);
          if (!asyncResult.AsyncWaitHandle.WaitOne(this.m_ConnectionTimeout))
            return false;
          try
          {
            tcpClient.EndConnect(asyncResult);
          }
          catch (SocketException ex)
          {
            this.m_LostConnection = true;
            return false;
          }
          new DTOFormatter().Serialize((Stream) tcpClient.GetStream(), dto);
          tcpClient.GetStream().Close();
          tcpClient.Close();
          Debug.Log((object) ("Sent " + (object) dto.messageType));
        }
      }
      catch (SocketException ex)
      {
        Debug.LogException((Exception) ex);
        this.m_LostConnection = true;
        return false;
      }
      return true;
    }

    public bool Ping()
    {
      bool flag = this.SendDTO(ResultDTO.CreatePing());
      this.m_LostConnection = false;
      return flag;
    }

    public void RunStarted(string platform, List<TestComponent> testsToRun)
    {
      this.SendDTO(ResultDTO.CreateRunStarted());
    }

    public void RunFinished(List<TestResult> testResults)
    {
      this.SendDTO(ResultDTO.CreateRunFinished(testResults));
    }

    public void TestStarted(TestResult test) => this.SendDTO(ResultDTO.CreateTestStarted(test));

    public void TestFinished(TestResult test) => this.SendDTO(ResultDTO.CreateTestFinished(test));

    public void TestRunInterrupted(List<ITestComponent> testsNotRun)
    {
      this.RunFinished(new List<TestResult>());
    }
  }
}
