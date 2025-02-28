// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.JellyPuffDeathExplode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;
using CodeHatch.Networking.Events;
using CodeHatch.Networking.Events.Entities.Enemy;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class JellyPuffDeathExplode : EntityBehaviour
  {
    public Animator animator;
    public string deathAnimation;
    public string explodeAnimation;
    private bool hasAttacked;
    private bool canAttack;
    private bool canDisable;

    public void OnEnable()
    {
      EventManager.Subscribe<EnemyDeathEvent>(new EventSubscriber<EnemyDeathEvent>(this.OnDeath));
    }

    public void OnDisable()
    {
      EventManager.Unsubscribe<EnemyDeathEvent>(new EventSubscriber<EnemyDeathEvent>(this.OnDeath));
    }

    public void OnDeath(EnemyDeathEvent e)
    {
      if (!((Object) e.Entity == (Object) this.Entity))
        return;
      this.animator.CrossFade(this.explodeAnimation, 1f);
      this.canAttack = true;
    }

    public void Update()
    {
      if (this.canAttack)
      {
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName(this.explodeAnimation))
          this.hasAttacked = true;
        if (this.hasAttacked)
        {
          EventManager.CallEvent((BaseEvent) new EnemyExplodeEvent(this.Entity));
          this.animator.CrossFade(this.deathAnimation, 1f);
          this.canAttack = false;
        }
      }
      if (this.animator.GetCurrentAnimatorStateInfo(0).IsName(this.deathAnimation))
      {
        this.canDisable = true;
      }
      else
      {
        if (!this.canDisable)
          return;
        this.animator.runtimeAnimatorController = (RuntimeAnimatorController) null;
      }
    }
  }
}
