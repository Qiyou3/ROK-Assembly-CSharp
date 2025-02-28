// Decompiled with JetBrains decompiler
// Type: BlueprintInstanceTimedCollection
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Interaction;
using CodeHatch.Engine.Core.Interaction.Attributes;
using CodeHatch.Engine.Core.Interaction.Players;
using UnityEngine;

#nullable disable
public class BlueprintInstanceTimedCollection : BlueprintInstance, IInteractable
{
  public float pickUpLockTime = 20f;
  private float lastPlaceTime;

  public bool CanPickUp
  {
    get => (double) Time.time - (double) this.lastPlaceTime > (double) this.pickUpLockTime;
  }

  [Player(CodeHatch.Engine.Core.Interaction.Players.Key.PickUp, Gesture.Hold, false)]
  [Interact]
  [Tutorial("(Hold)\nCollect Item")]
  public override void Collect()
  {
    if (!this.CanPickUp)
      Console.AddMessage(this.BlockedMessage());
    else
      base.Collect();
  }

  public string BlockedMessage()
  {
    return "Collecting locked for " + (object) Mathf.CeilToInt(this.pickUpLockTime - Time.time + this.lastPlaceTime) + " seconds";
  }

  public void OnEnable() => this.lastPlaceTime = Time.time;
}
