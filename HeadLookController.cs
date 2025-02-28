// Decompiled with JetBrains decompiler
// Type: HeadLookController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (AlignmentTracker))]
public class HeadLookController : MonoBehaviour
{
  public Transform neck;
  public Transform head;
  public Vector3 headLookVector = Vector3.forward;
  public Vector3 headUpVector = Vector3.up;
  public float rotationMultiplier = 0.5f;
  private Vector3 referenceLookDir;
  private Vector3 referenceUpDir;
  private AlignmentTracker tr;
  private Vector3 lookDir;
  private Vector3 upDir;

  public void Start()
  {
    this.tr = this.GetComponent(typeof (AlignmentTracker)) as AlignmentTracker;
    Quaternion quaternion = Quaternion.Inverse(this.neck.parent.rotation);
    this.referenceLookDir = quaternion * this.transform.rotation * this.headLookVector.normalized;
    this.referenceUpDir = quaternion * this.transform.rotation * this.headUpVector.normalized;
    this.lookDir = this.referenceLookDir;
    this.upDir = this.referenceUpDir;
  }

  public void LateUpdate()
  {
    if ((double) Time.deltaTime == 0.0)
      return;
    Quaternion rotation = this.neck.parent.rotation;
    Quaternion quaternion1 = Quaternion.Inverse(rotation);
    Vector3 vector3_1 = this.transform.rotation * this.headLookVector * 0.01f + Util.ProjectOntoPlane(this.tr.velocity, this.transform.up);
    Vector3 vector3_2 = Quaternion.AngleAxis(Mathf.Clamp(this.tr.angularVelocitySmoothed.magnitude / 2f, -120f, 120f), this.tr.angularVelocitySmoothed) * vector3_1;
    vector3_2 = vector3_2.normalized;
    Vector3 vector3_3 = quaternion1 * vector3_2;
    vector3_3 = vector3_3.normalized;
    float num1 = Vector3.Dot(vector3_3, this.referenceLookDir);
    if ((double) num1 < 0.0)
    {
      if ((double) Vector3.Dot(vector3_3, this.referenceUpDir) < 0.0)
        vector3_3 -= Vector3.Project(vector3_3, this.referenceUpDir);
      else
        vector3_3 += Vector3.Project(vector3_3, this.referenceUpDir) * num1;
    }
    float num2 = Vector3.Angle(this.referenceLookDir, vector3_3);
    Vector3 axis1 = Vector3.Cross(this.referenceLookDir, vector3_3);
    if ((double) num2 > 180.0)
      num2 -= 360f;
    Vector3 vector3_4 = Quaternion.AngleAxis(num2 * this.rotationMultiplier, axis1) * this.referenceLookDir;
    float num3 = Vector3.Angle(this.referenceLookDir, vector3_4);
    Vector3 axis2 = Vector3.Cross(this.referenceLookDir, vector3_4);
    if ((double) num3 > 180.0)
      num3 -= 360f;
    Vector3 normal = Quaternion.AngleAxis(Mathf.Clamp(num3, -80f, 80f), axis2) * this.referenceLookDir;
    Vector3 referenceUpDir = this.referenceUpDir;
    Vector3.OrthoNormalize(ref normal, ref referenceUpDir);
    this.lookDir = Vector3.Slerp(this.lookDir, normal, Time.deltaTime * 5f);
    this.upDir = Vector3.Slerp(this.upDir, referenceUpDir, Time.deltaTime * 5f);
    Vector3.OrthoNormalize(ref this.lookDir, ref this.upDir);
    Quaternion quaternion2 = Quaternion.Slerp(Quaternion.identity, rotation * Quaternion.LookRotation(this.lookDir, this.upDir) * Quaternion.Inverse(rotation * Quaternion.LookRotation(this.referenceLookDir, this.referenceUpDir)), 0.5f);
    this.neck.rotation = quaternion2 * this.neck.rotation;
    this.head.rotation = quaternion2 * this.head.rotation;
  }
}
