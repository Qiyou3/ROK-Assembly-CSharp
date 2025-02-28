// Decompiled with JetBrains decompiler
// Type: BasicFlying
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch;
using CodeHatch.Common;
using CodeHatch.Engine.Core.Cache;
using UnityEngine;

#nullable disable
public class BasicFlying : EntityBehaviour
{
  public float maximumVelocity = 10f;
  public float lookSpring = 1f;
  public float lookDamper = 1f;
  public float motorStrength = 1f;
  private LookBridge _lookBridge;
  private MotorBridge _motorBridge;
  private Rigidbody _mainRigidbody;

  private LookBridge MyLookBridge
  {
    get
    {
      if ((Object) this._lookBridge == (Object) null)
        this._lookBridge = this.Entity.GetOrCreate<LookBridge>();
      return this._lookBridge;
    }
  }

  private MotorBridge MyMotorBridge
  {
    get
    {
      if ((Object) this._motorBridge == (Object) null)
        this._motorBridge = this.Entity.GetOrCreate<MotorBridge>();
      return this._motorBridge;
    }
  }

  private Rigidbody MainRigidbody
  {
    get
    {
      if ((Object) this._mainRigidbody == (Object) null)
        this._mainRigidbody = (Rigidbody) (GameObjectAttribute<Rigidbody>) this.Entity.Get<CodeHatch.Engine.Core.Cache.MainRigidbody>();
      return this._mainRigidbody;
    }
  }

  public void FixedUpdate()
  {
    if ((double) this.MyLookBridge.Strength > 0.0)
      this.MainRigidbody.AddTorque(((this.MyLookBridge.Rotation * Quaternion.Inverse(this.MainRigidbody.rotation)).ToAngleAxis() * this.lookSpring - this.MainRigidbody.angularVelocity * this.lookDamper) * this.MyLookBridge.Strength, ForceMode.VelocityChange);
    if ((double) this.MyMotorBridge.Strength <= 0.0)
      return;
    this.MainRigidbody.AddForce((this.MyMotorBridge.GetVelocityAir(this.maximumVelocity) - this.MainRigidbody.velocity) * this.motorStrength * this.MyMotorBridge.Strength, ForceMode.VelocityChange);
    this.MainRigidbody.AddForce(-Physics.gravity * this.MyMotorBridge.Strength, ForceMode.Acceleration);
  }
}
