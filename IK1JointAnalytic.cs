// Decompiled with JetBrains decompiler
// Type: IK1JointAnalytic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class IK1JointAnalytic : IKSolver
{
  public override void Solve(Transform[] bones, Vector3 target)
  {
    Transform bone1 = bones[0];
    Transform bone2 = bones[1];
    Transform bone3 = bones[2];
    Vector3 vKneeDir = Vector3.Cross(bone3.position - bone1.position, Vector3.Cross(bone3.position - bone1.position, bone3.position - bone2.position));
    float magnitude1 = (bone2.position - bone1.position).magnitude;
    float magnitude2 = (bone3.position - bone2.position).magnitude;
    Vector3 position = bone1.position;
    Vector3 pAnkle = target;
    Vector3 knee = this.findKnee(position, pAnkle, magnitude1, magnitude2, vKneeDir);
    Quaternion quaternion = Quaternion.FromToRotation(bone2.position - bone1.position, knee - position) * bone1.rotation;
    if (float.IsNaN(quaternion.x))
    {
      this.LogWarning<IK1JointAnalytic>("hipRot=" + (object) quaternion + " pHip=" + (object) position + " pAnkle=" + (object) pAnkle + " fThighLength=" + (object) magnitude1 + " fShinLength=" + (object) magnitude2 + " vKneeDir=" + (object) vKneeDir);
    }
    else
    {
      bone1.rotation = quaternion;
      bone2.rotation = Quaternion.FromToRotation(bone3.position - bone2.position, pAnkle - knee) * bone2.rotation;
    }
  }

  public Vector3 findKnee(
    Vector3 pHip,
    Vector3 pAnkle,
    float fThigh,
    float fShin,
    Vector3 vKneeDir)
  {
    Vector3 vector3_1 = pAnkle - pHip;
    float num1 = vector3_1.magnitude;
    float num2 = (float) (((double) fThigh + (double) fShin) * 0.99900001287460327);
    if ((double) num1 > (double) num2)
    {
      pAnkle = pHip + vector3_1.normalized * num2;
      vector3_1 = pAnkle - pHip;
      num1 = num2;
    }
    float num3 = Mathf.Abs(fThigh - fShin) * 1.001f;
    if ((double) num1 < (double) num3)
    {
      pAnkle = pHip + vector3_1.normalized * num3;
      vector3_1 = pAnkle - pHip;
      num1 = num3;
    }
    float num4 = (float) (((double) num1 * (double) num1 + (double) fThigh * (double) fThigh - (double) fShin * (double) fShin) / 2.0) / num1;
    float num5 = Mathf.Sqrt((float) ((double) fThigh * (double) fThigh - (double) num4 * (double) num4));
    Vector3 vector3_2 = Vector3.Cross(vector3_1, Vector3.Cross(vKneeDir, vector3_1));
    return pHip + num4 * vector3_1.normalized + num5 * vector3_2.normalized;
  }
}
