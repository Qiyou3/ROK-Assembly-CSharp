// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.FlyingBrain
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Core.Utility;
using CodeHatch.ItemContainer;
using CodeHatch.RagdollPhysics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class FlyingBrain : EntityBehaviour
  {
    public string NoticeSFX = "eyeball_focus";
    public string TookDamageSFX = "eyeball_hurt1";
    public string DeathSFX = "eyeball_death";
    public float EnemyTargetOffset = 10f;
    public float UpdatePathsDelay = 1.5f;
    public PerformanceTracker performanceTracker;
    private readonly ChangeDetector<bool> _isAlive = new ChangeDetector<bool>();
    private readonly ChangeDetector<float> _healthAmount = new ChangeDetector<float>();
    private readonly ChangeDetector<bool> _isEnemyVisible = new ChangeDetector<bool>();
    private PathfindingComponentBase _pathfinder;
    private Laser _laser;
    private Health _health;
    private InteractableContainer _container;
    private LookBridge _lookBridge;
    private WaypointMotorController _motorController;
    private HiveNode _hiveNode;
    private readonly List<TargetPath> _paths = new List<TargetPath>();

    public void Start()
    {
      this._lookBridge = this.Entity.GetOrCreate<LookBridge>();
      this._motorController = this.Entity.Get<WaypointMotorController>();
      this._hiveNode = this.Entity.Get<HiveNode>();
      this._pathfinder = this.Entity.TryGet<PathfindingComponentBase>();
      this._laser = this.Entity.TryGet<Laser>();
      this._health = this.Entity.TryGet<Health>();
      this._container = this.Entity.TryGet<InteractableContainer>();
      this.SetAlive();
    }

    public void OnDrawGizmos()
    {
    }

    [DebuggerHidden]
    public IEnumerator UpdatePathsCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new FlyingBrain.\u003CUpdatePathsCoroutine\u003Ec__IteratorC3()
      {
        \u003C\u003Ef__this = this
      };
    }

    [DebuggerHidden]
    public IEnumerator UpdateCurrentPathCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new FlyingBrain.\u003CUpdateCurrentPathCoroutine\u003Ec__IteratorC4()
      {
        \u003C\u003Ef__this = this
      };
    }

    public void FixedUpdate()
    {
      if ((bool) (UnityEngine.Object) this._health)
      {
        this._isAlive.Value = this._health.IsAlive;
        this._healthAmount.Value = this._health.CurrentHealth;
      }
      else
      {
        this._isAlive.Value = true;
        this._healthAmount.Value = float.MaxValue;
      }
      this._isEnemyVisible.Value = (bool) (UnityEngine.Object) this._hiveNode && this._hiveNode.IsTargetDirectEnemy;
      if (this._isAlive.Changed)
        this.ChangeLifeState((bool) this._isAlive);
      if (!(bool) this._isAlive)
        return;
      this.UpdateLook();
      this.UpdateApproach();
      this.UpdateAudio();
    }

    private void UpdateLook()
    {
      if ((UnityEngine.Object) this._lookBridge == (UnityEngine.Object) null)
        return;
      if ((UnityEngine.Object) this._hiveNode == (UnityEngine.Object) null)
        this._lookBridge.Strength = 0.0f;
      else if ((UnityEngine.Object) this._hiveNode.Target != (UnityEngine.Object) null)
      {
        this._lookBridge.Rotation = Quaternion.LookRotation(this._hiveNode.Target.Position - this.Entity.Position);
        this._lookBridge.Strength = 1f;
      }
      else
        this._lookBridge.Strength = 0.0f;
    }

    private void UpdateApproach()
    {
      if ((bool) this._isEnemyVisible)
        this.ApproachWaypointWithSomeDistance();
      else
        this.ApproachWaypointDirectly();
    }

    private void UpdateAudio()
    {
      if (this._isEnemyVisible.Became(true))
        AudioController.PlayIfNotPlaying(this.NoticeSFX, this.Entity.MainTransform, 1f, 0.0f, 0.0f);
      if ((double) (float) this._healthAmount >= (double) this._healthAmount.PreviousValue)
        return;
      AudioController.PlayIfNotPlaying(this.TookDamageSFX, this.Entity.MainTransform, 1f, 0.0f, 0.0f);
    }

    private void ApproachWaypointDirectly()
    {
      if (!((UnityEngine.Object) this._motorController == (UnityEngine.Object) null))
        throw new NotImplementedException();
    }

    private void ApproachWaypointWithSomeDistance()
    {
      if (!((UnityEngine.Object) this._motorController == (UnityEngine.Object) null))
        throw new NotImplementedException();
    }

    private void ChangeLifeState(bool isAlive)
    {
      if (isAlive)
        this.SetAlive();
      else
        this.SetDead();
    }

    private void SetDead()
    {
      if ((bool) (UnityEngine.Object) this._lookBridge)
        this._lookBridge.Strength = 0.0f;
      if ((UnityEngine.Object) this._container != (UnityEngine.Object) null)
        this._container.IsLootable = true;
      throw new NotImplementedException();
    }

    private void SetAlive()
    {
      if ((bool) (UnityEngine.Object) this._lookBridge)
        this._lookBridge.Strength = 1f;
      if ((bool) (UnityEngine.Object) this._container)
        this._container.IsLootable = false;
      throw new NotImplementedException();
    }
  }
}
