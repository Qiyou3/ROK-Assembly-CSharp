// Decompiled with JetBrains decompiler
// Type: Health
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch;
using CodeHatch.Common;
using CodeHatch.Common.Attributes;
using CodeHatch.Damaging;
using CodeHatch.Engine.Entities.Definitions;
using CodeHatch.Engine.Events.Prefab;
using CodeHatch.Engine.Modding.Abstract;
using CodeHatch.Engine.Networking;
using CodeHatch.Engine.Serialization;
using CodeHatch.Networking;
using CodeHatch.Networking.Events;
using CodeHatch.Networking.Events.Entities;
using CodeHatch.Networking.Events.Entities.Enemy;
using CodeHatch.Networking.Sync;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Syncable]
public class Health : EntityHealth, IHealth, IResetable, IModable, ISerializable
{
  [Designer]
  [MinValue(0.0f)]
  [Tooltip("Maximum possible health of this object. CurrentHealth is reset to this on load/on instantiate.")]
  [SerializeField]
  private float _maxhealth = 50f;
  [Tooltip("Current health of this object. Is reset to MaxHealth on load/instantiate.")]
  [Syncable]
  [MinValue(0.0f)]
  [SerializeField]
  private float _currentHealth;
  [SerializeField]
  [Tooltip("If true the entity cannot take damage.")]
  private bool _isInvincible;
  [SerializeField]
  [Tooltip("If true the entity cannot be killed. Only Destroyed.")]
  private bool _preventDeath;
  [NoEdit]
  [SerializeField]
  private bool _hasDied;
  [Designer]
  public float healthRegenRate;
  private Damage _lastDamage;
  private bool _started;

  public float MaxHealth
  {
    get => this._maxhealth;
    set
    {
      this._maxhealth = (double) value >= 0.0 ? value : 0.0f;
      if ((double) this._currentHealth <= (double) this._maxhealth)
        return;
      this.CurrentHealth = this._maxhealth;
    }
  }

  public float CurrentHealth
  {
    get => this._currentHealth;
    set
    {
      if (this.IsInvincible && (double) value <= (double) this._currentHealth)
        return;
      if ((double) value <= 0.0)
        this.Kill();
      else if ((double) value > (double) this._maxhealth)
      {
        this._currentHealth = this._maxhealth;
        this._hasDied = false;
      }
      else
      {
        this._currentHealth = value;
        this._hasDied = false;
      }
    }
  }

  public float CurrentHealthPercent
  {
    get
    {
      return (double) this._maxhealth > (double) Mathf.Epsilon ? this._currentHealth / this._maxhealth : 0.0f;
    }
  }

  public bool IsInvincible
  {
    get => this._isInvincible;
    set => this._isInvincible = value;
  }

  public bool PreventDeath
  {
    get => this._preventDeath;
    set => this._preventDeath = value;
  }

  public bool IsDead => (double) this._currentHealth <= 0.0 && !this.IsInvincible;

  public bool IsAlive => !this.IsDead;

  public string Identifier => this.GetType().Name;

  public virtual void Awake() => this.CurrentHealth = this.MaxHealth;

  public virtual void Start()
  {
    this._started = true;
    SyncManager.Register<Health>(this);
    if (this.HasEntity)
      EventManager.Subscribe<EntityDamageEvent>(new EventSubscriber<EntityDamageEvent>(this.OnEntityDamage), EventHandlerOrder.Early);
    else
      this.LogError<Health>("Health attached to object {0} without an entity.", (object) this.name);
  }

  public virtual void OnDestroy()
  {
    if (!this._started)
      return;
    SyncManager.Unregister((object) this);
    if (this.HasEntity)
      EventManager.Unsubscribe<EntityDamageEvent>(new EventSubscriber<EntityDamageEvent>(this.OnEntityDamage));
    else
      this.LogError<Health>("Health attached to object {0} without an entity.", (object) this.name);
  }

  public virtual void OnDespawned() => this.CurrentHealth = this.MaxHealth;

  public virtual void Update()
  {
    if ((double) this.healthRegenRate <= 0.0 || this.IsDead)
      return;
    this.CurrentHealth += this.healthRegenRate * Time.deltaTime;
  }

  public void OnValidate()
  {
    if (!Application.isPlaying)
      this.CurrentHealth = this.MaxHealth;
    else
      this.CurrentHealth = this._currentHealth;
  }

  public void uLink_OnSerializeNetworkViewOwner(IStream stream, uLink.NetworkMessageInfo info)
  {
    this.uLink_OnSerializeNetworkView(stream, info);
  }

  public void uLink_OnSerializeNetworkView(IStream stream, uLink.NetworkMessageInfo info)
  {
    if (stream.IsWriting)
      stream.Write<float>(this.CurrentHealth);
    else
      this.CurrentHealth = stream.Read<float>();
  }

  public void Serialize(IStream stream) => stream.Write<float>(this._currentHealth);

  public void Deserialize(IStream stream) => this._currentHealth = stream.Read<float>();

  public void OnResetScene() => this.CurrentHealth = this._maxhealth;

  public void OnClearScene()
  {
  }

  public void OnSwitch()
  {
  }

  public ResetableOrder ResetOrder => ResetableOrder.Default;

  protected virtual void OnEntityDamage(EntityDamageEvent e)
  {
    if (e.Cancelled || (UnityEngine.Object) e.Entity != (UnityEngine.Object) this.Entity)
      return;
    if (Player.IsLocalServer)
      e.Recipients = EventReceiverController.GetEventRecievers(e.Entity);
    this.InvokeDamage(e);
    this._lastDamage = e.Damage;
    this.CurrentHealth -= this._lastDamage.Amount;
  }

  public override void Revive() => this.Revive(this.MaxHealth);

  public virtual void Revive(float newHealth)
  {
    this.CurrentHealth = newHealth;
    if (this.DeathBehaviour != EntityHealth.DeathBehaviours.Disable)
      return;
    this.Entity.gameObject.SetActive(true);
  }

  public void SetHealth(float newHealth) => this.CurrentHealth = newHealth;

  public void UpdateMaxHealth(float newMax, bool updateHealth)
  {
    this.MaxHealth = newMax;
    if (!updateHealth)
      return;
    this.Revive(this.MaxHealth);
  }

  public override bool Kill() => this.Kill((Damage) null);

  public virtual bool Kill(Damage killingDamage)
  {
    if (this.PreventDeath || this._hasDied)
      return false;
    if (killingDamage == null)
      killingDamage = this._lastDamage;
    if (killingDamage != null)
      killingDamage.IsFatal = true;
    this._lastDamage = killingDamage;
    this._hasDied = true;
    this.IsInvincible = false;
    this._currentHealth = 0.0f;
    if (Player.IsLocalServer)
      EventManager.CallEvent((BaseEvent) new EnemyDeathEvent(this.Entity, killingDamage));
    EntityDeathEvent entityDeathEvent = new EntityDeathEvent(this.Entity, killingDamage, (Action<BaseEvent>) null);
    if (this.Entity.IsLocallyOwned)
      EventManager.CallEvent((BaseEvent) entityDeathEvent);
    this.InvokeDeath(entityDeathEvent);
    if (this.DeathBehaviour == EntityHealth.DeathBehaviours.Disable)
      this.Entity.gameObject.SetActive(false);
    else if (this.DeathBehaviour == EntityHealth.DeathBehaviours.Destroy && Player.IsLocalServer)
      CustomNetworkInstantiate.NetworkDestroy(this.Entity.gameObject, InformationType.Kill);
    return true;
  }

  public string ModHandlerName => nameof (Health);

  public void GetModDefaults(IList<ModEntry> defaultModEntries)
  {
    defaultModEntries.Add(new ModEntry("MaxHealth", (object) this.MaxHealth));
    defaultModEntries.Add(new ModEntry("IsInvincible", (object) this.IsInvincible));
  }

  public void ApplyMod(string key, object value)
  {
    if (value == null)
      return;
    string key1 = key;
    if (key1 == null)
      return;
    // ISSUE: reference to a compiler-generated field
    if (Health.\u003C\u003Ef__switch\u0024map3 == null)
    {
      // ISSUE: reference to a compiler-generated field
      Health.\u003C\u003Ef__switch\u0024map3 = new System.Collections.Generic.Dictionary<string, int>(2)
      {
        {
          "MaxHealth",
          0
        },
        {
          "IsInvincible",
          1
        }
      };
    }
    int num;
    // ISSUE: reference to a compiler-generated field
    if (!Health.\u003C\u003Ef__switch\u0024map3.TryGetValue(key1, out num))
      return;
    switch (num)
    {
      case 0:
        this.MaxHealth = (float) value;
        SetHealthFromBundle component = this.GetComponent<SetHealthFromBundle>();
        if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
          break;
        component.bundle.Health = this.MaxHealth;
        break;
      case 1:
        this.IsInvincible = (bool) value;
        break;
    }
  }
}
