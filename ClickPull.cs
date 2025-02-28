// Decompiled with JetBrains decompiler
// Type: ClickPull
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class ClickPull : MonoBehaviour
{
  private Vector3 HitPt;
  private Rigidbody HitRigidBody;
  private float HitDepth;
  public float maxForce = 5000f;
  public float forceMultiplier = 1f;
  public float dampingMultiplier = 1f;

  public void FixedUpdate()
  {
    if (Input.GetMouseButtonDown(0))
    {
      Ray ray = this.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
      RaycastHit hitInfo;
      Physics.Raycast(ray, out hitInfo);
      Debug.DrawRay(ray.origin, ray.direction * 100f, Color.yellow);
      if ((UnityEngine.Object) hitInfo.collider != (UnityEngine.Object) null)
      {
        this.HitPt = hitInfo.collider.transform.InverseTransformPoint(hitInfo.point);
        this.HitRigidBody = hitInfo.collider.GetComponent<Rigidbody>();
        this.HitDepth = hitInfo.distance;
        if ((bool) (UnityEngine.Object) this.HitRigidBody && (bool) (UnityEngine.Object) this.GetComponent<Target>())
          this.GetComponent<Target>().target = hitInfo.collider.transform;
      }
      this.LogInfo<ClickPull>("{0}", (object) this.HitRigidBody);
    }
    else if (Input.GetMouseButton(0) && (UnityEngine.Object) this.HitRigidBody != (UnityEngine.Object) null)
    {
      Ray ray = this.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
      RaycastHit raycastHit = ((IEnumerable<RaycastHit>) Physics.RaycastAll(ray)).Where<RaycastHit>((Func<RaycastHit, bool>) (fuck => (UnityEngine.Object) fuck.collider.attachedRigidbody != (UnityEngine.Object) this.HitRigidBody)).OrderBy<RaycastHit, float>((Func<RaycastHit, float>) (fuck => fuck.distance)).FirstOrDefault<RaycastHit>();
      Vector3 vector3 = this.HitRigidBody.transform.TransformPoint(this.HitPt);
      Vector3 mousePosition = Input.mousePosition with
      {
        z = raycastHit.distance * Vector3.Dot(this.GetComponent<Camera>().transform.forward, ray.direction.normalized)
      };
      Vector3 worldPoint = this.GetComponent<Camera>().ScreenToWorldPoint(mousePosition);
      Debug.DrawLine(vector3, worldPoint);
      this.HitRigidBody.AddForceAtPosition((((worldPoint - vector3) * this.forceMultiplier - this.HitRigidBody.GetPointVelocity(vector3) * this.dampingMultiplier) / Time.fixedDeltaTime * this.HitRigidBody.mass).LimitMagnitude(this.maxForce), vector3, ForceMode.Force);
    }
    else
      this.HitRigidBody = (Rigidbody) null;
  }
}
