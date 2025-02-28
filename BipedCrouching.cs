// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.BipedCrouching
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Engine.Core.Cache;
using System;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class BipedCrouching : EntityBehaviour
  {
    public float maxCrouchOffset = 0.2f;
    public float minCrouchOffset = -0.1f;
    public JointDriveSerializable crouchDrive;
    public ConfigurableJoint crouchJoint;
    private MotorBridge _myMotorBridge;
    private float _prevStrength = -1f;
    private float _prevCrouch = -1f;

    public void Start() => this._myMotorBridge = this.Entity.GetOrCreate<MotorBridge>();

    public void FixedUpdate()
    {
      float strength = this._myMotorBridge.Strength;
      if ((double) Math.Abs(strength - this._prevStrength) > 0.0099999997764825821)
      {
        this.crouchJoint.yDrive = this.crouchDrive.GetScaled(strength);
        this._prevStrength = strength;
      }
      float crouch = this._myMotorBridge.Crouch;
      if ((double) Math.Abs(crouch - this._prevCrouch) > 0.0099999997764825821)
      {
        this.crouchJoint.targetPosition = new Vector3(0.0f, Mathf.Lerp(this.maxCrouchOffset, this.minCrouchOffset, crouch), 0.0f);
        this._prevCrouch = crouch;
      }
      Rigidbody component = this.crouchJoint.GetComponent<Rigidbody>();
      Rigidbody connectedBody = this.crouchJoint.connectedBody;
      Vector3 force = (Physics.gravity * (connectedBody.mass * strength)).Parallel(component.transform.up);
      component.AddForce(force);
      connectedBody.AddForce(-force);
    }

    public void OnDisable()
    {
      this.crouchJoint.yDrive = this.crouchJoint.yDrive.SetMode(JointDriveMode.None);
    }

    public void OnEnable()
    {
      this.crouchJoint.yDrive = this.crouchJoint.yDrive.SetMode(this.crouchDrive.Mode);
    }
  }
}
