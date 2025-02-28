// Decompiled with JetBrains decompiler
// Type: InstantProjectilePredictor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class InstantProjectilePredictor : ProjectilePredictor
{
  public override Vector3 PredictPosition(Vector3 aimAt, float flightDuration) => aimAt;

  public override float EstimateTime(Vector3 aimAt) => 0.0f;

  public override Vector3 Aim(Vector3 aimAt) => (aimAt - this.transform.position).normalized;
}
