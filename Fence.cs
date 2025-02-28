// Decompiled with JetBrains decompiler
// Type: Fence
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Fence : MonoBehaviour
{
  public float MinImpactForce = 1f;
  public float MassOfFinalObject = 200f;
  private float force;
  private Rigidbody body;

  public void OnCollisionEnter(Collision collision)
  {
    if ((Object) collision.rigidbody != (Object) null && (double) collision.rigidbody.mass < 1.0 / 1000.0)
      return;
    this.force = collision.relativeVelocity.magnitude * collision.gameObject.GetComponent<Rigidbody>().mass;
    if ((double) this.force <= (double) this.MinImpactForce)
      return;
    this.body = this.gameObject.AddComponent<Rigidbody>();
    this.body.mass = this.MassOfFinalObject;
    Object.Destroy((Object) this);
  }
}
