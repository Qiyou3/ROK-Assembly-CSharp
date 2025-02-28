// Decompiled with JetBrains decompiler
// Type: DisableWithDistanceManager
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
public class DisableWithDistanceManager : EntityBehaviour
{
  public float RefreshInterval = 1f;

  public void Start()
  {
    if (Player.IsLocalDedi)
    {
      this.LogInfo<DisableWithDistanceManager>("Destroying DisableWithDistance since we are the dedicated server.");
      Object.Destroy((Object) this);
      this.enabled = false;
    }
    else if (this.Entity.IsLocallyOwned)
    {
      this.LogInfo<DisableWithDistanceManager>("Destroying DisableWithDistance since we own this object.");
      Object.Destroy((Object) this);
      this.enabled = false;
    }
    else
      this.StartCoroutineWithExceptionHandling(new CoroutineUtil.CoroutineDelegate(this.LODCheckCoroutine));
  }

  [DebuggerHidden]
  private IEnumerator LODCheckCoroutine()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new DisableWithDistanceManager.\u003CLODCheckCoroutine\u003Ec__Iterator33()
    {
      \u003C\u003Ef__this = this
    };
  }

  private void OnDistanceRefresh(float newDistance)
  {
    foreach (DisableWithDistance disableWithDistance in this.Entity.TryGetArray<DisableWithDistance>())
      disableWithDistance.OnDistanceRefresh(newDistance);
  }
}
