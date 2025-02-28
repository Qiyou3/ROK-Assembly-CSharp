// Decompiled with JetBrains decompiler
// Type: UnityTest.TestRunnerConfigurator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using UnityTest.IntegrationTestRunner;

#nullable disable
namespace UnityTest
{
  public class TestRunnerConfigurator
  {
    public static string integrationTestsNetwork = "networkconfig.txt";
    public static string batchRunFileMarker = "batchrun.txt";
    private readonly List<IPEndPoint> m_IPEndPointList = new List<IPEndPoint>();

    public TestRunnerConfigurator()
    {
      this.CheckForBatchMode();
      this.CheckForSendingResultsOverNetwork();
    }

    public bool isBatchRun { get; private set; }

    public bool sendResultsOverNetwork { get; private set; }

    private void CheckForSendingResultsOverNetwork()
    {
      string str1 = !Application.isEditor ? TestRunnerConfigurator.GetTextFromTextAsset(TestRunnerConfigurator.integrationTestsNetwork) : TestRunnerConfigurator.GetTextFromTempFile(TestRunnerConfigurator.integrationTestsNetwork);
      if (str1 == null)
        return;
      this.sendResultsOverNetwork = true;
      this.m_IPEndPointList.Clear();
      string str2 = str1;
      char[] separator = new char[1]{ '\n' };
      foreach (string message in str2.Split(separator, StringSplitOptions.RemoveEmptyEntries))
      {
        int length = message.IndexOf(':');
        string ipString = length != -1 ? message.Substring(0, length) : throw new Exception(message);
        string s = message.Substring(length + 1);
        this.m_IPEndPointList.Add(new IPEndPoint(IPAddress.Parse(ipString), int.Parse(s)));
      }
    }

    private static string GetTextFromTextAsset(string fileName)
    {
      TextAsset textAsset = Resources.Load(fileName.Substring(0, fileName.LastIndexOf('.'))) as TextAsset;
      return (UnityEngine.Object) textAsset != (UnityEngine.Object) null ? textAsset.text : (string) null;
    }

    private static string GetTextFromTempFile(string fileName)
    {
      string textFromTempFile = (string) null;
      try
      {
      }
      catch
      {
        return (string) null;
      }
      return textFromTempFile;
    }

    private void CheckForBatchMode()
    {
      if (TestRunnerConfigurator.GetTextFromTextAsset(TestRunnerConfigurator.batchRunFileMarker) == null)
        return;
      this.isBatchRun = true;
    }

    public static List<string> GetAvailableNetworkIPs()
    {
      if (!NetworkInterface.GetIsNetworkAvailable())
        return new List<string>()
        {
          IPAddress.Loopback.ToString()
        };
      List<UnicastIPAddressInformation> source1 = new List<UnicastIPAddressInformation>();
      List<UnicastIPAddressInformation> source2 = new List<UnicastIPAddressInformation>();
      foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
      {
        if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || networkInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
        {
          IEnumerable<UnicastIPAddressInformation> collection = networkInterface.GetIPProperties().UnicastAddresses.Where<UnicastIPAddressInformation>((Func<UnicastIPAddressInformation, bool>) (a => a.Address.AddressFamily == AddressFamily.InterNetwork));
          source2.AddRange(collection);
          if (networkInterface.OperationalStatus == OperationalStatus.Up)
            source1.AddRange(collection);
        }
      }
      if (!source1.Any<UnicastIPAddressInformation>())
        return source2.Select<UnicastIPAddressInformation, string>((Func<UnicastIPAddressInformation, string>) (i => i.Address.ToString())).ToList<string>();
      source1.Sort((Comparison<UnicastIPAddressInformation>) ((ip1, ip2) =>
      {
        int int32 = BitConverter.ToInt32(((IEnumerable<byte>) ip1.IPv4Mask.GetAddressBytes()).Reverse<byte>().ToArray<byte>(), 0);
        return BitConverter.ToInt32(((IEnumerable<byte>) ip2.IPv4Mask.GetAddressBytes()).Reverse<byte>().ToArray<byte>(), 0).CompareTo(int32);
      }));
      if (source1.Count != 0)
        return source1.Select<UnicastIPAddressInformation, string>((Func<UnicastIPAddressInformation, string>) (i => i.Address.ToString())).ToList<string>();
      return new List<string>()
      {
        IPAddress.Loopback.ToString()
      };
    }

    public ITestRunnerCallback ResolveNetworkConnection()
    {
      List<NetworkResultSender> list = this.m_IPEndPointList.Select<IPEndPoint, NetworkResultSender>((Func<IPEndPoint, NetworkResultSender>) (ipEndPoint => new NetworkResultSender(ipEndPoint.Address.ToString(), ipEndPoint.Port))).ToList<NetworkResultSender>();
      TimeSpan timeSpan = TimeSpan.FromSeconds(30.0);
      DateTime now = DateTime.Now;
      while (DateTime.Now - now < timeSpan)
      {
        foreach (NetworkResultSender networkResultSender in list)
        {
          try
          {
            if (!networkResultSender.Ping())
              continue;
          }
          catch (Exception ex)
          {
            Debug.LogException(ex);
            this.sendResultsOverNetwork = false;
            return (ITestRunnerCallback) null;
          }
          return (ITestRunnerCallback) networkResultSender;
        }
        Thread.Sleep(500);
      }
      Debug.LogError((object) ("Couldn't connect to the server: " + string.Join(", ", this.m_IPEndPointList.Select<IPEndPoint, string>((Func<IPEndPoint, string>) (ipep => ipep.Address.ToString() + ":" + (object) ipep.Port)).ToArray<string>())));
      this.sendResultsOverNetwork = false;
      return (ITestRunnerCallback) null;
    }
  }
}
