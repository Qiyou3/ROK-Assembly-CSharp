// Decompiled with JetBrains decompiler
// Type: IKHands
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class IKHands : MonoBehaviour
{
  public Transform leftHandObj;
  public Transform rightHandObj;
  public Transform attachLeft;
  public Transform attachRight;
  public float leftHandPositionWeight;
  public float leftHandRotationWeight;
  public float rightHandPositionWeight;
  public float rightHandRotationWeight;
  private Animator animator;

  private void Start() => this.animator = this.gameObject.GetComponent<Animator>();

  private void OnAnimatorIK(int layerIndex)
  {
    if ((Object) this.leftHandObj != (Object) null)
    {
      this.animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, this.leftHandPositionWeight);
      this.animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, this.leftHandRotationWeight);
      this.animator.SetIKPosition(AvatarIKGoal.LeftHand, this.attachLeft.position);
      this.animator.SetIKRotation(AvatarIKGoal.LeftHand, this.attachLeft.rotation);
    }
    if (!((Object) this.rightHandObj != (Object) null))
      return;
    this.animator.SetIKPositionWeight(AvatarIKGoal.RightHand, this.rightHandPositionWeight);
    this.animator.SetIKRotationWeight(AvatarIKGoal.RightHand, this.rightHandRotationWeight);
    this.animator.SetIKPosition(AvatarIKGoal.RightHand, this.attachRight.position);
    this.animator.SetIKRotation(AvatarIKGoal.RightHand, this.attachRight.rotation);
  }
}
