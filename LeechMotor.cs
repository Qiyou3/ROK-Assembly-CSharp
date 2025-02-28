// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.LeechMotor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Cache;
using CodeHatch.Common;
using CodeHatch.Engine;
using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Core.Utility.Attributes;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class LeechMotor : MonoBehaviour, IEntityAware
  {
    private GetEntity _entity;
    private float _internalStrength = float.MinValue;
    private Rigidbody[] _rigidbodies;
    public Rigidbody MainRigidbody;
    [CanBeNull]
    public Transform lookTransform;
    [WarnNull]
    public Transform bodyLookTransform;
    public PhysicMaterial deadMaterial;
    public Collider[] slipperyColliders;
    private readonly List<PhysicMaterial> _originalMaterials = new List<PhysicMaterial>();
    public ConfigurableJoint[] configurableJoints;
    private UnityEngine.JointDrive[] _slerpJointDrives;
    public float[] strengthRemoval;
    [Designer]
    public float accelerationMultiplier;
    [Designer]
    public float jumpVelocityChange;
    [Designer]
    public float jumpInterval = 1f;
    public Loop movementLoop;
    private bool _started;
    private double _nextJumpTime;

    public Entity Entity
    {
      get
      {
        if (this._entity == null)
          this._entity = new GetEntity((MonoBehaviour) this);
        return this._entity.Get();
      }
    }

    public MotorBridge MyMotorBridge => this.Entity.GetOrCreate<MotorBridge>();

    public LookBridge MyLookBridge => this.Entity.GetOrCreate<LookBridge>();

    public Vector3 VelocityGround => this.MyMotorBridge.VelocityGround;

    public float Strength => this.MyMotorBridge.Strength;

    private float InternalStrength
    {
      set
      {
        if ((double) Math.Abs(value - this._internalStrength) < 0.0099999997764825821)
          return;
        this._internalStrength = value;
        this.FadeInFrictionOnPhysicsMaterials(value);
        this.ScaleConfigurableJoints(value);
      }
    }

    public Rigidbody[] Rigidbodies
    {
      get
      {
        if (this._rigidbodies == null)
          this._rigidbodies = this.GetComponentsInChildren<Rigidbody>();
        return this._rigidbodies;
      }
      set => this._rigidbodies = value;
    }

    private void CacheStartingPhysicsSettings()
    {
      this.StoreScaledJointDrives();
      this.StorePhysicsMaterialsForColliders(this.slipperyColliders);
    }

    private void StoreScaledJointDrives()
    {
      this._slerpJointDrives = new UnityEngine.JointDrive[this.configurableJoints.Length];
      for (int index = 0; index < this.configurableJoints.Length; ++index)
      {
        ConfigurableJoint configurableJoint = this.configurableJoints[index];
        this._slerpJointDrives[index] = configurableJoint.slerpDrive;
      }
    }

    private void StorePhysicsMaterialsForColliders(Collider[] colliders)
    {
      for (int index = 0; index < colliders.Length; ++index)
      {
        Collider collider = colliders[index];
        if ((UnityEngine.Object) collider != (UnityEngine.Object) null && (UnityEngine.Object) collider.material != (UnityEngine.Object) null)
          this._originalMaterials.Add(collider.material);
        else
          this._originalMaterials.Add((PhysicMaterial) null);
      }
    }

    private void ResetPhysicsSettings()
    {
      this.ResetJointsAndDrives();
      this.ResetColliderPhysicsMaterials();
    }

    private void ResetJointsAndDrives()
    {
      for (int index = 0; index < this.configurableJoints.Length; ++index)
        this.configurableJoints[index].slerpDrive = this._slerpJointDrives[index];
    }

    private void ResetColliderPhysicsMaterials()
    {
      for (int index = 0; index < this.slipperyColliders.Length; ++index)
      {
        Collider slipperyCollider = this.slipperyColliders[index];
        if (!((UnityEngine.Object) slipperyCollider == (UnityEngine.Object) null))
          slipperyCollider.material = this._originalMaterials[index];
      }
    }

    private void FadeInFrictionOnPhysicsMaterials(float motorStrength)
    {
      for (int index = 0; index < this.slipperyColliders.Length; ++index)
      {
        Collider slipperyCollider = this.slipperyColliders[index];
        if (!((UnityEngine.Object) slipperyCollider == (UnityEngine.Object) null))
        {
          PhysicMaterial material = slipperyCollider.material;
          material.bounciness = Mathf.Lerp(this.deadMaterial.bounciness, this._originalMaterials[index].bounciness, motorStrength);
          material.staticFriction = Mathf.Lerp(this.deadMaterial.staticFriction, this._originalMaterials[index].staticFriction, motorStrength);
          material.dynamicFriction = Mathf.Lerp(this.deadMaterial.dynamicFriction, this._originalMaterials[index].dynamicFriction, motorStrength);
          material.frictionCombine = this.deadMaterial.frictionCombine;
          material.bounceCombine = this.deadMaterial.bounceCombine;
          slipperyCollider.material = material;
        }
      }
    }

    private void ScaleConfigurableJoints(float motorStrength)
    {
      for (int index = 0; index < this.configurableJoints.Length; ++index)
      {
        ConfigurableJoint configurableJoint = this.configurableJoints[index];
        float scalar = Mathf.Lerp(1f, motorStrength, this.strengthRemoval[index]);
        UnityEngine.JointDrive scaled = this._slerpJointDrives[index].GetScaled(scalar, true, true, false);
        if ((double) motorStrength <= 0.0)
        {
          switch (scaled.mode)
          {
            case JointDriveMode.Velocity:
              scaled.mode = JointDriveMode.None;
              break;
            case JointDriveMode.PositionAndVelocity:
              scaled.mode = JointDriveMode.Position;
              break;
          }
        }
        configurableJoint.slerpDrive = scaled;
      }
    }

    public void Start()
    {
      this._started = true;
      this.CacheStartingPhysicsSettings();
    }

    public void Update() => throw new NotImplementedException();

    public void FixedUpdate()
    {
      this.InternalStrength = this.Strength;
      this.UpdateLookTransforms();
      this.UpdateVelocity();
    }

    public void OnDisable()
    {
      if (!this._started)
        return;
      this.InternalStrength = 0.0f;
    }

    private void UpdateLookTransforms()
    {
      if ((UnityEngine.Object) this.lookTransform != (UnityEngine.Object) null)
        this.lookTransform.rotation = this.MyLookBridge.Rotation;
      if (!((UnityEngine.Object) this.bodyLookTransform != (UnityEngine.Object) null))
        return;
      Vector3 forward = this.MyLookBridge.Forward.Perpendicular(Physics.gravity);
      if (!(forward != Vector3.zero))
        return;
      this.bodyLookTransform.rotation = Quaternion.LookRotation(forward, -Physics.gravity);
    }

    private void UpdateVelocity()
    {
      if ((double) this.Strength <= 0.0)
      {
        this.movementLoop.MuteIfNotUpdated();
      }
      else
      {
        this.movementLoop.Update(this.Entity.MainTransform, 1f, 1f);
        this.ApplyAccelerationToRigidbodies(this.GetFlatAcceleration(this.VelocityGround, this.MainRigidbody.velocity));
      }
    }

    private Vector3 GetFlatAcceleration(Vector3 desiredVelocity, Vector3 currentVelocity)
    {
      return (desiredVelocity - currentVelocity) with
      {
        y = 0.0f
      };
    }

    private void ApplyAccelerationToRigidbodies(Vector3 acceleration)
    {
      if ((double) this.Strength <= 0.0 || !acceleration.IsValid())
        return;
      acceleration *= this.accelerationMultiplier * this.Strength;
      for (int index = 0; index < this.Rigidbodies.Length; ++index)
        this.Rigidbodies[index].AddForce(acceleration, ForceMode.Acceleration);
    }

    [ContextMenu("AddAllConfigJoints")]
    public void GetAllConfigurableJoints()
    {
      this.LogInfo<LeechMotor>("This should only be called in the editor!");
      this.configurableJoints = this.GetComponentsInChildren<ConfigurableJoint>();
    }
  }
}
