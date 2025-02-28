// Decompiled with JetBrains decompiler
// Type: DisableWithUnloadedPageManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Networking;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

#nullable disable
public class DisableWithUnloadedPageManager : EntityBehaviour
{
  public float RefreshInterval = 1f;
  private bool _started;

  public void Start()
  {
    if (Player.IsLocalDedi)
    {
      this.LogInfo<DisableWithUnloadedPageManager>("Destroying DisableWithDistance since we are the dedicated server.");
      Object.Destroy((Object) this);
      this.enabled = false;
    }
    else if (this.Entity.IsLocallyOwned)
    {
      this.LogInfo<DisableWithUnloadedPageManager>("Destroying DisableWithDistance since we own this object.");
      Object.Destroy((Object) this);
      this.enabled = false;
    }
    else
    {
      this._started = true;
      this.StartCoroutineWithExceptionHandling(new CoroutineUtil.CoroutineDelegate(this.LODCheckCoroutine));
    }
  }

  public void OnEnable()
  {
    if (!this._started)
      return;
    this.StartCoroutineWithExceptionHandling(new CoroutineUtil.CoroutineDelegate(this.LODCheckCoroutine));
  }

  [DebuggerHidden]
  private IEnumerator LODCheckCoroutine()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new DisableWithUnloadedPageManager.\u003CLODCheckCoroutine\u003Ec__Iterator34()
    {
      \u003C\u003Ef__this = this
    };
  }

  private void OnPageStatusRefresh(bool isInLoadedPage)
  {
    foreach (DisableWithUnloadedPage withUnloadedPage in this.Entity.TryGetArray<DisableWithUnloadedPage>())
      withUnloadedPage.OnPageStatusRefresh(isInLoadedPage);
  }
}
