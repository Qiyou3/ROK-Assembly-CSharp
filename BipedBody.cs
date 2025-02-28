// Decompiled with JetBrains decompiler
// Type: BipedBody
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch;
using CodeHatch.Common;
using CodeHatch.OverlapCollisionIgnoring;
using JetBrains.Annotations;
using SmartAssembly.Attributes;
using UnityEngine;

#nullable disable
public class BipedBody : PhysicalBody
{
  [CanBeNull]
  private BipedOverlapCollisionIgnore _collisionIgnore;
  public Rigidbody Torso;
  public Rigidbody Legs;
  private LookBridge _lookBridge;
  [DoNotObfuscate]
  [SerializeField]
  [UsedImplicitly]
  private Transform _pivot;
  protected bool Started;

  [CanBeNull]
  private BipedOverlapCollisionIgnore CollisionIgnore
  {
    get
    {
      return (Object) this._collisionIgnore != (Object) null ? this._collisionIgnore : (this._collisionIgnore = this.Entity.TryGet<BipedOverlapCollisionIgnore>());
    }
  }

  protected void BeginIgnore()
  {
    BipedOverlapCollisionIgnore collisionIgnore = this.CollisionIgnore;
    if (!((Object) collisionIgnore != (Object) null))
      return;
    collisionIgnore.Begin();
  }

  protected LookBridge LookBridge
  {
    get
    {
      if ((Object) this._lookBridge == (Object) null)
        this._lookBridge = this.Entity.GetOrCreate<LookBridge>();
      return this._lookBridge;
    }
  }

  public override Transform Pivot => this._pivot;

  public override Vector3 Position
  {
    get => this.Pivot.position;
    set
    {
      this.Torso.transform.position += value - this.Pivot.position;
      this.Torso.position = this.Torso.transform.position;
      this.Torso.ZeroOutVelocity();
      this.Legs.transform.position = this.Torso.transform.position;
      this.Legs.position = this.Legs.transform.position;
      this.Legs.ZeroOutVelocity();
      this.BeginIgnore();
    }
  }

  public override Vector3 Forward => this.LookBridge.Forward;

  public override Quaternion Rotation
  {
    get => this.LookBridge.Rotation;
    set
    {
      this.LookBridge.Rotation = value;
      Vector3 position = this.Pivot.position;
      this.Torso.transform.rotation = value;
      this.Legs.transform.rotation = value;
      this.Torso.rotation = value;
      this.Legs.rotation = value;
      this.Torso.transform.position += position - this.Pivot.position;
      this.Torso.position = this.Torso.transform.position;
      this.Torso.ZeroOutVelocity();
      this.Legs.transform.position = this.Torso.transform.position;
      this.Legs.position = this.Legs.transform.position;
      this.Torso.ZeroOutVelocity();
      this.BeginIgnore();
    }
  }

  public override Orientation Orientation
  {
    get
    {
      return new Orientation()
      {
        Position = this.Position,
        Rotation = this.Rotation
      };
    }
    set
    {
      this.LookBridge.Rotation = value.Rotation;
      this.Torso.transform.rotation = value.Rotation;
      this.Legs.transform.rotation = value.Rotation;
      this.Torso.rotation = value.Rotation;
      this.Legs.rotation = value.Rotation;
      this.Torso.transform.position += value.Position - this.Pivot.position;
      this.Torso.position = this.Torso.transform.position;
      this.Torso.ZeroOutVelocity();
      this.Legs.transform.position = this.Torso.transform.position;
      this.Legs.position = this.Legs.transform.position;
      this.Legs.ZeroOutVelocity();
      this.BeginIgnore();
    }
  }

  public override float Mass
  {
    get => this.Torso.mass + this.Legs.mass;
    set
    {
      float num = value / this.Mass;
      this.Torso.mass *= num;
      this.Legs.mass *= num;
    }
  }

  public override Vector3 CenterOfMass
  {
    get
    {
      return (this.Torso.mass * this.Torso.worldCenterOfMass + this.Legs.mass * this.Legs.worldCenterOfMass) / this.Mass;
    }
    set => this.OffsetPosition(value - this.CenterOfMass);
  }

  public override Vector3 Velocity
  {
    get
    {
      return (this.Torso.mass * this.Torso.velocity + this.Legs.mass * this.Legs.velocity) / this.Mass;
    }
    set
    {
      if (value == Vector3.zero)
      {
        if (!this.Torso.isKinematic)
          this.Torso.velocity = Vector3.zero;
        if (this.Legs.isKinematic)
          return;
        this.Legs.velocity = Vector3.zero;
      }
      else
      {
        this.Torso.velocity = value;
        this.Legs.velocity = value;
      }
    }
  }

  public override Vector3 AngularVelocity
  {
    get
    {
      return (this.Torso.mass * this.Torso.angularVelocity + this.Legs.mass * this.Legs.angularVelocity) / this.Mass;
    }
    set
    {
      if (value == Vector3.zero)
      {
        if (!this.Torso.isKinematic)
          this.Torso.angularVelocity = Vector3.zero;
        if (this.Legs.isKinematic)
          return;
        this.Legs.angularVelocity = Vector3.zero;
      }
      else
      {
        this.Torso.angularVelocity = value;
        this.Legs.angularVelocity = value;
      }
    }
  }

  public override void OffsetPosition(Vector3 offset)
  {
    this.Torso.transform.position += offset;
    this.Legs.transform.position += offset;
    this.Torso.position = this.Torso.transform.position;
    this.Legs.position = this.Legs.transform.position;
    this.BeginIgnore();
  }

  public override void OffsetVelocity(Vector3 offset)
  {
    this.Torso.velocity += offset;
    this.Legs.velocity += offset;
  }

  public override void OffsetAngularVelocity(Vector3 offset)
  {
    this.Torso.angularVelocity += offset;
    this.Legs.angularVelocity += offset;
  }

  public override void AddForce(Vector3 force, ForceMode forceMode)
  {
    switch (forceMode)
    {
      case ForceMode.Force:
      case ForceMode.Impulse:
        float mass = this.Mass;
        this.Torso.AddForce(force * (this.Torso.mass / mass), forceMode);
        this.Legs.AddForce(force * (this.Legs.mass / mass), forceMode);
        break;
      case ForceMode.VelocityChange:
      case ForceMode.Acceleration:
        this.Torso.AddForce(force, forceMode);
        this.Legs.AddForce(force, forceMode);
        break;
      default:
        this.LogError<BipedBody>("forceMode {0} invalid value.", (object) forceMode);
        break;
    }
  }

  public override void AddForceAtPosition(
    Vector3 force,
    Vector3 position,
    ForceMode forceMode,
    bool project)
  {
    float closestDistanceSquared1;
    Vector3 vector3_1 = this.Torso.ClosestPointOnColliders(position, out closestDistanceSquared1);
    float closestDistanceSquared2;
    Vector3 vector3_2 = this.Legs.ClosestPointOnColliders(position, out closestDistanceSquared2);
    Rigidbody rigidbody;
    if ((double) closestDistanceSquared2 < (double) closestDistanceSquared1)
    {
      rigidbody = this.Legs;
      if (project)
        position = vector3_2;
    }
    else
    {
      rigidbody = this.Torso;
      if (project)
        position = vector3_1;
    }
    switch (forceMode)
    {
      case ForceMode.Force:
      case ForceMode.Impulse:
        rigidbody.AddForceAtPosition(force, position, forceMode);
        break;
      case ForceMode.VelocityChange:
      case ForceMode.Acceleration:
        float mass = this.Mass;
        rigidbody.AddForceAtPosition(force * mass / rigidbody.mass, position, forceMode);
        break;
      default:
        this.LogError<BipedBody>("forceMode {0} invalid value.", (object) forceMode);
        break;
    }
  }

  public override void AddTorque(Vector3 torque, ForceMode forceMode)
  {
    switch (forceMode)
    {
      case ForceMode.Force:
      case ForceMode.Impulse:
        float mass = this.Mass;
        this.Torso.AddTorque(torque * (this.Torso.mass / mass), forceMode);
        this.Legs.AddTorque(torque * (this.Legs.mass / mass), forceMode);
        break;
      case ForceMode.VelocityChange:
      case ForceMode.Acceleration:
        this.Torso.AddTorque(torque, forceMode);
        this.Legs.AddTorque(torque, forceMode);
        break;
      default:
        this.LogError<BipedBody>("forceMode {0} invalid value.", (object) forceMode);
        break;
    }
  }

  public override bool IsKinematic
  {
    get => this.Torso.isKinematic && this.Legs.isKinematic;
    set
    {
      this.Torso.isKinematic = value;
      this.Legs.isKinematic = value;
    }
  }

  public override void SetConstraints(RigidbodyConstraints constraints, bool unfreeze = false)
  {
    if (!unfreeze)
    {
      this.Torso.constraints = constraints;
      this.Legs.constraints = constraints;
    }
    if (!unfreeze)
      return;
    this.Torso.constraints &= ~constraints;
    this.Legs.constraints &= ~constraints;
  }

  public override bool UseGravity
  {
    get => this.Torso.useGravity && this.Legs.useGravity;
    set
    {
      this.Torso.useGravity = value;
      this.Legs.useGravity = value;
    }
  }

  public virtual void Start()
  {
    if ((Object) this.Pivot.parent != (Object) this.Torso.transform)
    {
      this.LogError<BipedBody>("The pivot should be on the Torso. Reparenting...");
      this.Pivot.parent = this.Torso.transform;
    }
    this.BeginIgnore();
    this.Started = true;
  }

  public void OnEnable()
  {
    if (!this.Started)
      return;
    this.BeginIgnore();
  }
}
