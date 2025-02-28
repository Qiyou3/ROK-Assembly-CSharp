// Decompiled with JetBrains decompiler
// Type: Client
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Net;
using uLink;

#nullable disable
public class Client : MonoBehaviour
{
  public string serverIP = "192.168.0.14";

  public void Start()
  {
    int num = (int) Network.Connect(this.serverIP, 7100);
  }

  public void uLink_OnConnectedToServer(IPEndPoint server)
  {
    this.LogInfo<Client>("Connected to server on port {0}", (object) server.Port);
  }
}
