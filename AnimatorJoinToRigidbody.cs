// Decompiled with JetBrains decompiler
// Type: AnimatorJoinToRigidbody
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class AnimatorJoinToRigidbody : AnimatorBehaviour
{
  public Transform myPivot;
  public Transform otherPivot;
  public Transform externalPivot;

  public override void Initialize(AnimatorBehaviourManager manager)
  {
    manager.AddPass(new System.Action(this.PostAnimate), AnimatorBehaviourPass.PostAnimate, 2001);
  }

  private void PostAnimate()
  {
    if ((UnityEngine.Object) this.myPivot == (UnityEngine.Object) null)
    {
      this.LogError<AnimatorJoinToRigidbody>("myPivot == null", (object) this);
    }
    else
    {
      if ((UnityEngine.Object) this.externalPivot != (UnityEngine.Object) null)
        this.externalPivot.position = this.myPivot.position;
      if ((UnityEngine.Object) this.otherPivot == (UnityEngine.Object) null)
        return;
      this.Manager.SetAvatarPosition(this.AvatarTransform.position + (this.otherPivot.position - this.myPivot.position));
    }
  }
}
