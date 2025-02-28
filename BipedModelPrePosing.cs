// Decompiled with JetBrains decompiler
// Type: BipedModelPrePosing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BipedModelPrePosing : MonoBehaviour
{
  public Rigidbody legRigidbody;
  private Vector3 previousPosition;
  public Transform footMarker;
  public Transform waistMarker;
  public Transform waistLegMarker;
  public Transform leftFoot;
  public Transform rightFoot;
  public Transform waist;
  private Quaternion restLocalRotation;
  private Vector3 restLocalPosition;
  private Vector3 restPosition;
  public float bobbing = 1f;
  public Vector3 hipsOffsetLegs = Vector3.zero;
  public Vector3 hipsOffsetTorso = Vector3.zero;
  public float velOffsetMultiplier = 0.1f;
  public float velOffsetMax = 0.75f;
  private Vector3 prevWaistMarkerPos;
  public float smoothingTime = 0.5f;
  public float smoothingPercentForTime = 0.9f;
  private Vector3 smoothAvgLocFootPosition;
  private Vector3 smoothLocHipPosition;
  private Vector3 smoothOffset;

  public void Start()
  {
    this.restLocalRotation = this.transform.localRotation;
    this.restLocalPosition = this.transform.localPosition;
    this.restPosition = this.transform.InverseTransformPoint(this.waistMarker.position);
    this.prevWaistMarkerPos = this.waistMarker.position;
  }

  public static Vector3 GetTransformVelocity(Transform transform, ref Vector3 previousPosition)
  {
    Vector3 transformVelocity = !((Object) transform.parent.GetComponent<Rigidbody>() != (Object) null) ? (transform.position - previousPosition) * Time.deltaTime : transform.parent.GetComponent<Rigidbody>().GetPointVelocity(transform.position);
    previousPosition = transform.position;
    return transformVelocity;
  }

  public void LateUpdate()
  {
    this.transform.localRotation = this.restLocalRotation;
    this.transform.localPosition = this.restLocalPosition;
    float num = 1f - Mathf.Pow(1f - this.smoothingPercentForTime, Time.deltaTime / this.smoothingTime);
    Vector3 vector3_1 = (this.footMarker.position - this.previousPosition) / Time.deltaTime;
    this.previousPosition = this.footMarker.position;
    Vector3 direction1 = (this.leftFoot.position + this.rightFoot.position) / 2f - this.footMarker.position;
    Vector3 direction2 = this.waist.position - this.waistLegMarker.position;
    this.smoothAvgLocFootPosition += (this.transform.InverseTransformDirection(direction1) - this.smoothAvgLocFootPosition) * num;
    Vector3 vector3_2 = this.transform.TransformDirection(this.smoothAvgLocFootPosition);
    this.smoothLocHipPosition += (this.transform.InverseTransformDirection(direction2) - this.smoothLocHipPosition) * num;
    Vector3 vector3_3 = this.transform.TransformDirection(this.smoothLocHipPosition);
    Vector3 vector3_4 = BipedModelPrePosing.GetTransformVelocity(this.waistMarker, ref this.prevWaistMarkerPos) * this.velOffsetMultiplier;
    float magnitude = vector3_4.magnitude;
    if ((double) magnitude > (double) this.velOffsetMax)
      vector3_4 *= this.velOffsetMax / magnitude;
    this.transform.rotation = Quaternion.FromToRotation(vector3_3 + this.waistLegMarker.position - (vector3_2 + this.footMarker.position), this.waistLegMarker.position - this.footMarker.position - vector3_4) * this.transform.rotation;
    Vector3 vector3_5 = this.waistMarker.position - this.transform.TransformPoint(this.restPosition);
    Vector3 vector3_6 = this.waistMarker.position - this.waist.position;
    this.smoothOffset += (vector3_6 + (vector3_5 - vector3_6) * this.bobbing - this.smoothOffset) * num;
    this.transform.position += this.smoothOffset;
    this.transform.position += this.transform.TransformDirection(this.hipsOffsetLegs) + this.waistMarker.TransformDirection(this.hipsOffsetTorso);
  }
}
