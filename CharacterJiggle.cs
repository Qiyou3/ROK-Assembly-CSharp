// Decompiled with JetBrains decompiler
// Type: CharacterJiggle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using System;
using UnityEngine;

#nullable disable
public class CharacterJiggle : AnimatorBehaviour
{
  public CharacterJiggle.JiggleParams PositionParams;
  public CharacterJiggle.JiggleParams RotationParams;
  private Vector3 _TargetPosition;
  private Vector3 _TargetRotation;
  private Vector3 _LastParentPosition;
  private Vector3 _LastParentVelocity;
  private Vector3 _LastParentRotation;
  private Vector3 _LastParentRotationalVelocity;
  private Vector3 _Velocity;
  private Vector3 _RotationalVelocity;

  public override void Initialize(AnimatorBehaviourManager manager)
  {
    this._TargetPosition = this.transform.localPosition;
    this._TargetRotation = this.transform.localRotation.ToAngleAxisNotNormalized();
    manager.AddPass(new System.Action(this.InternalUpdate), AnimatorBehaviourPass.PostIK, 50);
  }

  public void InternalUpdate()
  {
    if ((double) this.Manager.DeltaTime <= 0.0)
      return;
    Vector3 vector3_1 = (this.transform.parent.position - this._LastParentPosition) / this.Manager.DeltaTime;
    Vector3 acceleration1 = (vector3_1 - this._LastParentVelocity) / this.Manager.DeltaTime;
    Vector3 vector3_2 = (this.transform.parent.rotation.ToAngleAxisNotNormalized() - this._LastParentRotation) / this.Manager.DeltaTime;
    Vector3 acceleration2 = (vector3_2 - this._LastParentRotationalVelocity) / this.Manager.DeltaTime;
    this.transform.localPosition = this.PhysicsCalc(this.PositionParams, acceleration1, this._TargetPosition, this.transform.localPosition, ref this._Velocity);
    this.transform.localRotation = this.PhysicsCalc(this.RotationParams, acceleration2, this._TargetRotation, this.transform.localRotation.ToAngleAxisNotNormalized(), ref this._RotationalVelocity).AngleAxisQuaternion();
    this._LastParentPosition = this.transform.parent.position;
    this._LastParentVelocity = vector3_1;
    this._LastParentRotation = this.transform.parent.rotation.ToAngleAxisNotNormalized();
    this._LastParentRotationalVelocity = vector3_2;
  }

  private Vector3 PhysicsCalc(
    CharacterJiggle.JiggleParams jiggleParams,
    Vector3 acceleration,
    Vector3 target,
    Vector3 position,
    ref Vector3 velocity)
  {
    velocity *= Mathf.Pow(jiggleParams.Damping, this.Manager.DeltaTime);
    velocity -= (position - target) * jiggleParams.Spring * this.Manager.DeltaTime;
    velocity -= this.transform.parent.InverseTransformDirection(acceleration) * this.Manager.DeltaTime * jiggleParams.AccelerationMagnitude;
    position += velocity;
    Vector3 vector3 = position - target;
    float magnitude = vector3.magnitude;
    Vector3 rhs = vector3 / magnitude;
    if ((double) magnitude > (double) jiggleParams.OffsetLimit)
    {
      position = target + rhs * jiggleParams.OffsetLimit;
      float num = Vector3.Dot(velocity, rhs);
      if ((double) num > 0.0)
        velocity -= num * rhs;
    }
    return position;
  }

  [Serializable]
  public class JiggleParams
  {
    public float AccelerationMagnitude = 0.01f;
    public float OffsetLimit = 0.1f;
    public float Spring = 1f;
    public float Damping = 0.5f;
  }
}
