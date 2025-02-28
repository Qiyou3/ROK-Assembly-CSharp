// Decompiled with JetBrains decompiler
// Type: GunRotatingBarrel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Engine.Core.Cache;
using UnityEngine;

#nullable disable
public class GunRotatingBarrel : EntityBehaviour
{
  public int numberOfBarrels = 3;
  public AnimationCurve spinupVelocity;
  public AnimationCurve spinupAcceleration;
  public Vector3 rotationAxis = Vector3.up;
  public Transform counterTorqueTransform;
  public float counterTorqueAmount;
  public Vector3 counterTorqueAxis;
  public float torqueSmoothing = 0.2f;
  private float velocity;
  private float rotation;
  private float _velocityState;
  private float _accelerationState;
  private float _targetVelocity;
  private bool spinning;
  private Quaternion defaultRotation = Quaternion.identity;
  private Quaternion counterTorqueTransformDefaultRotation;
  private float lastVelocity;
  private float smoothTorque;

  public bool AtFullSpeed { get; protected set; }

  public bool AtFullStop { get; protected set; }

  public void Start()
  {
    this.defaultRotation = this.transform.localRotation;
    if (!(bool) (Object) this.counterTorqueTransform)
      return;
    this.counterTorqueTransformDefaultRotation = this.counterTorqueTransform.localRotation;
  }

  public void OnEnable() => this.rotation = 0.0f;

  public void Rotate(float interval)
  {
    this.spinning = true;
    this._targetVelocity = 360f / (float) this.numberOfBarrels / interval;
  }

  public void Update()
  {
    this.AtFullSpeed = (double) this._velocityState >= (double) this.spinupVelocity.GetMaxTime();
    this.AtFullStop = (double) this._velocityState <= (double) this.spinupVelocity.GetMinTime();
    if (this.AtFullSpeed || this.AtFullStop)
      this._accelerationState = 0.0f;
    this._accelerationState = Mathf.Clamp(this._accelerationState + (!this.spinning ? -1f : 1f) * Time.deltaTime, this.spinupAcceleration.GetMinTime(), this.spinupAcceleration.GetMaxTime());
    this._velocityState = Mathf.Clamp(this._velocityState + this.spinupAcceleration.Evaluate(this._accelerationState) * Time.deltaTime, this.spinupVelocity.GetMinTime(), this.spinupVelocity.GetMaxTime());
    this.lastVelocity = this.velocity;
    this.velocity = this.spinupVelocity.Evaluate(this._velocityState);
    this.rotation += this.velocity * Time.deltaTime * this._targetVelocity;
    this.transform.localRotation = this.defaultRotation * Quaternion.AngleAxis(this.rotation, this.rotationAxis);
    this.spinning = false;
    if (!(bool) (Object) this.counterTorqueTransform)
      return;
    this.smoothTorque = HalfLife.GainLoss(this.smoothTorque, (this.velocity - this.lastVelocity) / Time.deltaTime, this.torqueSmoothing, this.torqueSmoothing);
    this.counterTorqueTransform.transform.localRotation = this.counterTorqueTransformDefaultRotation * Quaternion.AngleAxis(this.smoothTorque * this.counterTorqueAmount, this.counterTorqueAxis);
  }
}
