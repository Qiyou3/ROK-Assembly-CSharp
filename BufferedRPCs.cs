// Decompiled with JetBrains decompiler
// Type: BufferedRPCs
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using uLink;

#nullable disable
public class BufferedRPCs
{
  private static List<NetworkBufferedRPC> rpcs;

  public static void ExcecuteRPCList()
  {
    Logger.Info<BufferedRPCs>("RPCS EXCECUTING");
    foreach (NetworkBufferedRPC rpc in BufferedRPCs.rpcs)
      rpc.ExecuteNow();
  }

  public static void SaveInstantiateRPCs(NetworkBufferedRPC[] bufferedArray)
  {
    BufferedRPCs.rpcs = new List<NetworkBufferedRPC>();
    Logger.Info<BufferedRPCs>("RPCS RECIEVED");
    foreach (NetworkBufferedRPC buffered in bufferedArray)
    {
      if (buffered.isInstantiate)
      {
        buffered.DontExecuteOnConnected();
        BufferedRPCs.rpcs.Add(buffered);
      }
    }
  }
}
