// Decompiled with JetBrains decompiler
// Type: DamageIndicatorManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Damaging;
using CodeHatch.Engine.Core.Cache;
using CodeHatch.Networking.Events;
using CodeHatch.Networking.Events.Entities;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class DamageIndicatorManager : EntityBehaviour
{
  public DamageIndicator damageIndicatorPrefab;
  public int maxDamageIndicators = 3;
  private List<DamageIndicator> _damageIndicators = new List<DamageIndicator>();

  public void Start()
  {
    EventManager.Subscribe<EntityDamageEvent>(new EventSubscriber<EntityDamageEvent>(this.OnEntityDamage));
  }

  public void OnDestroy()
  {
    EventManager.Unsubscribe<EntityDamageEvent>(new EventSubscriber<EntityDamageEvent>(this.OnEntityDamage));
  }

  private void OnEntityDamage(EntityDamageEvent e)
  {
    if (e.Cancelled || (UnityEngine.Object) e.Entity != (UnityEngine.Object) this.Entity || e.Damage.DamageTypes == DamageType.Healing || (double) e.Damage.Amount <= 0.0)
      return;
    Damage damage = e.Damage;
    if (this._damageIndicators.Count < this.maxDamageIndicators)
    {
      DamageIndicator component = UnityEngine.Object.Instantiate<DamageIndicator>(this.damageIndicatorPrefab).GetTransform().GetComponent<DamageIndicator>();
      this._damageIndicators.Add(component);
      component.damage = damage.Amount;
      component.damageDirection = damage.Damager.GetTransform();
      component.relativePoint = !((UnityEngine.Object) component.damageDirection != (UnityEngine.Object) null) ? Vector3.zero : component.damageDirection.InverseTransformPoint(damage.point);
      component.damageDirectionVec = damage.point;
      component.type = DamageIndicator.Type.Point;
      this.LogInfo<DamageIndicatorManager>("Added damage indicator.");
    }
    else
    {
      int index = this._damageIndicators.IndexOf<DamageIndicator>((Func<DamageIndicator, bool>) (i => (UnityEngine.Object) i == (UnityEngine.Object) null || (double) i.damage <= (double) damage.Amount));
      if (index == -1)
        return;
      DamageIndicator damageIndicator = this._damageIndicators[index];
      if ((UnityEngine.Object) damageIndicator == (UnityEngine.Object) null)
      {
        DamageIndicator component = UnityEngine.Object.Instantiate<DamageIndicator>(this.damageIndicatorPrefab).GetTransform().GetComponent<DamageIndicator>();
        this._damageIndicators[index] = component;
        component.damage = damage.Amount;
        component.damageDirection = damage.Damager.GetTransform();
        component.relativePoint = !((UnityEngine.Object) component.damageDirection != (UnityEngine.Object) null) ? Vector3.zero : component.damageDirection.InverseTransformPoint(damage.point);
        component.damageDirectionVec = damage.point;
        component.type = DamageIndicator.Type.Point;
      }
      else
      {
        damageIndicator.damage = damage.Amount;
        damageIndicator.damageDirection = damage.Damager.GetTransform();
        damageIndicator.relativePoint = !((UnityEngine.Object) damageIndicator.damageDirection != (UnityEngine.Object) null) ? Vector3.zero : damageIndicator.damageDirection.InverseTransformPoint(damage.point);
        damageIndicator.damageDirectionVec = damage.point;
        damageIndicator.type = DamageIndicator.Type.Point;
      }
    }
  }
}
