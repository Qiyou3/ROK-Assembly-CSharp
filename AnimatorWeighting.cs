// Decompiled with JetBrains decompiler
// Type: AnimatorWeighting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common.Attributes;
using System;
using UnityEngine;

#nullable disable
[Serializable]
public class AnimatorWeighting
{
  public float selfWeight = 1f;
  [Separator("State Weighting")]
  public string stateName;
  public int stateLayer;
  public bool[] layerMask;
  public bool invertStateWeight;
  [Separator("Parameter Weighting")]
  public string parameterName;
  public bool invertParameterWeight;
  [Separator("Resulting Weight")]
  public float weight;

  public float CalculateWeight(Animator animator)
  {
    if ((UnityEngine.Object) animator == (UnityEngine.Object) null || !animator.IsInitialized())
      return this.weight = 0.0f;
    this.weight = this.selfWeight;
    if (!string.IsNullOrEmpty(this.stateName))
    {
      float stateApparentWeight = animator.GetStateApparentWeight(this.stateName, this.stateLayer, this.layerMask);
      if (this.invertStateWeight)
        this.weight *= 1f - stateApparentWeight;
      else
        this.weight *= stateApparentWeight;
    }
    if (!string.IsNullOrEmpty(this.parameterName))
    {
      float num = Mathf.Clamp01(animator.GetFloat(this.parameterName));
      if (this.invertParameterWeight)
        this.weight *= 1f - num;
      else
        this.weight *= num;
    }
    return this.weight;
  }
}
