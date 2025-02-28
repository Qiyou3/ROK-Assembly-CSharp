// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.BipedRotation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Engine.Core.Cache;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  [RequireComponent(typeof (BipedMotor))]
  public class BipedRotation : EntityBehaviour
  {
    private BipedBody _body;
    private MotorBridge _myMotorBridge;
    public AngularConstraint TorsoConstraint;
    public AngularConstraint LegsConstraint;

    public void Start()
    {
      this._body = this.Entity.Get<BipedBody>();
      this._myMotorBridge = this.Entity.GetOrCreate<MotorBridge>();
      if (!((Object) this._body == (Object) null))
        return;
      this.LogError<BipedRotation>("body == null - disabling");
      this.enabled = false;
    }

    public void FixedUpdate()
    {
      if ((Object) this._body == (Object) null)
      {
        this.LogError<BipedRotation>("body == null - disabling");
        this.enabled = false;
      }
      else
      {
        float num = this._myMotorBridge.Strength * this._myMotorBridge.Stability;
        if ((double) num <= 0.0)
          return;
        Vector3 targetDirection = -Physics.gravity.normalized;
        Vector3 torque1 = this.TorsoConstraint.GetTorque(this._body.Torso.GetUp(), this._body.Torso.angularVelocity, targetDirection, Vector3.zero);
        Vector3 torque2 = this.LegsConstraint.GetTorque(this._body.Legs.GetUp(), this._body.Legs.angularVelocity, targetDirection, Vector3.zero);
        this._body.Torso.AddTorque(torque1 * num, ForceMode.Acceleration);
        this._body.Legs.AddTorque(torque2 * num, ForceMode.Acceleration);
      }
    }
  }
}
