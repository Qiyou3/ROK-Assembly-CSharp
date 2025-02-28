// Decompiled with JetBrains decompiler
// Type: AnimatorSnapshot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using JetBrains.Annotations;
using System;
using UnityEngine;

#nullable disable
public class AnimatorSnapshot
{
  private readonly AnimatorSnapshot.StateSnapshot[] _stateSnapshots;
  private readonly RuntimeAnimatorController _animatorController;

  public AnimatorSnapshot([NotNull] Animator sourceAnimator)
  {
    this._animatorController = !((UnityEngine.Object) sourceAnimator == (UnityEngine.Object) null) ? sourceAnimator.runtimeAnimatorController : throw new ArgumentNullException(nameof (sourceAnimator));
    this._stateSnapshots = new AnimatorSnapshot.StateSnapshot[sourceAnimator.layerCount];
    for (int layerIndex = 0; layerIndex < sourceAnimator.layerCount; ++layerIndex)
    {
      AnimatorStateInfo animatorStateInfo = sourceAnimator.GetCurrentAnimatorStateInfo(layerIndex);
      this._stateSnapshots[layerIndex].StateHash = animatorStateInfo.fullPathHash;
      this._stateSnapshots[layerIndex].StateTime = Mathf.Max(0.0f, animatorStateInfo.normalizedTime - Time.deltaTime / animatorStateInfo.length);
    }
  }

  public void Apply([NotNull] Animator destinationAnimator)
  {
    if ((UnityEngine.Object) destinationAnimator == (UnityEngine.Object) null)
      throw new ArgumentNullException(nameof (destinationAnimator));
    if ((UnityEngine.Object) destinationAnimator.runtimeAnimatorController != (UnityEngine.Object) this._animatorController)
    {
      this.LogError<AnimatorSnapshot>("Cannot apply snapshot because the destination animator's runtime animator controller does not match.");
    }
    else
    {
      for (int layer = 0; layer < destinationAnimator.layerCount && layer < this._stateSnapshots.Length; ++layer)
        destinationAnimator.Play(this._stateSnapshots[layer].StateHash, layer, this._stateSnapshots[layer].StateTime);
      destinationAnimator.Update(0.0f);
    }
  }

  private struct StateSnapshot
  {
    public int StateHash;
    public float StateTime;
  }
}
