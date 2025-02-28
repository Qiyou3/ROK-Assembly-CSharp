// Decompiled with JetBrains decompiler
// Type: ClickOnConnect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using uLink;

#nullable disable
public class ClickOnConnect : UnityEngine.MonoBehaviour
{
  public ClickOnConnect.TriggerEvent triggerEvent;
  private bool connected;
  public float refreshTime = 1f;

  public void Start()
  {
    this.connected = uLink.Network.status == NetworkStatus.Connected;
    this.StartCoroutine(this.CheckConnected());
  }

  [DebuggerHidden]
  public IEnumerator CheckConnected()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new ClickOnConnect.\u003CCheckConnected\u003Ec__Iterator41()
    {
      \u003C\u003Ef__this = this
    };
  }

  public enum TriggerEvent
  {
    Connect,
    Disconnect,
  }
}
