// Decompiled with JetBrains decompiler
// Type: HealthStatusEffects
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Cache;
using CodeHatch.Damaging.Statuses;
using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Entities.Definitions;
using CodeHatch.Networking.Events;
using CodeHatch.Networking.Events.Players;
using FullInspector;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class HealthStatusEffects : BaseBehavior, IEntityAware
{
  public List<HealthStatusEffects.EffectSetting> Effects = new List<HealthStatusEffects.EffectSetting>();
  private GetEntity _entity;

  public void Start()
  {
    EventManager.Subscribe<PlayerDamageEvent>(new EventSubscriber<PlayerDamageEvent>(this.OnDamage));
    this.Entity.GetOrCreate<PlayerStatusEffectsManager>();
  }

  public void OnDestroy()
  {
    EventManager.Unsubscribe<PlayerDamageEvent>(new EventSubscriber<PlayerDamageEvent>(this.OnDamage));
  }

  public void OnDamage(PlayerDamageEvent e)
  {
    if (!this.Entity.IsLocallyOwned || (Object) Entity.LocalPlayer != (Object) this.Entity || !e.Player.IsLocal)
      return;
    PlayerHealth playerHealth = this.Entity.Get<PlayerHealth>();
    for (int index = 0; index < this.Effects.Count; ++index)
    {
      StatusEffect effect = (StatusEffect) null;
      float severity = 0.0f;
      float duration = 0.0f;
      if (this.Effects[index].TriggerOnEachHit)
      {
        effect = this.Effects[index].Effect;
        severity = effect.Severity * (e.Damage.Amount / playerHealth.MaxHealth);
        duration = effect.Duration;
      }
      else
      {
        float num1 = 0.0f;
        float num2 = 0.0f;
        switch (this.Effects[index].TriggeringRegion)
        {
          case HealthStatusEffects.HealthRegion.Head:
            num1 = playerHealth.HeadHealth.CurrentHealth;
            num2 = playerHealth.HeadHealth.MaxHealth;
            break;
          case HealthStatusEffects.HealthRegion.Torso:
            num1 = playerHealth.TorsoHealth.CurrentHealth;
            num2 = playerHealth.TorsoHealth.MaxHealth;
            break;
          case HealthStatusEffects.HealthRegion.Legs:
            num1 = playerHealth.LegsHealth.CurrentHealth;
            num2 = playerHealth.LegsHealth.MaxHealth;
            break;
        }
        float num3 = (float) ((double) num2 * (double) this.Effects[index].ThresholdPercentage / 100.0);
        if ((double) num1 <= (double) num3)
        {
          effect = this.Effects[index].Effect;
          severity = (float) ((double) effect.Severity * 1.0 - (double) num1 / (double) num2);
          duration = effect.Duration;
        }
      }
      if (effect != null)
      {
        PlayerStatusEffectsManager statusEffectsManager = this.Entity.Get<PlayerStatusEffectsManager>();
        if (this.Effects[index].ScaleSeverityToHealth)
          statusEffectsManager.SetEffectState(this.Effects[index].Effect.GetType(), severity, duration, true);
        else
          statusEffectsManager.SetEffectState(effect, true);
      }
    }
  }

  public void Update()
  {
    if (!this.Entity.IsLocallyOwned)
      return;
    PlayerHealth playerHealth = this.Entity.Get<PlayerHealth>();
    for (int index = 0; index < this.Effects.Count; ++index)
    {
      if (!this.Effects[index].TriggerOnEachHit)
      {
        float num1 = 0.0f;
        float num2 = 0.0f;
        switch (this.Effects[index].TriggeringRegion)
        {
          case HealthStatusEffects.HealthRegion.Head:
            num1 = playerHealth.HeadHealth.CurrentHealth;
            num2 = playerHealth.HeadHealth.MaxHealth;
            break;
          case HealthStatusEffects.HealthRegion.Torso:
            num1 = playerHealth.TorsoHealth.CurrentHealth;
            num2 = playerHealth.TorsoHealth.MaxHealth;
            break;
          case HealthStatusEffects.HealthRegion.Legs:
            num1 = playerHealth.LegsHealth.CurrentHealth;
            num2 = playerHealth.LegsHealth.MaxHealth;
            break;
        }
        float num3 = (float) ((double) num2 * (double) this.Effects[index].ThresholdPercentage / 100.0);
        if ((double) num1 <= (double) num3)
        {
          PlayerStatusEffectsManager statusEffectsManager = this.Entity.Get<PlayerStatusEffectsManager>();
          StatusEffect effect = this.Effects[index].Effect;
          if (this.Effects[index].ScaleSeverityToHealth)
          {
            float severity = (float) ((double) effect.Severity * 1.0 - (double) num1 / (double) num2);
            float duration = effect.Duration;
            statusEffectsManager.SetEffectState(this.Effects[index].Effect.GetType(), severity, duration, true);
          }
          else
            statusEffectsManager.SetEffectState(effect, true);
        }
      }
    }
  }

  public Entity Entity
  {
    get
    {
      if (this._entity == null)
        this._entity = new GetEntity((MonoBehaviour) this);
      return this._entity.Get();
    }
  }

  public enum HealthRegion
  {
    Head,
    Torso,
    Legs,
  }

  public class EffectSetting
  {
    [UnityEngine.Tooltip("Overrides other settings to apply the effect every time the player takes damage.")]
    public bool TriggerOnEachHit;
    [UnityEngine.Tooltip("Scales the defined severity proportionately to health.")]
    public bool ScaleSeverityToHealth;
    public HealthStatusEffects.HealthRegion TriggeringRegion;
    [Range(0.0f, 100f)]
    public float ThresholdPercentage;
    public StatusEffect Effect;
  }
}
