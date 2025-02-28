// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.MonsterDeathAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Engine.Core.Cache;
using CodeHatch.Networking.Events;
using CodeHatch.Networking.Events.Entities.Enemy;
using JetBrains.Annotations;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class MonsterDeathAnimation : EntityBehaviour
  {
    public AnimatorObject DeathTrigger = (AnimatorObject) "Death";
    public AnimatorObject DeathVariationInt = (AnimatorObject) "Death Variation";
    public int DeathVariations = 1;
    [CanBeNull]
    private Animator _animator;
    private bool _recievedDeathEvent;
    private bool _started;

    private bool IsDead
    {
      get
      {
        if (!this._started)
          return false;
        if (this._recievedDeathEvent)
          return true;
        Entity entity = this.NullCheck<Entity>(this.Entity, "Entity");
        if ((Object) entity == (Object) null)
          return false;
        IHealth health = entity.TryGet<IHealth>();
        return health != null && health.IsDead;
      }
    }

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

    public void Awake()
    {
      EventManager.Subscribe<EnemyDeathEvent>(new EventSubscriber<EnemyDeathEvent>(this.OnEnemyDeath));
    }

    public void OnDestroy()
    {
      EventManager.Unsubscribe<EnemyDeathEvent>(new EventSubscriber<EnemyDeathEvent>(this.OnEnemyDeath));
    }

    public void Start() => this._started = true;

    public void OnEnemyDeath(EnemyDeathEvent theEvent)
    {
      if (!this._started || (Object) this.NullCheck<Entity>(this.Entity, "Entity") == (Object) null || (Object) theEvent.Entity != (Object) this.Entity || this._recievedDeathEvent)
        return;
      this._recievedDeathEvent = true;
      Animator animator = this.NullCheck<Animator>(this.Animator, "Animator");
      if ((Object) animator == (Object) null)
        return;
      if (this.DeathVariations > 1)
        animator.SetInteger((int) this.DeathVariationInt, Random.Range(0, this.DeathVariations));
      animator.SetTrigger((int) this.DeathTrigger);
    }

    public void OnEnable()
    {
      if (!this._started || !this.IsDead)
        return;
      Animator animator = this.NullCheck<Animator>(this.Animator, "Animator");
      if ((Object) animator == (Object) null)
        return;
      if (this.DeathVariations > 1)
        animator.SetInteger((int) this.DeathVariationInt, Random.Range(0, this.DeathVariations));
      animator.SetTrigger((int) this.DeathTrigger);
    }
  }
}
