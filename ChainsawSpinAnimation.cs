// Decompiled with JetBrains decompiler
// Type: ChainsawSpinAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Modules.Inventory.Holdables;
using UnityEngine;

#nullable disable
public class ChainsawSpinAnimation : EntityBehaviour, IHolderListener
{
  public string animationName = "Rev";

  public Entity HeldBy { get; set; }

  public void Update()
  {
    if ((Object) this.HeldBy == (Object) null)
      return;
    Chainsaw chainsaw = this.Entity.TryGet<Chainsaw>();
    if (!((Object) chainsaw != (Object) null))
      return;
    AnimationState animationState = this.GetComponent<Animation>()[this.animationName];
    animationState.wrapMode = WrapMode.Loop;
    animationState.speed = chainsaw.SpinSpeed;
    if (animationState.enabled)
      return;
    this.GetComponent<Animation>().Play(this.animationName);
  }
}
