// Decompiled with JetBrains decompiler
// Type: DamageReceiver
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.AI;
using CodeHatch.Common;
using CodeHatch.Damaging;
using CodeHatch.Engine.Core.Cache;
using CodeHatch.Networking.Events;
using CodeHatch.Networking.Events.Entities;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DamageReceiver : EntityBehaviour, ICensorable, IDamageReceiver
{
  private List<IDamageTypeModifier> _Modifiers = new List<IDamageTypeModifier>();
  public ParticleEmissionScaler particleEffect;
  public ParticleEmissionScaler censoredParticleEffect;
  public AnimationCurve damageForParticleScaling;
  public float particleTime = 0.5f;
  public float friendlyFireModifier = 1f;
  public float overallParticleScaler = 1f;

  public void OnDamage(Damage damage, DamageSource source = DamageSource.Damager)
  {
    if (damage == null)
    {
      this.LogError<DamageReceiver>("damage == null - DamageReceiver.OnDamage(null, {0}", (object) source);
    }
    else
    {
      Entity entity1 = this.transform.TryGetEntity();
      if (source == DamageSource.Damager)
      {
        Transform transform = damage.Damager.GetTransform();
        if ((Object) transform != (Object) null)
        {
          Entity entity2 = transform.TryGetEntity();
          Targetable targetable1 = !((Object) entity1 != (Object) null) ? UnityObjectUtil.SearchComponent<Targetable>(this.transform) : entity1.TryGet<Targetable>();
          Targetable targetable2 = !((Object) entity2 != (Object) null) ? UnityObjectUtil.SearchComponent<Targetable>(this.transform) : entity2.TryGet<Targetable>();
          if ((Object) targetable1 != (Object) null && (Object) targetable2 != (Object) null && targetable1.TeamColor == targetable2.TeamColor)
            damage.Amount *= this.friendlyFireModifier;
        }
        for (int index = 0; index < this._Modifiers.Count; ++index)
          this._Modifiers[index].ModifyDamage(damage);
        if ((double) damage.Amount > 0.0)
          EventManager.CallEvent((BaseEvent) new EntityDamageEvent(this.Entity, damage, (IDamageReceiver) this));
      }
      Vector3 force = damage.Force + damage.Force;
      if (damage.localForceContext == DamageForceContext.Add && source == DamageSource.Damager || damage.networkedForceContext == DamageForceContext.Add && source == DamageSource.Network)
      {
        Rigidbody rigidbody = !((Object) entity1 != (Object) null) ? UnityObjectUtil.SearchComponent<Rigidbody>(this) : entity1.TryGet<Rigidbody>();
        if ((Object) rigidbody != (Object) null)
          rigidbody.AddForceAtPosition(force, damage.point);
      }
      ParticleEmissionScaler original = !((Object) SingletonMonoBehaviour<Censorer>.Instance != (Object) null) || !SingletonMonoBehaviour<Censorer>.Instance.BloodIsCensored ? this.particleEffect : this.censoredParticleEffect;
      if (!((Object) original != (Object) null))
        return;
      ParticleEmissionScaler particleEmissionScaler = Object.Instantiate((Object) original, damage.point, Quaternion.LookRotation(!(force == Vector3.zero) ? -force : Vector3.up)) as ParticleEmissionScaler;
      if (!((Object) particleEmissionScaler != (Object) null))
        return;
      particleEmissionScaler.transform.parent = this.transform;
      particleEmissionScaler.scaler = this.damageForParticleScaling.Evaluate(damage.Amount) * this.overallParticleScaler;
    }
  }

  public void SetCensored(CensorLayer layer, bool isCensored)
  {
  }

  public void AddSelfToCensorableList() => Censorer.AddCensorable((ICensorable) this);

  public void RemoveSelfFromCensorableList() => Censorer.RemoveCensorable((ICensorable) this);

  public void Start()
  {
    this.AddSelfToCensorableList();
    if (this.HasEntity)
      this._Modifiers.AddRange((IEnumerable<IDamageTypeModifier>) this.Entity.TryGetArray<IDamageTypeModifier>());
    else
      this._Modifiers.AddRange(this.gameObject.GetImplementorsInChildren<IDamageTypeModifier>());
  }

  public void OnDestroy() => this.RemoveSelfFromCensorableList();
}
