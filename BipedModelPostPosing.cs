// Decompiled with JetBrains decompiler
// Type: BipedModelPostPosing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class BipedModelPostPosing : MonoBehaviour
{
  public int iterations = 3;
  public Transform headRigidbody;
  public Transform headBoneCenter;
  public Transform headBoneLook;
  public Transform torsoRigidbody;
  public BipedModelPostPosing.SpineBone[] spineBones;
  public Transform leftShoulderBone;
  public Transform rightShoulderBone;
  public float shoulderCorrection = 0.5f;

  public void LateUpdate()
  {
    Vector3 direction1 = this.leftShoulderBone.InverseTransformDirection(this.transform.up);
    Vector3 direction2 = this.rightShoulderBone.InverseTransformDirection(this.transform.up);
    Vector3 direction3 = this.leftShoulderBone.InverseTransformDirection(this.transform.forward);
    Vector3 direction4 = this.rightShoulderBone.InverseTransformDirection(this.transform.forward);
    foreach (BipedModelPostPosing.SpineBone spineBone in this.spineBones)
      spineBone.localRestModelRotation = Quaternion.Inverse(spineBone.bone.rotation) * this.transform.rotation;
    for (int index1 = 0; index1 < this.iterations; ++index1)
    {
      for (int index2 = 0; index2 < this.spineBones.Length; ++index2)
      {
        BipedModelPostPosing.SpineBone spineBone = this.spineBones[index2];
        Quaternion quaternion1 = Quaternion.Slerp(Quaternion.identity, this.torsoRigidbody.rotation * Quaternion.Inverse(spineBone.bone.rotation * spineBone.localRestModelRotation), spineBone.torsoRotationCorrection);
        spineBone.bone.rotation = quaternion1 * spineBone.bone.rotation;
        Quaternion quaternion2 = Quaternion.Slerp(Quaternion.identity, this.headRigidbody.rotation * Quaternion.Inverse(this.headBoneLook.rotation), spineBone.rotationCorrection);
        spineBone.bone.rotation = quaternion2 * spineBone.bone.rotation;
        Quaternion quaternion3 = Quaternion.Slerp(Quaternion.identity, Quaternion.FromToRotation(this.headBoneCenter.position - spineBone.bone.position, this.headRigidbody.position - spineBone.bone.position), spineBone.positionCorrection);
        spineBone.bone.rotation = quaternion3 * spineBone.bone.rotation;
      }
    }
    this.leftShoulderBone.rotation = Quaternion.Slerp(Quaternion.identity, Quaternion.FromToRotation(this.leftShoulderBone.TransformDirection(direction3), this.transform.forward), this.shoulderCorrection) * this.leftShoulderBone.rotation;
    this.rightShoulderBone.rotation = Quaternion.Slerp(Quaternion.identity, Quaternion.FromToRotation(this.rightShoulderBone.TransformDirection(direction4), this.transform.forward), this.shoulderCorrection) * this.rightShoulderBone.rotation;
    this.leftShoulderBone.rotation = Quaternion.Slerp(Quaternion.identity, Quaternion.FromToRotation(this.leftShoulderBone.TransformDirection(direction1), this.transform.up), this.shoulderCorrection) * this.leftShoulderBone.rotation;
    this.rightShoulderBone.rotation = Quaternion.Slerp(Quaternion.identity, Quaternion.FromToRotation(this.rightShoulderBone.TransformDirection(direction2), this.transform.up), this.shoulderCorrection) * this.rightShoulderBone.rotation;
  }

  [Serializable]
  public class SpineBone
  {
    public Transform bone;
    public float positionCorrection;
    public float rotationCorrection;
    public float torsoRotationCorrection;
    [HideInInspector]
    public Quaternion localRestModelRotation;
  }
}
