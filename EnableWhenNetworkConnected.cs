// Decompiled with JetBrains decompiler
// Type: EnableWhenNetworkConnected
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using uLink;
using UnityEngine;

#nullable disable
public class EnableWhenNetworkConnected : UnityEngine.MonoBehaviour
{
  public GameObject[] enableWhenDisconnected;
  public GameObject[] enableWhenConnecting;
  public GameObject[] enableWhenConnected;
  private NetworkStatus status = NetworkStatus.Disconnecting;
  private List<UIButton> _buttonsDisconnected;
  private List<UIButton> _buttonsConnecting;
  private List<UIButton> _buttonsConnected;

  public void Start()
  {
    this._buttonsDisconnected = new List<UIButton>();
    this._buttonsConnecting = new List<UIButton>();
    this._buttonsConnected = new List<UIButton>();
    for (int index = 0; index < this.enableWhenDisconnected.Length; ++index)
      this._buttonsDisconnected.AddRange((IEnumerable<UIButton>) this.enableWhenDisconnected[index].GetComponents<UIButton>());
    for (int index = 0; index < this.enableWhenConnecting.Length; ++index)
      this._buttonsConnecting.AddRange((IEnumerable<UIButton>) this.enableWhenConnecting[index].GetComponents<UIButton>());
    for (int index = 0; index < this.enableWhenConnected.Length; ++index)
      this._buttonsConnected.AddRange((IEnumerable<UIButton>) this.enableWhenConnected[index].GetComponents<UIButton>());
  }

  public void Update()
  {
    if (this.status == uLink.Network.status)
      return;
    if (uLink.Network.status == NetworkStatus.Disconnected)
    {
      for (int index = 0; index < this._buttonsDisconnected.Count; ++index)
        this._buttonsDisconnected[index].isEnabled = true;
      for (int index = 0; index < this._buttonsConnecting.Count; ++index)
        this._buttonsConnecting[index].isEnabled = false;
      for (int index = 0; index < this._buttonsConnected.Count; ++index)
        this._buttonsConnected[index].isEnabled = false;
      LoadingScreen.Instance.End();
    }
    if (uLink.Network.status == NetworkStatus.Connected || uLink.Network.status == NetworkStatus.Connecting)
    {
      for (int index = 0; index < this._buttonsDisconnected.Count; ++index)
        this._buttonsDisconnected[index].isEnabled = false;
      for (int index = 0; index < this._buttonsConnecting.Count; ++index)
        this._buttonsConnecting[index].isEnabled = false;
      for (int index = 0; index < this._buttonsConnected.Count; ++index)
        this._buttonsConnected[index].isEnabled = true;
    }
    this.status = uLink.Network.status;
  }
}
