// Decompiled with JetBrains decompiler
// Type: ExplodeDamageOld
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class ExplodeDamageOld : MonoBehaviour
{
  public float damage = 20f;
  public float force = 50000f;
  public float radius = 3f;
  public AnimationCurve forceCurve;
  public KeyCode triggerKey;

  public void Update()
  {
    if (!Input.GetKeyDown(this.triggerKey))
      return;
    foreach (Rigidbody rigidbody in ((IEnumerable<Collider>) Physics.OverlapSphere(this.transform.position, this.radius)).Select<Collider, Rigidbody>((Func<Collider, Rigidbody>) (c => c.attachedRigidbody)).Distinct<Rigidbody>().ToArray<Rigidbody>())
    {
      if (!((UnityEngine.Object) rigidbody == (UnityEngine.Object) null))
      {
        Vector3 position = rigidbody.ClosestPointOnBounds(this.transform.position);
        float num = this.forceCurve.Evaluate((position - this.transform.position).magnitude / this.radius);
        Vector3 normalized = (rigidbody.worldCenterOfMass - this.transform.position).normalized;
        rigidbody.AddForceAtPosition(normalized * num * this.force, position);
      }
    }
  }
}
