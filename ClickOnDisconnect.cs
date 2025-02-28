// Decompiled with JetBrains decompiler
// Type: ClickOnDisconnect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using uLink;
using UnityEngine;

#nullable disable
public class ClickOnDisconnect : UnityEngine.MonoBehaviour
{
  private bool disconnected;
  public GameObject onlyIfDisabled;
  public float refreshTime = 1f;

  public void Start()
  {
    this.disconnected = uLink.Network.status == NetworkStatus.Disconnected;
    this.StartCoroutine(this.CheckDisconnected());
  }

  [DebuggerHidden]
  public IEnumerator CheckDisconnected()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new ClickOnDisconnect.\u003CCheckDisconnected\u003Ec__Iterator42()
    {
      \u003C\u003Ef__this = this
    };
  }
}
