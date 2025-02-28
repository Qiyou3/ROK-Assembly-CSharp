// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.BipedSwimming
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Common.Attributes;
using CodeHatch.Engine.Core.Cache;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  [RequireComponent(typeof (Rigidbody), typeof (PlanarBuoyancy))]
  public class BipedSwimming : EntityBehaviour
  {
    public Vector3 ThrustPoint;
    public float MaximumSwimVelocity = 5f;
    public float AccelerationMultiplier = 2f;
    public float RotationAntiRollSpring = 0.1f;
    public float RotationForwardSpring = 0.2f;
    public float RotationUprightVecMagnitude = 0.5f;
    public float MaxAngularAcceleration = 1f;
    public AnimationCurve SubmersionForStrength = new AnimationCurve(new Keyframe[2]
    {
      new Keyframe(0.0f, 0.0f),
      new Keyframe(1f, 1f)
    });
    public AnimationCurve SubmersionForRotationStrength = new AnimationCurve(new Keyframe[2]
    {
      new Keyframe(0.0f, 0.0f),
      new Keyframe(1f, 1f)
    });
    private MotorBridge _motorBridge;
    private EntityRigidbodyManager _manager;
    private Rigidbody _rigidbody;
    private BipedAnimator _lookAnimator;
    private PlanarBuoyancy _buoyancy;
    private PlanarBuoyancy[] _buoyancyComponents;
    [NoEdit]
    [SerializeField]
    private float _strength;
    [NoEdit]
    [SerializeField]
    private float _rotationStrength;
    [SerializeField]
    [NoEdit]
    private float _maxSubmersion;

    private MotorBridge MyMotorBridge
    {
      get
      {
        if ((Object) this._motorBridge == (Object) null)
          this._motorBridge = this.Entity.GetOrCreate<MotorBridge>();
        return this._motorBridge;
      }
    }

    private EntityRigidbodyManager Manager
    {
      get
      {
        if ((Object) this._manager == (Object) null)
          this._manager = this.Entity.GetOrCreate<EntityRigidbodyManager>();
        return this._manager;
      }
    }

    private Rigidbody MyRigidbody
    {
      get
      {
        if ((Object) this._rigidbody == (Object) null)
          this._rigidbody = this.GetComponent<Rigidbody>();
        return this._rigidbody;
      }
    }

    private BipedAnimator LookAnimator
    {
      get
      {
        if ((Object) this._lookAnimator == (Object) null)
          this._lookAnimator = this.Entity.Get<BipedAnimator>();
        return this._lookAnimator;
      }
    }

    private PlanarBuoyancy Buoyancy
    {
      get
      {
        if ((Object) this._buoyancy == (Object) null)
          this._buoyancy = this.GetComponent<PlanarBuoyancy>();
        return this._buoyancy;
      }
    }

    private PlanarBuoyancy[] BuoyancyComponents
    {
      get
      {
        if (this._buoyancyComponents == null)
          this._buoyancyComponents = this.Entity.TryGetArray<PlanarBuoyancy>();
        return this._buoyancyComponents;
      }
    }

    public void FixedUpdate()
    {
      PlanarBuoyancy[] buoyancyComponents = this.BuoyancyComponents;
      this._maxSubmersion = 0.0f;
      for (int index = 0; index < buoyancyComponents.Length; ++index)
        this._maxSubmersion = Mathf.Max(this._maxSubmersion, buoyancyComponents[0].Submersion);
      float num = this.MyMotorBridge.Strength * (1f - this.MyMotorBridge.Stability);
      this._strength = this.SubmersionForStrength.Evaluate(this.Buoyancy.Submersion) * num;
      this._rotationStrength = this.SubmersionForRotationStrength.Evaluate(this._maxSubmersion) * num;
      Vector3 velocityAir = this.MyMotorBridge.GetVelocityAir(this.MaximumSwimVelocity);
      this.MyRigidbody.AddForceAtPosition((velocityAir - this.Manager.AllRigidbodies.AverageVelocity) * this.AccelerationMultiplier * this._strength, this.MyRigidbody.TransformPoint(this.ThrustPoint), ForceMode.Acceleration);
      Quaternion rotation = this.MyRigidbody.rotation;
      Vector3 normal = rotation * Vector3.up;
      this.MyRigidbody.AddTorque(((Quaternion.identity * Quaternion.Inverse(rotation)).ToAngleAxis().Parallel(normal) * this.RotationAntiRollSpring * this._rotationStrength).LimitMagnitude(this.MaxAngularAcceleration), ForceMode.Acceleration);
      Vector3 fromDirection = this.MyRigidbody.rotation * Vector3.up;
      Vector3 toDirection = this.MyRigidbody.velocity + velocityAir - Physics.gravity.normalized * this.RotationUprightVecMagnitude;
      this.MyRigidbody.AddTorque((Quaternion.FromToRotation(fromDirection, toDirection).ToAngleAxis() * toDirection.magnitude * this.RotationForwardSpring * this._rotationStrength).LimitMagnitude(this.MaxAngularAcceleration), ForceMode.Acceleration);
    }
  }
}
