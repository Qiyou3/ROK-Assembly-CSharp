// Decompiled with JetBrains decompiler
// Type: BipedRagdollableBody
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Engine.Core.Utility;
using CodeHatch.RagdollPhysics;
using JetBrains.Annotations;
using RootMotion.FinalIK;
using UnityEngine;

#nullable disable
public class BipedRagdollableBody : BipedBody
{
  public HumanBodyBones PivotLocationOnRagdoll;
  private Transform _pivotLocationRagdoll;
  public Transform PivotLocationTorso;
  public Transform RagdollRoot;
  public Transform RagdollParentWhileNonRagdoll;
  public Transform RagdollParentWhileRagdoll;
  private AnimatorBehaviourManager _managerBacking;
  private Animator _animatorBacking;
  private BipedIK _bipedBacking;
  private Ragdoll _ragdoll;

  private Transform PivotLocationRagdoll
  {
    get
    {
      if ((Object) this._pivotLocationRagdoll == (Object) null)
        this._pivotLocationRagdoll = this.Entity.GetOrCreate<CharacterDefinition>().GetTransform(this.PivotLocationOnRagdoll);
      return this._pivotLocationRagdoll;
    }
  }

  private AnimatorBehaviourManager _manager
  {
    get
    {
      if ((Object) this._managerBacking == (Object) null)
        this._managerBacking = this.Entity.TryGet<AnimatorBehaviourManager>();
      return this._managerBacking;
    }
    set => this._managerBacking = value;
  }

  private Animator _animator
  {
    get
    {
      if ((Object) this._animatorBacking == (Object) null)
        this._animatorBacking = this.Entity.TryGet<Animator>();
      return this._animatorBacking;
    }
    set => this._animatorBacking = value;
  }

  private BipedIK _biped
  {
    get
    {
      if ((Object) this._bipedBacking == (Object) null)
        this._bipedBacking = this.Entity.TryGet<BipedIK>();
      return this._bipedBacking;
    }
    set => this._bipedBacking = value;
  }

  private Ragdoll Ragdoll
  {
    get
    {
      if ((Object) this._ragdoll == (Object) null)
        this._ragdoll = this.Entity.Get<Ragdoll>();
      return this._ragdoll;
    }
  }

  [CanBeNull]
  private RigidbodyGroup RagdollRigidbodies
  {
    get
    {
      Ragdoll ragdoll = this.Ragdoll;
      return (Object) ragdoll != (Object) null ? ragdoll.RigidbodyGroup : (RigidbodyGroup) null;
    }
  }

  public static Quaternion FromToRotation(Quaternion from, Quaternion to)
  {
    return to * Quaternion.Inverse(from);
  }

  public bool IsRagdoll
  {
    get => this.Ragdoll.IsRagdoll;
    set
    {
      if (this.Ragdoll.IsRagdoll == value)
        return;
      if (value)
      {
        this.Ragdoll.IsRagdoll = true;
        if (this.RagdollRigidbodies == null)
        {
          this.LogError<BipedRagdollableBody>("RagdollRigidbodies == null");
          this.Ragdoll.IsRagdoll = false;
          return;
        }
        this.Pivot.parent = this.PivotLocationRagdoll;
        this.Pivot.localPosition = Vector3.zero;
        this.Pivot.localRotation = Quaternion.identity;
        this.RagdollRoot.parent = this.RagdollParentWhileRagdoll;
        if (this.RagdollRigidbodies != null)
        {
          this.RagdollRigidbodies.TotalMass = base.Mass;
          this.RagdollRigidbodies.SetVelocity(base.Velocity);
          this.RagdollRigidbodies.SetAngularVelocity(base.AngularVelocity);
          this.RagdollRigidbodies.IsLocked = base.IsKinematic;
          this.RagdollRigidbodies.UseGravity = base.UseGravity;
          CharacterDefinition characterDefinition = this.Entity.GetOrCreate<CharacterDefinition>();
          Vector3 position = characterDefinition.GetTransform(HumanBodyBones.Hips).position;
          this.RagdollRigidbodies.OffsetPosition(this.Torso.worldCenterOfMass - this.RagdollRigidbodies.CenterOfMass);
          this.RagdollRigidbodies.RotateAroundPoint(Quaternion.FromToRotation(characterDefinition.GetTransform(HumanBodyBones.Neck).position - position, this.Torso.GetUp()), this.Torso.worldCenterOfMass);
        }
        else
          this.LogError<BipedRagdollableBody>("RagdollRigidbodies == null");
        this.Torso.gameObject.SetActive(false);
        this.Legs.gameObject.SetActive(false);
      }
      else
      {
        CharacterDefinition characterDefinition = this.Entity.GetOrCreate<CharacterDefinition>();
        Vector3 vector3_1;
        Vector3 vector3_2;
        Vector3 vector3_3;
        bool flag;
        bool useGravity;
        if (this.RagdollRigidbodies != null)
        {
          vector3_1 = this.RagdollRigidbodies.CenterOfMass;
          vector3_2 = this.RagdollRigidbodies.AverageVelocity;
          vector3_3 = this.RagdollRigidbodies.AverageAngularVelocity;
          flag = this.RagdollRigidbodies.IsLocked;
          useGravity = this.RagdollRigidbodies.UseGravity;
        }
        else
        {
          this.LogError<BipedRagdollableBody>("RagdollRigidbodies == null");
          vector3_1 = characterDefinition.GetTransform(HumanBodyBones.Spine).position;
          vector3_2 = Vector3.zero;
          vector3_3 = Vector3.zero;
          flag = base.IsKinematic;
          useGravity = base.UseGravity;
        }
        this.Ragdoll.IsRagdoll = false;
        this.Torso.gameObject.SetActive(true);
        this.Legs.gameObject.SetActive(true);
        this.Pivot.parent = this.PivotLocationTorso;
        this.Pivot.localPosition = Vector3.zero;
        this.Pivot.localRotation = Quaternion.identity;
        Vector3 position = characterDefinition.GetTransform(HumanBodyBones.Hips).position;
        Vector3 lhs1 = characterDefinition.GetTransform(HumanBodyBones.RightShoulder).position - characterDefinition.GetTransform(HumanBodyBones.LeftShoulder).position;
        Vector3 vector3_4 = characterDefinition.GetTransform(HumanBodyBones.Neck).position - position;
        this.Torso.transform.rotation = Quaternion.LookRotation(Vector3.Cross(lhs1, vector3_4), vector3_4);
        this.Torso.rotation = this.Torso.transform.rotation;
        this.Torso.position = this.Torso.transform.position;
        this.Torso.transform.position += vector3_1 - this.Torso.worldCenterOfMass;
        this.Torso.position = this.Torso.transform.position;
        Vector3 vector3_5 = (characterDefinition.GetTransform(HumanBodyBones.LeftFoot).position + characterDefinition.GetTransform(HumanBodyBones.RightFoot).position) / 2f;
        Vector3 lhs2 = characterDefinition.GetTransform(HumanBodyBones.RightUpperLeg).position - characterDefinition.GetTransform(HumanBodyBones.LeftUpperLeg).position;
        Vector3 vector3_6 = position - vector3_5;
        this.Legs.transform.rotation = Quaternion.LookRotation(Vector3.Cross(lhs2, vector3_6), vector3_6);
        this.Legs.rotation = this.Legs.transform.rotation;
        this.Legs.position = this.Legs.transform.position;
        this.Legs.transform.position += vector3_1 - this.Legs.worldCenterOfMass;
        this.Legs.position = this.Legs.transform.position;
        base.Velocity = vector3_2;
        base.AngularVelocity = vector3_3;
        base.IsKinematic = flag;
        base.UseGravity = useGravity;
        this.RagdollRoot.parent = this.RagdollParentWhileNonRagdoll;
        this.RagdollRoot.localPosition = Vector3.zero;
        this.RagdollRoot.localRotation = Quaternion.identity;
        this.BeginIgnore();
      }
      this.EnableAnimation(!this.Ragdoll.IsRagdoll);
      if (this.RagdollRigidbodies == null)
        return;
      this.RagdollRigidbodies._refresher.IsRefreshed = false;
    }
  }

  private void EnableAnimation(bool enable)
  {
    if ((Object) this._manager != (Object) null)
      this._manager.enabled = enable;
    if ((Object) this._animator != (Object) null)
      this._animator.enabled = enable;
    if (!((Object) this._biped != (Object) null))
      return;
    this._biped.enabled = enable;
  }

  public override Vector3 Position
  {
    get => base.Position;
    set
    {
      if (!this.Ragdoll.IsRagdoll)
      {
        base.Position = value;
      }
      else
      {
        Vector3 offset = value - this.Position;
        if (this.RagdollRigidbodies != null)
        {
          this.RagdollRigidbodies.OffsetPosition(offset);
          this.RagdollRigidbodies.SetVelocity(Vector3.zero);
          this.RagdollRigidbodies.SetAngularVelocity(Vector3.zero);
        }
        else
          this.LogError<BipedRagdollableBody>("RagdollRigidbodies == null");
      }
    }
  }

  public override Quaternion Rotation
  {
    get => base.Rotation;
    set
    {
      if (!this.Ragdoll.IsRagdoll)
        base.Rotation = value;
      else
        this.LookBridge.Rotation = value;
    }
  }

  public override Orientation Orientation
  {
    get => base.Orientation;
    set
    {
      if (!this.Ragdoll.IsRagdoll)
      {
        base.Orientation = value;
      }
      else
      {
        Vector3 offset = value.Position - base.Position;
        if (this.RagdollRigidbodies != null)
        {
          this.RagdollRigidbodies.OffsetPosition(offset);
          this.RagdollRigidbodies.SetVelocity(Vector3.zero);
          this.RagdollRigidbodies.SetAngularVelocity(Vector3.zero);
        }
        else
          this.LogError<BipedRagdollableBody>("RagdollRigidbodies != null");
        this.LookBridge.Rotation = value.Rotation;
      }
    }
  }

  public override float Mass
  {
    get
    {
      if (!this.Ragdoll.IsRagdoll)
        return base.Mass;
      if (this.RagdollRigidbodies != null)
        return this.RagdollRigidbodies.TotalMass;
      this.LogError<BipedRagdollableBody>("RagdollRigidbodies == null");
      return base.Mass;
    }
    set
    {
      if (!this.Ragdoll.IsRagdoll)
        base.Mass = value;
      else if (this.RagdollRigidbodies == null)
        this.LogError<BipedRagdollableBody>("RagdollRigidbodies == null");
      else
        this.RagdollRigidbodies.TotalMass = value;
    }
  }

  public override Vector3 CenterOfMass
  {
    get
    {
      if (!this.Ragdoll.IsRagdoll)
        return base.CenterOfMass;
      if (this.RagdollRigidbodies != null)
        return this.RagdollRigidbodies.CenterOfMass;
      this.LogError<BipedRagdollableBody>("RagdollRigidbodies == null");
      return base.CenterOfMass;
    }
    set
    {
      if (!this.Ragdoll.IsRagdoll)
        base.CenterOfMass = value;
      else if (this.RagdollRigidbodies == null)
        this.LogError<BipedRagdollableBody>("RagdollRigidbodies == null");
      else
        this.RagdollRigidbodies.OffsetPosition(value - this.RagdollRigidbodies.CenterOfMass);
    }
  }

  public override Vector3 Velocity
  {
    get
    {
      if (!this.Ragdoll.IsRagdoll)
        return base.Velocity;
      if (this.RagdollRigidbodies != null)
        return this.RagdollRigidbodies.AverageVelocity;
      this.LogError<BipedRagdollableBody>("RagdollRigidbodies == null");
      return Vector3.zero;
    }
    set
    {
      if (!this.Ragdoll.IsRagdoll)
        base.Velocity = value;
      else if (this.RagdollRigidbodies == null)
        this.LogError<BipedRagdollableBody>("RagdollRigidbodies == null");
      else
        this.RagdollRigidbodies.SetVelocity(value);
    }
  }

  public override Vector3 AngularVelocity
  {
    get
    {
      if (!this.Ragdoll.IsRagdoll)
        return base.AngularVelocity;
      if (this.RagdollRigidbodies != null)
        return this.RagdollRigidbodies.AverageAngularVelocity;
      this.LogError<BipedRagdollableBody>("RagdollRigidbodies == null");
      return Vector3.zero;
    }
    set
    {
      if (!this.Ragdoll.IsRagdoll)
        base.AngularVelocity = value;
      else if (this.RagdollRigidbodies == null)
        this.LogError<BipedRagdollableBody>("RagdollRigidbodies == null");
      else
        this.RagdollRigidbodies.SetAngularVelocity(value);
    }
  }

  public override void OffsetPosition(Vector3 offset)
  {
    if (!this.Ragdoll.IsRagdoll)
      base.OffsetPosition(offset);
    else if (this.RagdollRigidbodies == null)
      this.LogError<BipedRagdollableBody>("RagdollRigidbodies == null");
    else
      this.RagdollRigidbodies.OffsetPosition(offset);
  }

  public override void OffsetVelocity(Vector3 offset)
  {
    if (!this.Ragdoll.IsRagdoll)
      base.OffsetVelocity(offset);
    else if (this.RagdollRigidbodies == null)
      this.LogError<BipedRagdollableBody>("RagdollRigidbodies == null");
    else
      this.RagdollRigidbodies.OffsetVelocity(offset);
  }

  public override void OffsetAngularVelocity(Vector3 offset)
  {
    if (!this.Ragdoll.IsRagdoll)
      base.OffsetAngularVelocity(offset);
    else if (this.RagdollRigidbodies == null)
      this.LogError<BipedRagdollableBody>("RagdollRigidbodies == null");
    else
      this.RagdollRigidbodies.OffsetAngularVelocity(offset);
  }

  public override void AddForce(Vector3 force, ForceMode forceMode)
  {
    if (!this.Ragdoll.IsRagdoll)
      base.AddForce(force, forceMode);
    else if (this.RagdollRigidbodies == null)
    {
      this.LogError<BipedRagdollableBody>("RagdollRigidbodies == null");
    }
    else
    {
      switch (forceMode)
      {
        case ForceMode.Force:
          this.RagdollRigidbodies.AddUniformForce(force);
          break;
        case ForceMode.Impulse:
          this.RagdollRigidbodies.AddUniformForce(force / Time.fixedDeltaTime);
          break;
        case ForceMode.VelocityChange:
          this.RagdollRigidbodies.AddUniformForce(force * this.RagdollRigidbodies.TotalMass / Time.fixedDeltaTime);
          break;
        case ForceMode.Acceleration:
          this.RagdollRigidbodies.AddUniformForce(force * this.RagdollRigidbodies.TotalMass);
          break;
        default:
          this.LogError<BipedRagdollableBody>("forceMode {0} invalid value.", (object) forceMode);
          break;
      }
    }
  }

  public override void AddForceAtPosition(
    Vector3 force,
    Vector3 position,
    ForceMode forceMode,
    bool project)
  {
    if (this.Ragdoll.IsRagdoll)
      return;
    base.AddForceAtPosition(force, position, forceMode, project);
  }

  public override void AddTorque(Vector3 torque, ForceMode forceMode)
  {
    if (!this.Ragdoll.IsRagdoll)
      base.AddForce(torque, forceMode);
    else if (this.RagdollRigidbodies == null)
    {
      this.LogError<BipedRagdollableBody>("RagdollRigidbodies == null");
    }
    else
    {
      switch (forceMode)
      {
        case ForceMode.Force:
          this.RagdollRigidbodies.AddUniformTorque(torque);
          break;
        case ForceMode.Impulse:
          this.RagdollRigidbodies.AddUniformTorque(torque / Time.fixedDeltaTime);
          break;
        case ForceMode.VelocityChange:
          this.RagdollRigidbodies.AddUniformTorque(torque * this.RagdollRigidbodies.TotalMass / Time.fixedDeltaTime);
          break;
        case ForceMode.Acceleration:
          this.RagdollRigidbodies.AddUniformTorque(torque * this.RagdollRigidbodies.TotalMass);
          break;
        default:
          this.LogError<BipedRagdollableBody>("forceMode {0} invalid value.", (object) forceMode);
          break;
      }
    }
  }

  public override bool IsKinematic
  {
    get
    {
      if (!this.Ragdoll.IsRagdoll)
        return base.IsKinematic;
      if (this.RagdollRigidbodies != null)
        return this.RagdollRigidbodies.IsLocked;
      this.LogError<BipedRagdollableBody>("RagdollRigidbodies == null");
      return false;
    }
    set
    {
      if (this.IsKinematic == value)
        return;
      if (!this.Ragdoll.IsRagdoll)
        base.IsKinematic = value;
      else if (this.RagdollRigidbodies == null)
        this.LogError<BipedRagdollableBody>("RagdollRigidbodies == null");
      else
        this.RagdollRigidbodies.IsLocked = value;
    }
  }

  public override bool UseGravity
  {
    get
    {
      if (!this.Ragdoll.IsRagdoll)
        return base.UseGravity;
      if (this.RagdollRigidbodies != null)
        return this.RagdollRigidbodies.UseGravity;
      this.LogError<BipedRagdollableBody>("RagdollRigidbodies == null");
      return base.UseGravity;
    }
    set
    {
      if (this.UseGravity == value)
        return;
      if (!this.Ragdoll.IsRagdoll)
        base.IsKinematic = value;
      else if (this.RagdollRigidbodies == null)
        this.LogError<BipedRagdollableBody>("RagdollRigidbodies == null");
      else
        this.RagdollRigidbodies.UseGravity = value;
    }
  }

  public override void Start()
  {
    if (this.IsRagdoll)
    {
      this.LogError<BipedRagdollableBody>("Character is initially a ragdoll. This isn't supported yet!");
      if ((Object) this.Pivot.parent != (Object) this.PivotLocationRagdoll.transform)
      {
        this.LogError<BipedRagdollableBody>("The pivot should be on the PivotLocationRagdoll at Start() if the character IS initially a ragdoll. Reparenting...");
        this.Pivot.parent = this.PivotLocationRagdoll.transform;
      }
    }
    else if ((Object) this.Pivot.parent != (Object) this.PivotLocationTorso.transform)
    {
      this.LogError<BipedRagdollableBody>("The pivot should be on the PivotLocationTorso at Start() if the character IS NOT initially a ragdoll. Reparenting...");
      this.Pivot.parent = this.PivotLocationTorso.transform;
    }
    this.BeginIgnore();
    this.Started = true;
  }
}
