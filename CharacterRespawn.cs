// Decompiled with JetBrains decompiler
// Type: CharacterRespawn
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Damaging.Statuses;
using CodeHatch.Engine.Behaviours;
using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Core.Input;
using CodeHatch.Engine.Networking;
using CodeHatch.Networking.Events;
using CodeHatch.Networking.Events.Players;
using CodeHatch.Spawning;
using Oxide.Core;
using UnityEngine;

#nullable disable
public class CharacterRespawn : EntityBehaviour, IResetable
{
  private const string respawnSound = "Player - Respawn";
  public KeyButton respawnKeyButton = new KeyButton("Respawn", KeyCode.R);
  private int _respawnCount;

  public KeyButton RespawnKey => this.respawnKeyButton;

  public int RespawnCount => this._respawnCount;

  public void Awake()
  {
    EventManager.Subscribe<PlayerRespawnAtBaseEvent>(new EventSubscriber<PlayerRespawnAtBaseEvent>(this.OnPlayerRespawnAtBase), EventHandlerOrder.Early);
    EventManager.Subscribe<PlayerRespawnAtBedEvent>(new EventSubscriber<PlayerRespawnAtBedEvent>(this.OnPlayerRespawnAtBed), EventHandlerOrder.Early);
    EventManager.Subscribe<PlayerRespawnRandomlyEvent>(new EventSubscriber<PlayerRespawnRandomlyEvent>(this.OnPlayerRespawnRandomly), EventHandlerOrder.Early);
    EventManager.Subscribe<PlayerRespawnNormalEvent>(new EventSubscriber<PlayerRespawnNormalEvent>(this.OnPlayerRespawnNormal), EventHandlerOrder.Early);
  }

  public void OnDestroy()
  {
    EventManager.Unsubscribe<PlayerRespawnAtBaseEvent>(new EventSubscriber<PlayerRespawnAtBaseEvent>(this.OnPlayerRespawnAtBase));
    EventManager.Unsubscribe<PlayerRespawnAtBedEvent>(new EventSubscriber<PlayerRespawnAtBedEvent>(this.OnPlayerRespawnAtBed));
    EventManager.Unsubscribe<PlayerRespawnRandomlyEvent>(new EventSubscriber<PlayerRespawnRandomlyEvent>(this.OnPlayerRespawnRandomly));
    EventManager.Unsubscribe<PlayerRespawnNormalEvent>(new EventSubscriber<PlayerRespawnNormalEvent>(this.OnPlayerRespawnNormal));
  }

  public void Respawn()
  {
    if (!this.Entity.IsLocallyOwned && !Player.IsLocalServer)
      return;
    EventManager.CallEvent((BaseEvent) new PlayerRespawnNormalEvent(this.Entity.OwnerId, this.Entity.Position, this.Entity.MainTransform.rotation));
  }

  private void OnPlayerRespawnNormal(PlayerRespawnNormalEvent e)
  {
    if ((long) e.Player.Id != (long) this.Entity.OwnerId)
      return;
    if (Player.IsLocalServer)
      e.Position = SpawnpointManager.GetSpawnPoint(this.Entity);
    Interface.CallHook("OnPlayerRespawn", (object) e);
    this.RespawnInternal(e.Position, e.Rotation);
  }

  public void RespawnRandomly()
  {
    if (!this.Entity.IsLocallyOwned && !Player.IsLocalServer)
      return;
    EventManager.CallEvent((BaseEvent) new PlayerRespawnRandomlyEvent(this.Entity.OwnerId, this.Entity.Position, this.Entity.MainTransform.rotation));
  }

  private void OnPlayerRespawnRandomly(PlayerRespawnRandomlyEvent e)
  {
    if ((long) e.Player.Id != (long) this.Entity.OwnerId)
      return;
    if (Player.IsLocalServer)
      e.Position = SpawnpointManager.GetRandomSpawnPoint();
    Interface.CallHook("OnPlayerRespawn", (object) e);
    this.RespawnInternal(e.Position, e.Rotation);
  }

  public void RespawnAtBase()
  {
    if (!this.Entity.IsLocallyOwned && !Player.IsLocalServer)
      return;
    EventManager.CallEvent((BaseEvent) new PlayerRespawnAtBaseEvent(this.Entity.OwnerId, this.Entity.Position, this.Entity.MainTransform.rotation));
  }

  private void OnPlayerRespawnAtBase(PlayerRespawnAtBaseEvent e)
  {
    Player player = e.Player;
    if ((long) player.Id != (long) this.Entity.OwnerId)
      return;
    if (Player.IsLocalServer)
    {
      Vector3 zero = Vector3.zero;
      if (SavedSpawnPointsManager.Instance.ConsumeSpawnPoint(player.Id, ref zero))
      {
        e.Position = zero;
        Interface.CallHook("OnPlayerRespawn", (object) e);
      }
      else
      {
        e.Cancel(e.Player.Name + " tried to respawn at a bed when no bed spawns were available.");
        return;
      }
    }
    this.RespawnInternal(e.Position, e.Rotation);
  }

  public void RespawnAtBed()
  {
    if (!this.Entity.IsLocalPlayer)
      return;
    EventManager.CallEvent((BaseEvent) new PlayerRespawnAtBedEvent(this.Entity.OwnerId, this.Entity.Position, this.Entity.MainTransform.rotation));
  }

  private void OnPlayerRespawnAtBed(PlayerRespawnAtBedEvent e)
  {
    Player player = e.Player;
    if ((long) player.Id != (long) this.Entity.OwnerId)
      return;
    if (Player.IsLocalServer)
    {
      Vector3 zero = Vector3.zero;
      if (SavedSpawnPointsManager.Instance.ConsumeSpawnPoint(player.Id, ref zero))
      {
        e.Position = zero;
        Interface.CallHook("OnPlayerRespawn", (object) e);
      }
      else
      {
        e.Cancel(e.Player.Name + " tried to respawn at a bed when no bed spawns were available.");
        return;
      }
    }
    this.RespawnInternal(e.Position, e.Rotation);
  }

  private void RespawnInternal(Vector3 posiiton, Quaternion rotation)
  {
    this.Entity.GetOrCreate<CharacterTeleport>().Teleport(posiiton);
    this.Revive();
  }

  private void Revive()
  {
    IHealth health = this.Entity.TryGet<IHealth>();
    health?.Revive(health.MaxHealth);
    PlayerStatusEffectsManager statusEffectsManager = this.Entity.GetOrCreate<PlayerStatusEffectsManager>();
    if ((Object) statusEffectsManager != (Object) null)
      statusEffectsManager.DeactivateAll();
    Nourishment nourishment = this.Entity.TryGet<Nourishment>();
    if ((Object) nourishment != (Object) null)
      nourishment.Current = nourishment.NourishmentDefault;
    if (this.Entity.IsLocallyOwned)
      AudioController.Play("Player - Respawn", this.Entity.MainTransform);
    ++this._respawnCount;
  }

  public ResetableOrder ResetOrder => ResetableOrder.Default;

  public void OnClearScene()
  {
  }

  public void OnResetScene() => this.Respawn();

  public void OnSwitch() => this.Respawn();
}
