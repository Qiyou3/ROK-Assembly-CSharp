// Decompiled with JetBrains decompiler
// Type: ConstraintLayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using System;
using UnityEngine;

#nullable disable
[Serializable]
public class ConstraintLayer
{
  public string name;
  public bool enabled = true;
  public Vector3 forceVec = Vector3.zero;
  public Vector3 damperVec = Vector3.zero;
  public float strength = 1000f;
  public float damper = 1000f;
  public float limit = 5000f;
  public float halfLife;
  public bool useAxis = true;
  public bool localAxis = true;
  public Vector3 axis = Vector3.up;
  public float axisStrength = 1000f;
  public float axisDamper = 1000f;
  public float axisLimit = 5000f;
  public float axisHalfLife;

  public void AddForce(Vector3 _force, Vector3 _damper, BaseConstraint baseConstraint)
  {
    if (this.useAxis)
    {
      if (this.localAxis)
      {
        Vector3 normal = baseConstraint.transform.TransformDirection(this.axis);
        this.forceVec += _force.Perpendicular(normal) * this.strength + _force.Parallel(normal) * this.axisStrength;
        this.damperVec += _damper.Perpendicular(normal) * this.damper + _damper.Parallel(normal) * this.axisDamper;
      }
      else
      {
        this.forceVec += _force.Perpendicular(this.axis) * this.strength + _force.Parallel(this.axis) * this.axisStrength;
        this.damperVec += _damper.Perpendicular(this.axis) * this.damper + _damper.Parallel(this.axis) * this.axisDamper;
      }
    }
    else
    {
      this.forceVec += _force * this.strength;
      this.damperVec += _damper * this.damper;
    }
  }

  public Vector3 GetForce()
  {
    if (!this.enabled)
      return Vector3.zero;
    if (this.useAxis)
    {
      Vector3 vector3_1 = this.forceVec.Parallel(this.axis);
      Vector3 vector3_2 = this.forceVec - vector3_1;
      Vector3 vector3_3 = this.damperVec.Parallel(this.axis);
      Vector3 vector3_4 = this.damperVec - vector3_3;
      float magnitude1 = (vector3_1 + vector3_3).magnitude;
      float magnitude2 = (vector3_2 + vector3_4).magnitude;
      if ((double) magnitude1 > (double) this.axisLimit)
      {
        float num = this.axisLimit / magnitude1;
        vector3_1 *= num;
        vector3_3 *= num;
      }
      if ((double) magnitude2 > (double) this.limit)
      {
        float num = this.axisLimit / magnitude2;
        vector3_2 *= num;
        vector3_4 *= num;
      }
      this.forceVec = vector3_1 + vector3_2;
      this.damperVec = vector3_3 + vector3_4;
      Vector3 force = this.forceVec + this.damperVec;
      float num1 = (double) this.halfLife > 0.0 ? ((double) this.halfLife != double.PositiveInfinity ? Mathf.Pow(0.5f, Time.deltaTime / this.halfLife) : 1f) : 0.0f;
      float num2 = (double) this.axisHalfLife > 0.0 ? ((double) this.axisHalfLife != double.PositiveInfinity ? Mathf.Pow(0.5f, Time.deltaTime / this.axisHalfLife) : 1f) : 0.0f;
      this.forceVec = this.forceVec.Perpendicular(this.axis) * num1 + this.forceVec.Parallel(this.axis) * num2;
      this.damperVec = this.damperVec.Perpendicular(this.axis) * num1 + this.damperVec.Parallel(this.axis) * num2;
      return force;
    }
    float magnitude = (this.forceVec + this.damperVec).magnitude;
    if ((double) magnitude > (double) this.limit)
    {
      this.forceVec *= this.limit / magnitude;
      this.damperVec *= this.limit / magnitude;
    }
    Vector3 force1 = this.forceVec + this.damperVec;
    float num3 = (double) this.halfLife != 0.0 ? ((double) this.halfLife != double.PositiveInfinity ? Mathf.Pow(0.5f, Time.deltaTime / this.halfLife) : 1f) : 0.0f;
    this.forceVec *= num3;
    this.damperVec *= num3;
    return force1;
  }
}
