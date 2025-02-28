// Decompiled with JetBrains decompiler
// Type: EnableIfConnected
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using uLink;
using UnityEngine;

#nullable disable
public class EnableIfConnected : UnityEngine.MonoBehaviour
{
  public GameObject[] enableIfDisconnected;
  public GameObject[] enableIfConnected;
  private NetworkStatus status;
  public float refreshTime = 1f;

  public void Start()
  {
    this.status = uLink.Network.status;
    this.StartCoroutine(this.CheckConnected());
  }

  public void OnEnable() => this.Refresh();

  [DebuggerHidden]
  public IEnumerator CheckConnected()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new EnableIfConnected.\u003CCheckConnected\u003Ec__Iterator43()
    {
      \u003C\u003Ef__this = this
    };
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
        foreach (GameObject gameObject in this.enableIfConnected)
        {
          if ((Object) gameObject != (Object) null)
            gameObject.SetActive(false);
        }
        break;
      case NetworkStatus.Connected:
        foreach (GameObject gameObject in this.enableIfConnected)
        {
          if ((Object) gameObject != (Object) null)
            gameObject.SetActive(true);
        }
        foreach (GameObject gameObject in this.enableIfDisconnected)
        {
          if ((Object) gameObject != (Object) null)
            gameObject.SetActive(false);
        }
        break;
    }
  }
}
