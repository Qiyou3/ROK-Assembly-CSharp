// Decompiled with JetBrains decompiler
// Type: ChainsawRevAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Modules.Inventory.Holdables;
using UnityEngine;

#nullable disable
public class ChainsawRevAnimation : EntityBehaviour, IHolderListener
{
  public string animationName = "Rev";
  public float directionChangeHalflife = 0.1f;

  public Entity HeldBy { get; set; }

  public bool Cutting
  {
    set
    {
      AnimationState animationState = this.GetComponent<Animation>()[this.animationName];
      animationState.wrapMode = WrapMode.ClampForever;
      int target = !value ? -1 : 1;
      animationState.speed = HalfLife.GainLoss(animationState.speed, (float) target, this.directionChangeHalflife, this.directionChangeHalflife);
      animationState.time = Mathf.Clamp(animationState.time, 0.0f, animationState.length);
      if (animationState.enabled)
        return;
      this.GetComponent<Animation>().Play(this.animationName);
    }
  }

  public void Update()
  {
    if ((Object) this.HeldBy == (Object) null)
      return;
    this.Cutting = this.HeldBy.Get<WeaponBridge>().IsFiring;
  }
}
