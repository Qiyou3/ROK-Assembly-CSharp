// Decompiled with JetBrains decompiler
// Type: AnimatorStateManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;
using UnityEngine;

#nullable disable
public class AnimatorStateManager : EntityBehaviour
{
  private Animator _animator;
  public string layerName;
  private AnimatorStateManager.State _state;
  private bool _isAssigned;

  public Animator MyAnimator
  {
    get
    {
      if ((Object) this._animator == (Object) null)
      {
        Entity entity = this.TryGetEntity();
        if ((Object) entity != (Object) null)
          this._animator = entity.Get<Animator>();
      }
      return this._animator;
    }
  }

  public bool IsStateAssigned => this._isAssigned;

  public void AssignState(AnimatorStateManager.State state)
  {
    this._state = state;
    this._isAssigned = true;
  }

  public void Update()
  {
    if (!this._isAssigned)
      return;
    this.MyAnimator.CrossFade(this.layerName + "." + this._state.ToString(), 0.25f);
    this._isAssigned = false;
  }

  public enum State
  {
    Locomotion,
    Tumble,
    Sitting,
  }
}
