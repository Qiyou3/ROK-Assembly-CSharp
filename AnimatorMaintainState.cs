// Decompiled with JetBrains decompiler
// Type: AnimatorMaintainState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (Animator))]
public class AnimatorMaintainState : MonoBehaviour
{
  private Animator _animator;
  private AnimatorSnapshot _snapshot;

  private Animator Animator
  {
    get
    {
      if ((Object) this._animator == (Object) null)
        this._animator = this.GetComponent<Animator>();
      return this._animator;
    }
  }

  public void OnEnable()
  {
    if ((Object) this.Animator == (Object) null || this._snapshot == null)
      return;
    this.Animator.SetSnapshot(this._snapshot);
    this._snapshot = (AnimatorSnapshot) null;
  }

  public void OnDisable()
  {
    Animator animator = this.Animator;
    if ((Object) animator == (Object) null)
      return;
    this._snapshot = animator.GetSnapshot();
  }
}
