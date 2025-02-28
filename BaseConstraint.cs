// Decompiled with JetBrains decompiler
// Type: BaseConstraint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BaseConstraint : BaseBehavior
{
  public float globalLimit = 5000f;
  public List<ConstraintLayer> constraintLayers;

  public void AddForce(Vector3 force, Vector3 damper)
  {
    foreach (ConstraintLayer constraintLayer in this.constraintLayers)
      constraintLayer.AddForce(force, damper, this);
  }

  public Vector3 GetForce()
  {
    Vector3 zero = Vector3.zero;
    foreach (ConstraintLayer constraintLayer in this.constraintLayers)
      zero += constraintLayer.GetForce();
    return this.ApplyGlobalLimit(zero, this.globalLimit);
  }

  public Vector3 ApplyGlobalLimit(Vector3 force, float limit)
  {
    float magnitude = force.magnitude;
    if ((double) magnitude > (double) limit)
    {
      foreach (ConstraintLayer constraintLayer in this.constraintLayers)
      {
        constraintLayer.forceVec *= limit / magnitude;
        constraintLayer.damperVec *= limit / magnitude;
      }
      force *= limit / magnitude;
    }
    return force;
  }

  public Vector3 ApplyGlobalDecay(Vector3 force, float decay)
  {
    foreach (ConstraintLayer constraintLayer in this.constraintLayers)
    {
      constraintLayer.forceVec *= decay;
      constraintLayer.damperVec *= decay;
    }
    return force * decay;
  }
}
