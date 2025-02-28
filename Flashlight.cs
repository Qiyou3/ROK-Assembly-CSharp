// Decompiled with JetBrains decompiler
// Type: Flashlight
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;
using CodeHatch.Networking.Sync;
using UnityEngine;

#nullable disable
[Syncable]
public class Flashlight : EntityBehaviour
{
  [Syncable]
  public bool on;
  public GameObject[] flashlights;
  private bool _lastOn;

  public void Update()
  {
    if (this.on == this._lastOn)
      return;
    this._lastOn = this.on;
    for (int index = 0; index < this.flashlights.Length; ++index)
      this.flashlights[index].SetActive(this.on);
  }

  public void Start()
  {
    SyncManager.Register<Flashlight>(this).SetController(this.Entity.Owner);
    this._lastOn = true;
    this.on = false;
    for (int index = 0; index < this.flashlights.Length; ++index)
      this.flashlights[index].SetActive(this.on);
  }

  public void OnDestroy() => SyncManager.Unregister((object) this);
}
