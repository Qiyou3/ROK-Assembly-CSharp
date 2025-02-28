// Decompiled with JetBrains decompiler
// Type: Carryable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Core.Interaction;
using CodeHatch.Engine.Core.Interaction.Attributes;
using CodeHatch.Engine.Core.Interaction.Players;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (uLinkNetworkView))]
[RequireComponent(typeof (Rigidbody))]
public class Carryable : uLinkEntityBehaviour, IInteractable, ICarryable
{
  private PhysicsGrabber _grabber;

  public InteractionState InteractionDisabled => InteractionState.Enabled;

  public bool IsolateInteraction => false;

  public void Activate(out bool possessActivator)
  {
    if (!Entity.LocalPlayerExists)
    {
      possessActivator = false;
    }
    else
    {
      PhysicsGrabber physicsGrabber = Entity.LocalPlayer.Get<PhysicsGrabber>();
      Vector3 grabPosition = physicsGrabber.GetGrabPosition();
      physicsGrabber.GrabEntity(this.Entity, grabPosition);
      possessActivator = true;
    }
  }

  public void Deactivate() => Entity.GetLocal().Get<PhysicsGrabber>().DropObject();

  [Player(CodeHatch.Engine.Core.Interaction.Players.Key.PickUp, Gesture.Click, false)]
  [Interact]
  [Tutorial("Carry")]
  public void Carry(BaseInteractController controller)
  {
    Entity entity = controller.Entity;
    if (!((UnityEngine.Object) entity != (UnityEngine.Object) null))
      return;
    PhysicsGrabber physicsGrabber = entity.Get<PhysicsGrabber>();
    if (!((UnityEngine.Object) this._grabber == (UnityEngine.Object) null) || !((UnityEngine.Object) physicsGrabber != (UnityEngine.Object) null))
      return;
    this._grabber = physicsGrabber;
    this._grabber.GrabEntity(this.Entity, this._grabber.GetGrabPosition());
    controller.Lock((IInteractable) this, new System.Action(this.Release));
  }

  [UnlockOnly]
  [Tutorial("Release")]
  [Interact]
  [Player(CodeHatch.Engine.Core.Interaction.Players.Key.PickUp, Gesture.Click, false)]
  public void Release()
  {
    if (!((UnityEngine.Object) this._grabber != (UnityEngine.Object) null))
      return;
    this._grabber.DropObject();
    this._grabber = (PhysicsGrabber) null;
  }
}
