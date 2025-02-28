// Decompiled with JetBrains decompiler
// Type: DeerRunAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.AI;
using CodeHatch.Engine.Core.Cache;
using UnityEngine;

#nullable disable
public class DeerRunAnimation : EntityBehaviour
{
  public AnimationClip gallopAnimation;
  public AnimationClip runningAnimation;
  public float runAtSpeed;
  public float gallopAnimSpeed;
  public float runningAnimSpeed;
  public MovementAnimationController animController;
  private Animation anim;
  private Rigidbody mainRigidbody;
  private Transform rootBodyDirection;

  public void Start()
  {
    this.anim = this.Entity.Get<Animation>();
    this.mainRigidbody = this.Entity.Get<MainRigidbody>().GetComponent<Rigidbody>();
    this.rootBodyDirection = this.Entity.MainTransform;
  }

  public void Update()
  {
    if ((double) Vector3.Dot(this.mainRigidbody.velocity, this.rootBodyDirection.forward) < (double) this.runAtSpeed)
    {
      this.anim[this.runningAnimation.name].weight = Mathf.Lerp(this.anim[this.runningAnimation.name].weight, 0.0f, 1f);
      this.animController.RunAnimation = this.gallopAnimation;
      this.animController.RunSpeedMultiplier = this.gallopAnimSpeed;
    }
    else
    {
      this.anim[this.gallopAnimation.name].weight = Mathf.Lerp(this.anim[this.gallopAnimation.name].weight, 0.0f, 1f);
      this.animController.RunAnimation = this.runningAnimation;
      this.animController.RunSpeedMultiplier = this.runningAnimSpeed;
    }
  }
}
