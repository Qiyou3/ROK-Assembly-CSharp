// Decompiled with JetBrains decompiler
// Type: DestroyWhenDead
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Networking;
using CodeHatch.Inventory.Blueprints.Components;
using uLink;
using UnityEngine;

#nullable disable
public class DestroyWhenDead : EntityBehaviour
{
  private IHealth _Health;
  public GameObject DeathParticles;
  public bool NetworkParticles = true;

  public IHealth Health => this._Health ?? (this._Health = this.Entity.Get<IHealth>());

  public void Start()
  {
    BlueprintInstance blueprintInstance = this.Entity.TryGet<BlueprintInstance>();
    if (!((Object) blueprintInstance != (Object) null) || !blueprintInstance.Blueprint.Has<PoolablePrefabBlueprint>())
      return;
    this.enabled = false;
  }

  public void Update()
  {
    if (this.Health == null || !this.Health.IsDead || !Player.IsLocalServer)
      return;
    if (this.NetworkParticles)
      uLink.Network.Instantiate<ulong>(this.DeathParticles, this.transform.position, this.transform.rotation, (NetworkGroup) 0, this.Entity.NetViewID);
    else
      Object.Instantiate((Object) this.DeathParticles, this.transform.position, this.transform.rotation);
    uLink.Network.Destroy(this.gameObject);
  }

  public void OnDespawned()
  {
    if (this.Health == null || !this.Health.IsDead || !Player.IsLocalServer)
      return;
    if (this.NetworkParticles)
      uLink.Network.Instantiate<ulong>(this.DeathParticles, this.transform.position, this.transform.rotation, (NetworkGroup) 0, this.Entity.NetViewID);
    else
      Object.Instantiate((Object) this.DeathParticles, this.transform.position, this.transform.rotation);
  }
}
