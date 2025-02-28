// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.PrimalBrain
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using Assets.CodeHatch.RagdollPhysics;
using CodeHatch.Common;
using CodeHatch.Common.Attributes;
using CodeHatch.Damaging;
using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Spawning;
using CodeHatch.ItemContainer;
using CodeHatch.Networking.Sync;
using CodeHatch.RagdollPhysics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class PrimalBrain : uLinkEntityBehaviour
  {
    private const float flinchRatio = 0.25f;
    public static string terrainLayerString = "terrain";
    private static int _terrainLayer;
    private WaypointMotorController motorController;
    private AttackController attackController;
    private LookBridge lookBridge;
    private AnimationController animationController;
    private NpcConfig config;
    private IHealth health;
    private InteractableContainer container;
    public CodeHatch.Common.ObjectPool Pool;
    private List<Transform> avoidanceList = new List<Transform>();
    private List<Transform> discoveredAvoidanceList = new List<Transform>();
    [Range(0.0f, 10f)]
    public float VariationRollerLowerBound = 0.8f;
    [Range(0.0f, 10f)]
    public float VariationRollerUpperBound = 1.2f;
    private float viewAngleVariationFactor = 1f;
    private float viewDistanceVariationFactor = 1f;
    private float maxTargetDistance = 100f;
    public float MaxChaseTime = 60f;
    private float viewConeWidth = 60f;
    private GameClock clock;
    private float attackTime;
    public float AverageAttackTime = 1.25f;
    public float AttackTimeVariation = 1.5f;
    [Range(0.1f, 1000f)]
    public float StateMachineRate = 5f;
    public float FlinchTime = 1f;
    public float DeathTime = 5f;
    [NoEdit]
    public NpcState State;
    [NoEdit]
    private float StateTimer;
    public Targetable MoveTarget;
    public Vector3 IdleMoveTarget;
    public CollisionDamage AttackTarget;
    private Vector3 ClosestPointOnMoveTarget;
    private Vector3 ClosestPointOnAttackTarget;
    public PerformanceTracker PerformanceTracker;
    public Vector3 TargetDistancePenaltyMultiplier = new Vector3(1f, 5f, 1f);
    private NpcState previousState;
    private Targetable myTargetable;
    private bool canAttack;
    private float previousHealth;
    private Vector3 selfPos;
    private Targetable debugLastConsidered;
    private Vector3 debugLastConsideredClosest;
    private bool foundValidTarget;
    private float timeChasing;
    private Erratify[] erratifiers;
    private float timeSinceLastTargettingCycle;
    private bool hasRequiredComponents;
    private float viewConeWidthRatio;

    public DynamicSpawner Spawner { get; set; }

    public float AttackTime
    {
      get
      {
        if ((double) this.attackTime <= 0.0)
          this.attackTime = Mathf.Pow(this.AttackTimeVariation, (float) UnityEngine.Random.Range(-1, 1)) + this.StateTimer;
        return this.attackTime;
      }
    }

    public void ResetAttackTime()
    {
      this.attackTime = Mathf.Pow(this.AttackTimeVariation, (float) UnityEngine.Random.Range(-1, 1)) + this.StateTimer;
    }

    public void OnEnable()
    {
      this.State = NpcState.Spawned;
      this.StartCorutines();
    }

    public void Start()
    {
      this.FetchRequriedComponents();
      PrimalBrain._terrainLayer = LayerMask.NameToLayer(PrimalBrain.terrainLayerString);
    }

    private void FetchRequriedComponents()
    {
      this.motorController = this.Entity.Get<WaypointMotorController>();
      this.lookBridge = this.Entity.GetOrCreate<LookBridge>();
      this.attackController = this.Entity.Get<AttackController>();
      this.animationController = this.Entity.TryGet<AnimationController>();
      this.config = this.Entity.Get<NpcConfig>();
      this.health = this.Entity.Get<IHealth>();
      this.container = this.Entity.Get<InteractableContainer>();
      this.myTargetable = this.Entity.Get<Targetable>();
      this.clock = GameClock.Instance;
      this.erratifiers = this.Entity.TryGetArray<Erratify>();
      this.hasRequiredComponents = true;
    }

    private void RollVariations()
    {
      double num = (double) UnityEngine.Random.Range(this.VariationRollerLowerBound, this.VariationRollerUpperBound);
      this.viewAngleVariationFactor = UnityEngine.Random.Range(this.VariationRollerLowerBound, this.VariationRollerUpperBound);
      this.viewDistanceVariationFactor = UnityEngine.Random.Range(this.VariationRollerLowerBound, this.VariationRollerUpperBound);
    }

    private void StartCorutines()
    {
      this.StartCoroutine(this.StateMachineCoroutine());
      this.StartCoroutine(this.DelayedCoroutine());
    }

    private static Vector3 GetRandomPointInRadius(Vector3 pos, float radius)
    {
      Vector2 vector2 = new Vector2(pos.x, pos.z) + UnityEngine.Random.insideUnitCircle * radius;
      RaycastHit hitInfo;
      Physics.Raycast(new Vector3(vector2.x, 1000000f, vector2.y), -Vector3.up, out hitInfo, float.MaxValue, PrimalBrain._terrainLayer);
      return hitInfo.point;
    }

    private Vector3 TargetLookAtPoint
    {
      get
      {
        return (UnityEngine.Object) this.AttackTarget != (UnityEngine.Object) null ? this.ClosestPointOnAttackTarget : this.ClosestPointOnMoveTarget;
      }
    }

    private void LookAtTarget()
    {
      this.lookBridge.Rotation = Quaternion.LookRotation(this.TargetLookAtPoint - this.Entity.Position);
      this.lookBridge.Strength = 1f;
    }

    private void ConsiderTargetsInTheWay()
    {
      Vector3 vector3 = this.MoveTarget.Position - this.selfPos;
      UnityEngine.Debug.DrawRay(this.Entity.Position, vector3 / vector3.magnitude * 5f, Color.yellow);
    }

    private void CheckForChaseTimeout()
    {
      if (this.foundValidTarget || (double) this.timeChasing <= (double) this.MaxChaseTime)
        return;
      this.MoveTarget = (Targetable) null;
    }

    private void MoveTowardsTarget()
    {
    }

    [DebuggerHidden]
    private IEnumerator DelayedCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new PrimalBrain.\u003CDelayedCoroutine\u003Ec__IteratorC5()
      {
        \u003C\u003Ef__this = this
      };
    }

    private void FilterAvoidanceList()
    {
      this.avoidanceList = new List<Transform>((IEnumerable<Transform>) NpcConfig.NpcTransforms);
      for (int index = this.discoveredAvoidanceList.Count - 1; index >= 0; --index)
      {
        if ((UnityEngine.Object) this.discoveredAvoidanceList[index] == (UnityEngine.Object) null)
          this.discoveredAvoidanceList.RemoveAt(index);
      }
      this.avoidanceList.AddRange((IEnumerable<Transform>) this.discoveredAvoidanceList);
      this.avoidanceList.Remove(this.Entity.MainTransform);
      for (int index = this.avoidanceList.Count - 1; index >= 0; --index)
      {
        if ((double) Vector3.Distance(this.avoidanceList[index].position, this.Entity.Position) > 50.0)
          this.avoidanceList.RemoveAt(index);
      }
    }

    public void FixedUpdate()
    {
      this.StateTimer += Time.fixedDeltaTime;
      this.timeSinceLastTargettingCycle += Time.fixedDeltaTime;
      if (!this.foundValidTarget && (UnityEngine.Object) this.MoveTarget != (UnityEngine.Object) null)
        this.timeChasing += Time.fixedDeltaTime;
      else
        this.timeChasing = 0.0f;
    }

    private void UpdateErratifiers()
    {
      float num = this.clock.CurrentTimeBlock != GameClock.TimeBlock.Night ? 0.2f : 1f;
      for (int index = 0; index < this.erratifiers.Length; ++index)
        this.erratifiers[index].blend = num;
    }

    public void OnDestroy()
    {
      SyncManager.Unregister((object) this);
      NpcConfig.NpcTransforms.Remove(this.Entity.MainTransform);
    }

    [DebuggerHidden]
    public IEnumerator StateMachineCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new PrimalBrain.\u003CStateMachineCoroutine\u003Ec__IteratorC6()
      {
        \u003C\u003Ef__this = this
      };
    }

    private void DetermineCurrentState()
    {
      if (this.State == NpcState.Spawned)
        return;
      if (this.health != null)
      {
        if (this.health.IsDead)
        {
          this.State = NpcState.Dead;
          return;
        }
        if (!this.health.IsDead && this.State == NpcState.Dead)
          this.State = NpcState.Spawned;
      }
      if ((UnityEngine.Object) this.MoveTarget != (UnityEngine.Object) null)
      {
        bool flag1 = (double) (this.ClosestPointOnAttackTarget - this.Entity.Position).sqrMagnitude < (double) this.config.AttackReach * (double) this.config.AttackReach;
        bool flag2 = (double) (this.ClosestPointOnMoveTarget - this.Entity.Position).sqrMagnitude < (double) this.config.AttackReach * (double) this.config.AttackReach;
        this.canAttack = flag1 || flag2;
        this.State = !this.canAttack ? NpcState.Move : NpcState.Attack;
      }
      else if (this.State == NpcState.Move || this.State == NpcState.Attack)
        this.State = NpcState.Idle;
      if (this.health == null)
        return;
      this.previousHealth = this.health.CurrentHealth;
    }

    private void ApplyState()
    {
      switch (this.State)
      {
        case NpcState.Spawned:
          this.SpawnedState();
          break;
        case NpcState.Idle:
          this.IdleState();
          break;
        case NpcState.Move:
          this.MoveState();
          break;
        case NpcState.Flinch:
          this.FlinchState();
          break;
        case NpcState.Dead:
          this.DeadState();
          break;
      }
      this.previousState = this.State;
      if (!this.canAttack || this.State == NpcState.Dead)
        return;
      this.AttackState();
    }

    private void SpawnedState()
    {
      NpcConfig.NpcTransforms.Add(this.Entity.MainTransform);
      this.RollVariations();
      this.State = NpcState.Idle;
      throw new NotImplementedException();
    }

    private void MoveState()
    {
      if (this.previousState != NpcState.Move)
        ;
    }

    private void AttackState()
    {
      if ((double) this.StateTimer >= (double) this.AttackTime && !((UnityEngine.Object) this.MoveTarget == (UnityEngine.Object) null))
        throw new NotImplementedException();
    }

    private void FlinchState()
    {
      if (this.previousState == NpcState.Flinch)
        return;
      if ((UnityEngine.Object) this.animationController != (UnityEngine.Object) null)
        this.animationController.PlayAnimataion(AnimationController.AnimationType.Hit);
      this.StateTimer = 0.0f;
    }

    private void IdleState()
    {
      if ((double) this.StateTimer > 15.0)
      {
        this.IdleMoveTarget = PrimalBrain.GetRandomPointInRadius(this.selfPos, 50f);
        this.StateTimer = 0.0f;
      }
      if (this.previousState != NpcState.Idle)
        ;
    }

    private void DeadState()
    {
      if (this.previousState != NpcState.Dead)
      {
        this.StateTimer = 0.0f;
        this.MoveTarget = (Targetable) null;
        this.AttackTarget = (CollisionDamage) null;
        if ((UnityEngine.Object) this.container != (UnityEngine.Object) null)
          this.container.IsLootable = true;
        if ((UnityEngine.Object) this.Spawner != (UnityEngine.Object) null)
          this.Spawner.NotifyDeath(this.Entity.gameObject);
        if (this.previousState != NpcState.Dead)
          this.RemoveRootTransformFromList();
        throw new NotImplementedException();
      }
    }

    private void RemoveRootTransformFromList()
    {
      NpcConfig.NpcTransforms.Remove(this.Entity.MainTransform);
    }

    private Vector3 DetermineClosestPointOnTarget(Targetable t, Vector3 closestTo)
    {
      return t.ClosestPointOnColliders(closestTo, this.TargetDistancePenaltyMultiplier);
    }

    public void AutoTarget()
    {
      Targetable targetable = this.Entity.Get<Targetable>();
      if (!((UnityEngine.Object) targetable != (UnityEngine.Object) null))
        return;
      this.MoveTarget = targetable.GetHighestPriorityOpponent();
    }
  }
}
