// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.MonsterHitAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Engine.Core.Cache;
using CodeHatch.Networking.Events;
using CodeHatch.Networking.Events.Entities;
using JetBrains.Annotations;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class MonsterHitAnimation : EntityBehaviour
  {
    public AnimatorObject HitTrigger = (AnimatorObject) "Hit";
    public AnimatorObject HitVariationInt = (AnimatorObject) "Hit Variation";
    public int HitVariations = 1;
    [CanBeNull]
    private Animator _animator;

    [CanBeNull]
    private Animator Animator
    {
      get
      {
        if ((Object) this._animator == (Object) null)
        {
          Entity entity = this.NullCheck<Entity>(this.Entity, "Entity");
          if ((Object) entity == (Object) null)
            return (Animator) null;
          this._animator = entity.Get<Animator>();
        }
        return this._animator;
      }
    }

    public void OnEnable()
    {
      EventManager.Subscribe<EntityDamageEvent>(new EventSubscriber<EntityDamageEvent>(this.OnDamage));
    }

    public void OnDisable()
    {
      EventManager.Unsubscribe<EntityDamageEvent>(new EventSubscriber<EntityDamageEvent>(this.OnDamage));
    }

    public void OnDamage(EntityDamageEvent theEvent)
    {
      if ((Object) this.NullCheck<Entity>(this.Entity, "Entity") == (Object) null || (Object) theEvent.Entity != (Object) this.Entity)
        return;
      Animator animator = this.NullCheck<Animator>(this.Animator, "Animator");
      if ((Object) animator == (Object) null)
        return;
      if (this.HitVariations > 1)
        animator.SetInteger((int) this.HitVariationInt, Random.Range(0, this.HitVariations));
      animator.SetTrigger((int) this.HitTrigger);
    }
  }
}
