// Decompiled with JetBrains decompiler
// Type: IKSimple
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class IKSimple : IKSolver
{
  public int maxIterations = 100;

  public override void Solve(Transform[] bones, Vector3 target)
  {
    Transform bone = bones[bones.Length - 1];
    Vector3[] vector3Array = new Vector3[bones.Length - 2];
    float[] numArray1 = new float[bones.Length - 2];
    Quaternion[] quaternionArray = new Quaternion[bones.Length - 2];
    for (int index = 0; index < bones.Length - 2; ++index)
    {
      vector3Array[index] = Vector3.Cross(bones[index + 1].position - bones[index].position, bones[index + 2].position - bones[index + 1].position);
      vector3Array[index] = Quaternion.Inverse(bones[index].rotation) * vector3Array[index];
      vector3Array[index] = vector3Array[index].normalized;
      numArray1[index] = Vector3.Angle(bones[index + 1].position - bones[index].position, bones[index + 1].position - bones[index + 2].position);
      quaternionArray[index] = bones[index + 1].localRotation;
    }
    float[] numArray2 = new float[bones.Length - 1];
    float num1 = 0.0f;
    for (int index = 0; index < bones.Length - 1; ++index)
    {
      numArray2[index] = (bones[index + 1].position - bones[index].position).magnitude;
      num1 += numArray2[index];
    }
    this.positionAccuracy = num1 * (1f / 1000f);
    float magnitude1 = (bone.position - bones[0].position).magnitude;
    float magnitude2 = (target - bones[0].position).magnitude;
    bool flag1 = false;
    bool flag2 = false;
    float num2;
    float num3;
    if ((double) magnitude2 > (double) magnitude1)
    {
      flag1 = true;
      num2 = 1f;
      num3 = 0.0f;
    }
    else
    {
      flag2 = true;
      num2 = 1f;
      num3 = 0.0f;
    }
    int num4 = 0;
    while ((double) Mathf.Abs(magnitude1 - magnitude2) > (double) this.positionAccuracy && num4 < this.maxIterations)
    {
      ++num4;
      float t = flag1 ? (float) (((double) num3 + (double) num2) / 2.0) : num2;
      for (int index = 0; index < bones.Length - 2; ++index)
      {
        float num5 = flag2 ? (float) ((double) numArray1[index] * (1.0 - (double) t) + ((double) numArray1[index] - 30.0) * (double) t) : Mathf.Lerp(180f, numArray1[index], t);
        Quaternion quaternion = Quaternion.AngleAxis(numArray1[index] - num5, vector3Array[index]) * quaternionArray[index];
        bones[index + 1].localRotation = quaternion;
      }
      magnitude1 = (bone.position - bones[0].position).magnitude;
      if ((double) magnitude2 > (double) magnitude1)
        flag1 = true;
      if (flag1)
      {
        if ((double) magnitude2 > (double) magnitude1)
          num2 = t;
        else
          num3 = t;
        if ((double) num2 < 0.0099999997764825821)
          break;
      }
      else
      {
        num3 = num2;
        ++num2;
      }
    }
    bones[0].rotation = Quaternion.AngleAxis(Vector3.Angle(bone.position - bones[0].position, target - bones[0].position), Vector3.Cross(bone.position - bones[0].position, target - bones[0].position)) * bones[0].rotation;
  }
}
