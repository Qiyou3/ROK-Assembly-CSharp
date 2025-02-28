// Decompiled with JetBrains decompiler
// Type: Arcader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Arcader : UnityCarModifier
{
  protected override void TorqueHelper(
    out float torque,
    float absLateralSlip,
    float lateralSlip,
    float strength)
  {
    torque = (float) (((double) absLateralSlip - 1.0) * (double) Mathf.Sign(lateralSlip) * 10000.0) * strength * this.torqueHelperStrength;
    if ((double) absLateralSlip <= 1.0)
      return;
    this.body.AddRelativeTorque(-Vector3.up * torque);
  }

  protected override void GripHelper(
    out float gripSlip,
    out float gripVelo,
    float absLateralSlip,
    float strength)
  {
    gripSlip = gripVelo = 0.0f;
    foreach (Wheel allWheel in this.axles.allWheels)
    {
      gripSlip = Mathf.Clamp01(absLateralSlip) * strength;
      allWheel.gripSlip = gripSlip;
      gripVelo = (float) (((double) this.veloKmh - (double) this.minVelocity) * 0.001500000013038516) * strength * this.gripHelperStrength;
      allWheel.gripVelo = gripVelo;
    }
  }

  protected override void ResetParameters()
  {
    if (this.axles.allWheels == null)
      return;
    this.GripHelper(out this.gripSlip, out this.gripVelo, 0.0f, 0.0f);
  }
}
