// Decompiled with JetBrains decompiler
// Type: Constraint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class Constraint
{
  public float globalLimit = 5000f;
  public List<Constraint.ConstraintLayer> constraintLayers;
  private bool automaticallyApplySustain = true;
  public Vector3 lastComputedForce;

  public void AddForce(Vector3 force, Vector3 damper)
  {
    for (int index = 0; index < this.constraintLayers.Count; ++index)
      this.constraintLayers[index].AddForce(force, damper);
  }

  public Vector3 GetForce()
  {
    this.lastComputedForce = Vector3.zero;
    for (int index = 0; index < this.constraintLayers.Count; ++index)
      this.lastComputedForce += this.constraintLayers[index].GetForce(this.automaticallyApplySustain);
    this.ApplyGlobalLimit(ref this.lastComputedForce, this.globalLimit);
    return this.lastComputedForce;
  }

  public Vector3 PeekForce() => this.lastComputedForce;

  public void ApplySustain()
  {
    this.automaticallyApplySustain = false;
    for (int index = 0; index < this.constraintLayers.Count; ++index)
      this.constraintLayers[index].ApplySustain();
  }

  public void SetForce(Vector3 newForce)
  {
    float magnitude1 = newForce.magnitude;
    Vector3 force1 = this.GetForce();
    float magnitude2 = force1.magnitude;
    if ((double) magnitude2 > 0.0)
      this.ApplyGlobalDecay(ref force1, magnitude1 / magnitude2);
    Vector3 force2 = this.GetForce();
    Vector3 offset = (newForce - force2) / (float) this.constraintLayers.Count;
    for (int index = 0; index < this.constraintLayers.Count; ++index)
      this.constraintLayers[index].OffsetForce(offset);
    this.lastComputedForce = newForce;
  }

  public void ApplyGlobalLimit(ref Vector3 force, float limit)
  {
    float magnitude = force.magnitude;
    if ((double) magnitude <= (double) limit)
      return;
    for (int index = 0; index < this.constraintLayers.Count; ++index)
    {
      Constraint.ConstraintLayer constraintLayer = this.constraintLayers[index];
      constraintLayer.forceVec *= limit / magnitude;
      constraintLayer.damperVec *= limit / magnitude;
    }
    force *= limit / magnitude;
  }

  public static float GetLimitFactor(Vector3 force, float limit)
  {
    float magnitude = force.magnitude;
    return (double) magnitude > (double) limit ? limit / magnitude : 1f;
  }

  public void ApplyGlobalDecay(ref Vector3 force, float decay)
  {
    for (int index = 0; index < this.constraintLayers.Count; ++index)
    {
      Constraint.ConstraintLayer constraintLayer = this.constraintLayers[index];
      constraintLayer.forceVec *= decay;
      constraintLayer.damperVec *= decay;
    }
    force *= decay;
  }

  public void ApplyGlobalDecay(float decay)
  {
    for (int index = 0; index < this.constraintLayers.Count; ++index)
    {
      Constraint.ConstraintLayer constraintLayer = this.constraintLayers[index];
      constraintLayer.forceVec *= decay;
      constraintLayer.damperVec *= decay;
    }
  }

  public void Reset()
  {
    for (int index = 0; index < this.constraintLayers.Count; ++index)
    {
      Constraint.ConstraintLayer constraintLayer = this.constraintLayers[index];
      constraintLayer.forceVec = Vector3.zero;
      constraintLayer.damperVec = Vector3.zero;
    }
  }

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
    public Vector3 lastComputedForce = Vector3.zero;

    public void AddForce(Vector3 _force, Vector3 _damper)
    {
      this.forceVec += _force * this.strength;
      this.damperVec += _damper * this.damper;
    }

    public Vector3 GetForce(bool automaticallyApplySustain)
    {
      if (!this.enabled)
        return Vector3.zero;
      float magnitude = (this.forceVec + this.damperVec).magnitude;
      if ((double) magnitude > (double) this.limit)
      {
        this.forceVec *= this.limit / magnitude;
        this.damperVec *= this.limit / magnitude;
      }
      this.lastComputedForce = this.forceVec + this.damperVec;
      if (automaticallyApplySustain)
        this.ApplySustain();
      return this.lastComputedForce;
    }

    public void OffsetForce(Vector3 offset)
    {
      this.forceVec += offset / 2f;
      this.damperVec += offset / 2f;
    }

    public void ApplySustain()
    {
      float num = (double) this.halfLife != 0.0 ? ((double) this.halfLife != double.PositiveInfinity ? Mathf.Pow(0.5f, Time.deltaTime / this.halfLife) : 1f) : 0.0f;
      this.forceVec *= num;
      this.damperVec *= num;
    }
  }
}
