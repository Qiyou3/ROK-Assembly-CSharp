// Decompiled with JetBrains decompiler
// Type: DamagableContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Damaging;
using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Core.Utility.Attributes;
using CodeHatch.Engine.Networking;
using CodeHatch.ItemContainer;
using CodeHatch.Networking.Events;
using CodeHatch.Networking.Events.Entities;
using CodeHatch.Networking.Events.Item;
using UnityEngine;

#nullable disable
public class DamagableContainer : EntityBehaviour
{
  private const float DISTANCE_LOOT = 10f;
  public bool CollectWhenDead = true;
  [Range(1f, 100f)]
  public int ItemsToCollectPerDamage = 1;
  [FlagEnum(typeof (DamageType))]
  [SerializeField]
  public DamageType GiveOnDamageType;
  private Container _container;

  public Container Container
  {
    get
    {
      if ((Object) this._container == (Object) null)
        this._container = this.GetComponent<Container>();
      return this._container;
    }
  }

  public void OnEnable()
  {
    if (!Player.IsLocalServer)
      return;
    EventManager.Subscribe<EntityDamageEvent>(new EventSubscriber<EntityDamageEvent>(this.OnEntityDamage));
  }

  public void OnDisable()
  {
    if (!Player.IsLocalServer)
      return;
    EventManager.Unsubscribe<EntityDamageEvent>(new EventSubscriber<EntityDamageEvent>(this.OnEntityDamage));
  }

  private void OnEntityDamage(EntityDamageEvent theEvent)
  {
    if (theEvent.Cancelled || (Object) theEvent.Entity != (Object) this.Entity || (Object) theEvent.Sender.Entity == (Object) null || (this.GiveOnDamageType & theEvent.Damage.DamageTypes) == DamageType.Unknown || !theEvent.Sender.Entity.Has<PlayerEntity>() || (double) Vector3.Distance(theEvent.Sender.Entity.Position, this.Entity.Position) > 10.0)
      return;
    IHealth health = this.Entity.TryGet<IHealth>();
    if (this.CollectWhenDead && (health == null || !health.IsDead))
      return;
    for (int index = 0; index < this.ItemsToCollectPerDamage; ++index)
      this.GiveItem(theEvent.Sender, theEvent.Damage == null ? 1 : (int) theEvent.Damage.Amount);
  }

  private void GiveItem(Player player, int rollPower)
  {
    if ((Object) this.Container == (Object) null || this.Container.Contents == null || this.Container.Contents.IsEmpty)
      return;
    InvGameItemStack stack = (InvGameItemStack) null;
    int num = Random.Range(0, this.Container.Contents.InnerSlotCount);
    for (int slot = num; slot < this.Container.Contents.InnerSlotCount; ++slot)
    {
      stack = this.Container.Contents.GetItem(slot);
      if (stack != null)
        break;
    }
    if (stack == null)
    {
      for (int slot = 0; slot < num; ++slot)
      {
        stack = this.Container.Contents.GetItem(slot);
        if (stack != null)
          break;
      }
    }
    if (stack == null)
      return;
    int quantity = Mathf.Min(stack.StackAmount, Random.Range(1, 1 + rollPower));
    ItemPassEvent theEvent = new ItemPassEvent(this.Container.Contents.SplitItem(stack, quantity, true), player, "Loot");
    theEvent.Recipients.Clear();
    theEvent.Recipients.Add(player);
    EventManager.CallEvent((BaseEvent) theEvent);
  }
}
