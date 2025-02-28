// Decompiled with JetBrains decompiler
// Type: DejembeDrumHealthRegenMultiplier
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Modules.PeriodicEffects;
using CodeHatch.Engine.Serialization;

#nullable disable
public class DejembeDrumHealthRegenMultiplier : HealthRegenerationMultiplier
{
  private bool Used_field = true;
  private float Duration_field = 15f;
  private float Value_field = 4f;
  private bool ShowTicker_field = true;
  private ValueDisplayMode DisplayValueAs_field = ValueDisplayMode.Multiplier;
  private string Description_field = "Health Regeneration";
  private string Icon_field = "Dejembe Drum_Icon";
  private EffectTickFunc<float> Tick_field = EffectDefinition<float>.DefaultTickFunc;
  private bool EffectCanStack_field;
  private bool TimersPerStack_field;
  private bool CanExpireWhileOffline_field = true;

  public DejembeDrumHealthRegenMultiplier(int ID)
    : base(ID)
  {
    EffectInstance<float, DejembeDrumHealthRegenMultiplier> effectInstance = new EffectInstance<float, DejembeDrumHealthRegenMultiplier>();
    SerializeFactory.RegisterType<EffectInstance<float, DejembeDrumHealthRegenMultiplier>>(effectInstance, effectInstance.Identifier);
  }

  public override bool Used
  {
    get => this.Used_field;
    protected set => this.Used_field = value;
  }

  public override float Duration
  {
    get => this.Duration_field;
    protected set => this.Duration_field = value;
  }

  public override float Value
  {
    get => this.Value_field;
    set => this.Value_field = value;
  }

  public override bool ShowTicker
  {
    get => this.ShowTicker_field;
    protected set => this.ShowTicker_field = value;
  }

  public override ValueDisplayMode DisplayValueAs => this.DisplayValueAs_field;

  public override string Description
  {
    get => this.Description_field;
    protected set => this.Description_field = value;
  }

  public override string Icon => this.Icon_field;

  public override EffectTickFunc<float> Tick => this.Tick_field;

  public override bool EffectCanStack
  {
    get => this.EffectCanStack_field;
    protected set => this.EffectCanStack_field = value;
  }

  public override bool TimersPerStack
  {
    get => this.TimersPerStack_field;
    protected set => this.TimersPerStack_field = value;
  }

  public override bool CanExpireWhileOffline
  {
    get => this.CanExpireWhileOffline_field;
    protected set => this.CanExpireWhileOffline_field = value;
  }

  public override void Apply(Entity Entity)
  {
    PeriodicEffectsManager.Apply<float, DejembeDrumHealthRegenMultiplier>(this, Entity);
  }

  public override void Remove(Entity Entity, int numberOfStacks)
  {
    PeriodicEffectsManager.Remove<float, DejembeDrumHealthRegenMultiplier>(this, numberOfStacks, Entity);
  }
}
