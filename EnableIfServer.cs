// Decompiled with JetBrains decompiler
// Type: EnableIfServer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Networking;
using uLink;
using UnityEngine;

#nullable disable
public class EnableIfServer : UnityEngine.MonoBehaviour
{
  public GameObject[] enableIfServer;
  public GameObject[] enableIfClient;
  public GameObject[] enableIfDisconnected;
  private NetworkStatus status;
  private bool isServer;
  public float refreshTime = 1f;
  private Coroutine checkCoroutine;

  public void Start()
  {
    this.status = uLink.Network.status;
    this.isServer = Player.IsLocalServer;
  }

  public void Update() => this.CheckRefresh();

  public void CheckRefresh()
  {
    this.status = uLink.Network.status;
    this.isServer = Player.IsLocalServer;
    this.Refresh();
  }

  private void Refresh()
  {
    switch (this.status)
    {
      case NetworkStatus.Disconnected:
        foreach (GameObject gameObject in this.enableIfDisconnected)
        {
          if ((Object) gameObject != (Object) null)
            gameObject.SetActive(true);
        }
        foreach (GameObject gameObject in this.enableIfServer)
        {
          if ((Object) gameObject != (Object) null)
            gameObject.SetActive(false);
        }
        foreach (GameObject gameObject in this.enableIfClient)
        {
          if ((Object) gameObject != (Object) null)
            gameObject.SetActive(false);
        }
        break;
      case NetworkStatus.Connected:
        foreach (GameObject gameObject in this.enableIfDisconnected)
        {
          if ((Object) gameObject != (Object) null)
            gameObject.SetActive(false);
        }
        if (this.isServer)
        {
          foreach (GameObject gameObject in this.enableIfServer)
          {
            if ((Object) gameObject != (Object) null)
              gameObject.SetActive(true);
          }
          foreach (GameObject gameObject in this.enableIfClient)
          {
            if ((Object) gameObject != (Object) null)
              gameObject.SetActive(false);
          }
          break;
        }
        foreach (GameObject gameObject in this.enableIfClient)
        {
          if ((Object) gameObject != (Object) null)
            gameObject.SetActive(true);
        }
        foreach (GameObject gameObject in this.enableIfServer)
        {
          if ((Object) gameObject != (Object) null)
            gameObject.SetActive(false);
        }
        break;
    }
  }
}
