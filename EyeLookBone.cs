// Decompiled with JetBrains decompiler
// Type: EyeLookBone
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using UnityEngine;

#nullable disable
public class EyeLookBone : BaseLookBone
{
  [Range(0.0f, 1f)]
  public float FromToVsLookAtRatio;
  public Transform UpReference;
  public Vector3 UpReferenceLocalUp = Vector3.up;
  public Vector3 LocalForward;
  public float MaxDegreesPerSecond = float.MaxValue;
  private Vector3 _previousEyeToLookAt = Vector3.zero;
  private Quaternion _previousSourceRotation = Quaternion.identity;
  private bool _first = true;

  public Vector3 UpReferenceGlobalUp
  {
    get
    {
      return (Object) this.UpReference != (Object) null ? this.UpReference.rotation * this.UpReferenceLocalUp : this.UpReferenceLocalUp;
    }
  }

  public void UpdateMaxDegreesPerSecond()
  {
    Vector3 forward = this.Forward;
    Quaternion sourceRotation = this.SourceRotation;
    if (this._first)
    {
      this.MaxDegreesPerSecond = float.MaxValue;
      this._previousEyeToLookAt = forward;
      this._previousSourceRotation = sourceRotation;
      this._first = false;
    }
    else
    {
      this.MaxDegreesPerSecond = Mathf.Max(Vector3.Angle(this._previousEyeToLookAt, forward), Quaternion.Angle(this._previousSourceRotation, sourceRotation)) / Time.deltaTime;
      this._previousEyeToLookAt = forward;
      this._previousSourceRotation = sourceRotation;
    }
  }

  public override void UpdateTargetRotationRecursively(float interest)
  {
    this.transform.rotation = this.GetRotationRequired() * this.transform.rotation;
    this.LimitRotation(interest);
  }

  public Quaternion GetRotationRequired()
  {
    Quaternion rotation1 = this.transform.rotation;
    Vector3 vector3 = rotation1.TransformVector(this.LocalForward);
    Vector3 forward = this.Forward;
    Quaternion rotation2 = Quaternion.FromToRotation(vector3, forward);
    Vector3 referenceGlobalUp = this.UpReferenceGlobalUp;
    Vector3 vector = this.SourceRotation.InverseTransformVector(referenceGlobalUp);
    Vector3 upwards = rotation1.TransformVector(vector);
    Quaternion to = Quaternion.LookRotation(forward, referenceGlobalUp) * Quaternion.LookRotation(vector3, upwards).GetInverse();
    return Quaternion.Slerp(rotation2, to, this.FromToVsLookAtRatio);
  }
}
