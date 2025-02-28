// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.AnimationController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Engine.Core.Cache;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class AnimationController : EntityBehaviour
  {
    public AnimationClip DeathAnimation;
    public AnimationClip[] HitAnimations;

    public void PlayAnimataion(AnimationController.AnimationType type)
    {
      Animation animation = this.Entity.Get<Animation>();
      AnimationState animationState;
      switch (type)
      {
        case AnimationController.AnimationType.Hit:
          animationState = animation[this.HitAnimations.GetRandom<AnimationClip>().name];
          break;
        case AnimationController.AnimationType.Death:
          animationState = animation[this.DeathAnimation.name];
          break;
        default:
          return;
      }
      animationState.layer = 2;
      animationState.enabled = true;
      animationState.wrapMode = WrapMode.Once;
      animationState.weight = 1f;
      animationState.blendMode = AnimationBlendMode.Blend;
    }

    public enum AnimationType
    {
      Hit,
      Death,
    }
  }
}
