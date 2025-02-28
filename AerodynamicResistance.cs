// Decompiled with JetBrains decompiler
// Type: AerodynamicResistance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class AerodynamicResistance : MonoBehaviour
{
  public float Cx = 0.3f;
  public float Area = 1.858f;
  public float dragForce;
  private Rigidbody body;
  private CarDynamics cardynamics;

  public void Start()
  {
    this.body = this.GetComponent<Rigidbody>();
    this.cardynamics = this.GetComponent<CarDynamics>();
  }

  public void FixedUpdate()
  {
    this.dragForce = (double) this.body.velocity.sqrMagnitude > 1.0 / 1000.0 ? 0.5f * this.Cx * this.Area * this.cardynamics.airDensity * this.body.velocity.sqrMagnitude : 0.0f;
    this.body.AddForce(-this.dragForce * this.body.velocity.normalized);
  }
}
