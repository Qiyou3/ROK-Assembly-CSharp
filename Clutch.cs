// Decompiled with JetBrains decompiler
// Type: Clutch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Clutch
{
  public float maxTorque;
  public float speedDiff;
  private float clutch_position;
  private float impulseLimit;

  public float GetDragImpulse(
    float engine_speed,
    float drive_speed,
    float engineInertia,
    float totalRotationalInertia,
    float ratio,
    float wheelImpulse,
    float engineTorqueImpulse)
  {
    float num = totalRotationalInertia / (ratio * ratio);
    this.impulseLimit = this.clutch_position * this.maxTorque * Time.deltaTime;
    this.speedDiff = engine_speed - drive_speed;
    return Mathf.Clamp((float) (((double) (engineInertia * num * this.speedDiff) - (double) (engineInertia * (wheelImpulse / ratio)) + (double) (num * engineTorqueImpulse)) / ((double) engineInertia + (double) num)), -this.impulseLimit, this.impulseLimit);
  }

  public void SetClutchPosition(float value) => this.clutch_position = value;

  public float GetClutchPosition() => this.clutch_position;
}
