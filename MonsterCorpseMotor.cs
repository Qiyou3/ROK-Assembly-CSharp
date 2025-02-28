// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.MonsterCorpseMotor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Engine.Core.Cache;
using CodeHatch.Tracing;
using SmartAssembly.Attributes;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class MonsterCorpseMotor : EntityBehaviour
  {
    [DoNotObfuscate]
    [SerializeField]
    [CodeHatch.Engine.Core.Utility.Attributes.CanBeNull]
    private Transform _axisProvider;
    [CodeHatch.Engine.Core.Utility.Attributes.CanBeNull]
    [SerializeField]
    [DoNotObfuscate]
    private Transform _tracePositionTransform;
    private TracerIgnoreParams _monsterIgnoreParams;
    [JetBrains.Annotations.CanBeNull]
    private MotorBridge _motorBridge;
    [JetBrains.Annotations.CanBeNull]
    private Rigidbody _mainRigidbody;
    public VelocityConstraint VelocityConstraint;
    public BalanceConstraint BalanceConstraint;
    public MonsterTerrainTracer TerrainTracer;
    public float GroundNormalSmoothingTime = 0.2f;
    public float MaximumVelocityRemote = 10f;
    private Vector3 _groundNormal = Vector3.up;
    [Range(0.0f, 100f)]
    public float TargetDrag = 30f;
    [Range(0.0f, 10f)]
    public float DragHalfLife = 1f;
    [Range(0.0f, 100f)]
    public float TargetAngularDrag = 30f;
    [Range(0.0f, 10f)]
    public float AngularDragHalfLife = 0.5f;
    [Range(0.0f, 5f)]
    public float RemoteCorrectiveThreshold = 1f;
    [Range(0.0f, 10f)]
    public float KinematicTime = 5f;
    private float _kinematicTimer = -1f;
    private bool _recordedOriginalDrag;
    private float _originalDrag;
    private float _originalAngularDrag;

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

    private Vector3 DesiredVelocity
    {
      get
      {
        MotorBridge motorBridge = this.NullCheck<MotorBridge>(this.MyMotorBridge, "MyMotorBridge");
        return (Object) motorBridge == (Object) null ? Vector3.zero : motorBridge.GetVelocityGround(this.MaximumVelocityRemote);
      }
    }

    private bool ShouldGraduallyStop
    {
      get => (Object) this.TerrainTracer.LastRaycastHit.collider != (Object) null;
    }

    private void UpdateGradualStop()
    {
      Rigidbody rigidbody = this.NullCheck<Rigidbody>(this.MainRigidbody, "MainRigidbody");
      if ((Object) rigidbody == (Object) null)
        return;
      if ((double) this._kinematicTimer < 0.0)
        this._kinematicTimer = TimeUtil.ResetTimer(this.KinematicTime);
      if (!this.ShouldGraduallyStop)
      {
        this._kinematicTimer = TimeUtil.ResetTimer(this.KinematicTime);
        if (!this._recordedOriginalDrag)
          return;
        rigidbody.drag = this._originalDrag;
        rigidbody.angularDrag = this._originalAngularDrag;
      }
      else if (TimeUtil.ExceededTime(this._kinematicTimer))
      {
        rigidbody.isKinematic = true;
        Object.Destroy((Object) this);
        this.enabled = false;
      }
      else
      {
        if (!this._recordedOriginalDrag)
        {
          this._originalDrag = rigidbody.drag;
          this._originalAngularDrag = rigidbody.angularDrag;
          this._recordedOriginalDrag = true;
        }
        float rate1 = HalfLife.GetRate(this.DragHalfLife);
        rigidbody.drag += (this.TargetDrag - rigidbody.drag) * rate1;
        float rate2 = HalfLife.GetRate(this.AngularDragHalfLife);
        rigidbody.angularDrag += (this.TargetAngularDrag - rigidbody.angularDrag) * rate2;
      }
    }

    public void Update()
    {
      this.TerrainTracer.Update(this.TracePosition, this.MonsterIgnoreParams);
      this.UpdateGroundNormal();
      this.UpdateGradualStop();
    }

    private void UpdateGroundNormal()
    {
      this._groundNormal = Vector3.Lerp(this._groundNormal, !((Object) this.TerrainTracer.LastRaycastHit.collider != (Object) null) ? -Physics.gravity.normalized : this.TerrainTracer.LastRaycastHit.normal, HalfLife.GetRate(this.GroundNormalSmoothingTime));
      this._groundNormal.Normalize();
    }

    public void FixedUpdate()
    {
      Entity entity = this.NullCheck<Entity>(this.Entity, "Entity");
      if ((Object) entity == (Object) null)
        return;
      Rigidbody rigidbody = this.NullCheck<Rigidbody>(this.MainRigidbody, "MainRigidbody");
      if ((Object) rigidbody == (Object) null)
        return;
      if ((Object) this.TerrainTracer.LastRaycastHit.collider != (Object) null)
      {
        Vector3 torque = this.BalanceConstraint.GetTorque(this.AxisProvider.rotation, rigidbody.angularVelocity, this._groundNormal);
        rigidbody.AddTorque(torque, ForceMode.Acceleration);
      }
      if (entity.IsLocallyControlled)
      {
        Vector3 force = this.VelocityConstraint.GetForce(rigidbody.velocity, Vector3.zero).Perpendicular(Physics.gravity);
        rigidbody.AddForce(force, ForceMode.Acceleration);
      }
      else
      {
        Vector3 force = this.VelocityConstraint.GetForce(rigidbody.velocity, this.DesiredVelocity);
        rigidbody.AddForce(force, ForceMode.Acceleration);
      }
    }
  }
}
