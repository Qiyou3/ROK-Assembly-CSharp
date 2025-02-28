// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.MonsterMotor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Modding.Abstract;
using CodeHatch.Tracing;
using JetBrains.Annotations;
using SmartAssembly.Attributes;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class MonsterMotor : EntityBehaviour, IModable
  {
    private const float _gripThreshold = 0.01f;
    [SerializeField]
    [CodeHatch.Engine.Core.Utility.Attributes.CanBeNull]
    [DoNotObfuscate]
    private Transform _axisProvider;
    [SerializeField]
    [CodeHatch.Engine.Core.Utility.Attributes.CanBeNull]
    [DoNotObfuscate]
    private Transform _tracePositionTransform;
    [DoNotObfuscate]
    [CodeHatch.Engine.Core.Utility.Attributes.CanBeNull]
    [SerializeField]
    private Transform _thrustPoint;
    [SerializeField]
    [DoNotObfuscate]
    [CodeHatch.Engine.Core.Utility.Attributes.CanBeNull]
    private Transform _restPoint;
    [UsedImplicitly]
    [SerializeField]
    [DoNotObfuscate]
    [CodeHatch.Engine.Core.Utility.Attributes.CanBeNull]
    private Transform _centerOfMass;
    private TracerIgnoreParams _monsterIgnoreParams;
    [JetBrains.Annotations.CanBeNull]
    private LookBridge _lookBridge;
    [JetBrains.Annotations.CanBeNull]
    private MotorBridge _motorBridge;
    [JetBrains.Annotations.CanBeNull]
    private Animator _animator;
    [JetBrains.Annotations.CanBeNull]
    private Rigidbody _mainRigidbody;
    public float MaximumVelocityBase = 10f;
    public float MaximumVelocityRemoteMultiplier = 1f;
    public bool UseVelocityMultiplierParameter;
    public AnimatorObject MaximumVelocityMultiplierFloat = (AnimatorObject) "Velocity Limit Multiplier";
    public BalanceConstraint BalanceConstraint;
    public AngularConstraint LookConstraint;
    public VelocityConstraint ThrustConstraint;
    public DamperConstraint RestConstraint;
    public MonsterTerrainTracer TerrainTracer;
    public float GroundNormalSmoothingTime = 0.2f;
    public AnimationCurve SlopeDotToGrip = new AnimationCurve(new Keyframe[2]
    {
      new Keyframe(0.7f, 0.0f),
      new Keyframe(0.9f, 1f)
    });
    public bool GripAffectsTorque;
    public float GripDecayTime = 0.2f;
    private Vector3 _groundNormal = Vector3.up;
    private float _grip = 1f;
    private Vector3 _groundProjectedForward = Vector3.forward;
    private Vector3 _groundProjectedRight = Vector3.right;
    public bool WalkOnLowHealth;
    public float WalkHealthPercentage = 0.3f;
    public float SpeedDecreaseMultiplier = 0.25f;
    private Quaternion _rotation = Quaternion.identity;
    private Vector3 _up = Vector3.up;
    private Vector3 _forward = Vector3.forward;
    private Vector3 _right = Vector3.right;
    private Vector3 _velocityAtThrustPoint = Vector3.zero;
    private Vector3 _velocityAtRestPoint = Vector3.zero;
    private Vector3 _targetVelocity = Vector3.zero;
    private Vector3 _angularVelocity = Vector3.zero;

    private Transform AxisProvider
    {
      get
      {
        if (!((Object) this._axisProvider == (Object) null))
          return this._axisProvider;
        Rigidbody rigidbody = this.NullCheck<Rigidbody>(this.MainRigidbody, "MainRigidbody");
        if ((Object) rigidbody != (Object) null)
        {
          this._axisProvider = rigidbody.transform;
          return this._axisProvider;
        }
        Entity entity = this.NullCheck<Entity>(this.Entity, "Entity");
        if ((Object) entity != (Object) null)
        {
          this._axisProvider = entity.MainTransform;
          return this._axisProvider;
        }
        this._axisProvider = this.transform;
        return this._axisProvider;
      }
    }

    private Vector3 TracePosition
    {
      get
      {
        if ((Object) this._tracePositionTransform == (Object) null)
        {
          this._tracePositionTransform = new GameObject("Trace Position").transform;
          this._tracePositionTransform.transform.parent = this.MainRigidbody.transform;
          this._tracePositionTransform.localPosition = this.MainRigidbody.centerOfMass;
        }
        return this._tracePositionTransform.position;
      }
    }

    private Vector3 ThrustPoint
    {
      get
      {
        if ((Object) this._thrustPoint == (Object) null)
        {
          this._thrustPoint = new GameObject("Thrust Position").transform;
          this._thrustPoint.transform.parent = this.MainRigidbody.transform;
          Vector3 worldCenterOfMass = this.WorldCenterOfMass;
          Vector3 worldPoint = worldCenterOfMass + this.AxisProvider.rotation * new Vector3(0.0f, 0.0f, 10000f);
          this._thrustPoint.position = Vector3.Lerp(worldCenterOfMass, this.MainRigidbody.ClosestPointOnColliders(worldPoint), 0.333f);
        }
        return this._thrustPoint.position;
      }
    }

    private Vector3 RestPoint
    {
      get
      {
        if ((Object) this._restPoint == (Object) null)
        {
          this._restPoint = new GameObject("Rest Position").transform;
          this._restPoint.transform.parent = this.MainRigidbody.transform;
          Vector3 worldCenterOfMass = this.WorldCenterOfMass;
          Vector3 worldPoint = worldCenterOfMass + this.AxisProvider.rotation * new Vector3(0.0f, 0.0f, -10000f);
          this._restPoint.position = Vector3.Lerp(worldCenterOfMass, this.MainRigidbody.ClosestPointOnColliders(worldPoint), 0.333f);
        }
        return this._restPoint.position;
      }
    }

    private Vector3 WorldCenterOfMass
    {
      get
      {
        if ((Object) this._centerOfMass == (Object) null)
          return this.MainRigidbody.worldCenterOfMass;
        this.UpdateCenterOfMass();
        return this._centerOfMass.position;
      }
    }

    private void UpdateCenterOfMass()
    {
      if (!((Object) this._centerOfMass != (Object) null))
        return;
      this.MainRigidbody.centerOfMass = this.MainRigidbody.transform.InverseTransformPoint(this._centerOfMass.position);
    }

    private TracerIgnoreParams MonsterIgnoreParams
    {
      get
      {
        if (this._monsterIgnoreParams == null)
          this._monsterIgnoreParams = new TracerIgnoreParams("Monster Terrain Tracer", this.Entity, TracerEntityIgnoreFlags.IgnoreSelfAndConnected);
        return this._monsterIgnoreParams;
      }
    }

    [JetBrains.Annotations.CanBeNull]
    protected LookBridge MyLookBridge
    {
      get
      {
        if ((Object) this._lookBridge == (Object) null)
        {
          Entity entity = this.NullCheck<Entity>(this.Entity, "Entity");
          if ((Object) entity == (Object) null)
            return (LookBridge) null;
          this._lookBridge = entity.GetOrCreate<LookBridge>();
        }
        return this._lookBridge;
      }
    }

    [JetBrains.Annotations.CanBeNull]
    protected MotorBridge MyMotorBridge
    {
      get
      {
        if ((Object) this._motorBridge == (Object) null)
        {
          Entity entity = this.NullCheck<Entity>(this.Entity, "Entity");
          if ((Object) entity == (Object) null)
            return (MotorBridge) null;
          this._motorBridge = entity.GetOrCreate<MotorBridge>();
        }
        return this._motorBridge;
      }
    }

    [JetBrains.Annotations.CanBeNull]
    private Animator Animator
    {
      get
      {
        if ((Object) this._animator == (Object) null)
        {
          Entity entity = this.NullCheck<Entity>(this.Entity, "Entity");
          if ((Object) entity == (Object) null)
            return (Animator) null;
          this._animator = entity.TryGet<Animator>();
        }
        return this._animator;
      }
    }

    [JetBrains.Annotations.CanBeNull]
    protected Rigidbody MainRigidbody
    {
      get
      {
        if ((Object) this._mainRigidbody == (Object) null)
        {
          Entity entity = this.NullCheck<Entity>(this.Entity, "Entity");
          if ((Object) entity == (Object) null)
            return (Rigidbody) null;
          this._mainRigidbody = (Rigidbody) (GameObjectAttribute<Rigidbody>) entity.Get<CodeHatch.Engine.Core.Cache.MainRigidbody>();
        }
        return this._mainRigidbody;
      }
    }

    private float MaximumVelocity
    {
      get
      {
        Entity entity = this.NullCheck<Entity>(this.Entity, "Entity");
        return ((Object) entity == (Object) null || entity.IsLocallyControlled ? this.MaximumVelocityBase : this.MaximumVelocityBase * this.MaximumVelocityRemoteMultiplier) * this.MaximumVelocityMultiplier;
      }
    }

    private float MaximumVelocityMultiplier
    {
      get
      {
        if (!this.UseVelocityMultiplierParameter)
          return 1f;
        Animator animator = this.Animator;
        return (Object) animator == (Object) null || !animator.IsParameterControlledByCurve((int) this.MaximumVelocityMultiplierFloat) ? 1f : animator.GetFloat((int) this.MaximumVelocityMultiplierFloat);
      }
    }

    public void Update()
    {
      this.UpdateCenterOfMass();
      this.TerrainTracer.Update(this.TracePosition, this.MonsterIgnoreParams);
      this.UpdateGroundNormal();
      this.UpdateGrip();
    }

    private void UpdateGroundNormal()
    {
      if ((Object) this.TerrainTracer.LastRaycastHit.collider != (Object) null)
      {
        float gripFromNormal = this.GetGripFromNormal(this.TerrainTracer.LastRaycastHit.normal);
        this._groundNormal = Vector3.Lerp(this._groundNormal, this.TerrainTracer.LastRaycastHit.normal, HalfLife.GetRate(this.GroundNormalSmoothingTime) * gripFromNormal);
      }
      else
        this._groundNormal = Vector3.Lerp(this._groundNormal, -Physics.gravity.normalized, HalfLife.GetRate(this.GroundNormalSmoothingTime));
      this._groundNormal.Normalize();
    }

    private void UpdateGrip()
    {
      if (!this.Entity.IsLocallyControlled)
        this._grip = 1f;
      else
        this._grip = HalfLife.GainLoss(this._grip, !((Object) this.TerrainTracer.LastRaycastHit.collider == (Object) null) ? this.GetGripFromNormal(this._groundNormal) : 0.0f, 0.0f, this.GripDecayTime);
    }

    private float GetGripFromNormal(Vector3 normal)
    {
      return this.SlopeDotToGrip.Evaluate(Mathf.Clamp01(Vector3.Dot(-Physics.gravity.normalized, normal)));
    }

    public void FixedUpdate()
    {
      Rigidbody rigidbody = this.NullCheck<Rigidbody>(this.MainRigidbody, "MainRigidbody");
      if ((Object) rigidbody == (Object) null || (double) this.MyMotorBridge.Strength <= 0.0 || (double) this._grip <= 0.0099999997764825821)
        return;
      this.UpdateAxes();
      Vector3 force1 = this.GetThrustForce() * this._grip;
      Vector3 force2 = this.GetRestForce() * this._grip;
      Vector3 torque = this.BalanceConstraint.GetTorque(this._rotation, this._angularVelocity, this._groundNormal) + this.GetLookTorque();
      if (this.GripAffectsTorque)
        torque *= this._grip;
      rigidbody.AddForceAtPosition(force1, this.ThrustPoint, ForceMode.Acceleration);
      rigidbody.AddForceAtPosition(force2, this.RestPoint, ForceMode.Acceleration);
      rigidbody.AddTorque(torque, ForceMode.Acceleration);
    }

    private void UpdateAxes()
    {
      Rigidbody rigidbody = this.NullCheck<Rigidbody>(this.MainRigidbody, "MainRigidbody");
      if ((Object) rigidbody == (Object) null)
        return;
      Transform axisProvider = this.AxisProvider;
      this._rotation = axisProvider.rotation;
      this._up = axisProvider.up;
      this._forward = axisProvider.forward;
      this._right = axisProvider.right;
      this._velocityAtThrustPoint = rigidbody.GetPointVelocity(this.ThrustPoint);
      this._velocityAtRestPoint = rigidbody.GetPointVelocity(this.RestPoint);
      this._angularVelocity = rigidbody.angularVelocity;
      this._targetVelocity = this.NullCheck<MotorBridge>(this.MyMotorBridge, "MyMotorBridge").GetVelocityGround(this.MaximumVelocity);
      this.ApplyLowHealthVelocityMultiplier();
      Quaternion rotation = Quaternion.FromToRotation(this._up, this._groundNormal);
      this._groundProjectedForward = rotation * this._forward;
      this._groundProjectedRight = rotation * this._right;
    }

    private void ApplyLowHealthVelocityMultiplier()
    {
      if (!this.WalkOnLowHealth)
        return;
      Health health = this.Entity.TryGet<Health>();
      if ((Object) health == (Object) null || (double) health.CurrentHealthPercent > (double) this.WalkHealthPercentage)
        return;
      this._targetVelocity *= this.SpeedDecreaseMultiplier;
    }

    private Vector3 GetThrustForce()
    {
      return this.ThrustConstraint.GetForce(this._velocityAtThrustPoint, this._targetVelocity).Perpendicular(this._groundNormal);
    }

    private Vector3 GetRestForce()
    {
      return this.RestConstraint.GetForce(this._velocityAtRestPoint).Parallel(this._groundProjectedRight);
    }

    private Vector3 GetLookTorque()
    {
      return this.LookConstraint.GetTorque(this._groundProjectedForward, this._angularVelocity, this.MyLookBridge.Forward.Perpendicular(this._groundNormal), Vector3.zero).Parallel(this._groundNormal);
    }

    public string ModHandlerName => "Movement";

    public void GetModDefaults(IList<ModEntry> defaultModEntries)
    {
      SetMotorValuesFromBundle component = this.GetComponent<SetMotorValuesFromBundle>();
      defaultModEntries.Add(new ModEntry("WalkAfterHealthPercent", (object) (float) (!((Object) component == (Object) null) ? (double) component.bundle.lowHealthPercentage : (double) this.WalkHealthPercentage)));
      defaultModEntries.Add(new ModEntry("DamagedSlowMultiplier", (object) (float) (!((Object) component == (Object) null) ? (double) component.bundle.speedDecreaseMultiplier : (double) this.SpeedDecreaseMultiplier)));
    }

    public void ApplyMod(string key, object value)
    {
      if (value == null)
        return;
      SetMotorValuesFromBundle component = this.GetComponent<SetMotorValuesFromBundle>();
      string key1 = key;
      if (key1 == null)
        return;
      // ISSUE: reference to a compiler-generated field
      if (MonsterMotor.\u003C\u003Ef__switch\u0024map1A == null)
      {
        // ISSUE: reference to a compiler-generated field
        MonsterMotor.\u003C\u003Ef__switch\u0024map1A = new Dictionary<string, int>(2)
        {
          {
            "WalkAfterHealthPercent",
            0
          },
          {
            "DamagedSlowMultiplier",
            1
          }
        };
      }
      int num;
      // ISSUE: reference to a compiler-generated field
      if (!MonsterMotor.\u003C\u003Ef__switch\u0024map1A.TryGetValue(key1, out num))
        return;
      switch (num)
      {
        case 0:
          this.WalkHealthPercentage = Mathf.Clamp((float) value, 0.0f, 1f);
          this.WalkOnLowHealth = (double) this.WalkHealthPercentage > 0.0;
          if (!((Object) component != (Object) null))
            break;
          component.bundle.lowHealthPercentage = this.WalkHealthPercentage;
          component.bundle.walkOnLowHealth = this.WalkOnLowHealth;
          break;
        case 1:
          this.SpeedDecreaseMultiplier = Mathf.Clamp((float) value, 0.0f, float.MaxValue);
          if (!((Object) component != (Object) null))
            break;
          component.bundle.speedDecreaseMultiplier = this.SpeedDecreaseMultiplier;
          break;
      }
    }
  }
}
