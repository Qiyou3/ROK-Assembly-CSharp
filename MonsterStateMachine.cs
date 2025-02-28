// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.MonsterStateMachine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class MonsterStateMachine : StateMachineBase<MonsterState>
  {
    public bool attackOnReact;
    public bool useAttackState = true;
    private Targeter _targeter;
    private ReactOnDamage _reactOnDamage;

    private Targeter MyTargeter
    {
      get
      {
        if ((Object) this._targeter == (Object) null)
          this._targeter = this.Entity.TryGet<Targeter>();
        return this._targeter;
      }
    }

    private ReactOnDamage React
    {
      get
      {
        if ((Object) this._reactOnDamage == (Object) null)
          this._reactOnDamage = this.Entity.TryGet<ReactOnDamage>();
        return this._reactOnDamage;
      }
    }

    public void Update()
    {
      if ((Object) this.MyTargeter == (Object) null)
        this.state = MonsterState.Wander;
      else if ((Object) this.CurrentTarget == (Object) null)
        this.state = MonsterState.Wander;
      else if (this.useAttackState)
        this.state = MonsterState.Attack;
      if (this.state == MonsterState.Wander && (Object) this.React != (Object) null && this.React.Location != null)
      {
        if (this.attackOnReact)
          this.state = MonsterState.Attack;
        else
          this.state = MonsterState.React;
      }
      this.BroadcastStateChanges();
    }
  }
}
