// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.ModdableClockDrivenWaypoints
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Modding.Abstract;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class ModdableClockDrivenWaypoints : AIBehaviour, IModable
  {
    [JetBrains.Annotations.CanBeNull]
    [CodeHatch.Engine.Core.Utility.Attributes.CanBeNull]
    public ClockDrivenWaypoint Flee;
    [CodeHatch.Engine.Core.Utility.Attributes.CanBeNull]
    [JetBrains.Annotations.CanBeNull]
    public ClockDrivenWaypoint Attack;
    [JetBrains.Annotations.CanBeNull]
    [CodeHatch.Engine.Core.Utility.Attributes.CanBeNull]
    public ClockDrivenWaypoint React;
    [JetBrains.Annotations.CanBeNull]
    [CodeHatch.Engine.Core.Utility.Attributes.CanBeNull]
    public ClockDrivenWaypoint Wander;
    [JetBrains.Annotations.CanBeNull]
    [CodeHatch.Engine.Core.Utility.Attributes.CanBeNull]
    public ChangeWaypointSettingsLowHealth FleeLowHealth;
    [JetBrains.Annotations.CanBeNull]
    private ModdableClockDrivenWaypoints.VelocityWrapper _fleeDay;
    [JetBrains.Annotations.CanBeNull]
    private ModdableClockDrivenWaypoints.VelocityWrapper _fleeNight;
    [JetBrains.Annotations.CanBeNull]
    private ModdableClockDrivenWaypoints.VelocityWrapper _fleeBoth;
    [JetBrains.Annotations.CanBeNull]
    private ModdableClockDrivenWaypoints.VelocityWrapper _attackDay;
    [JetBrains.Annotations.CanBeNull]
    private ModdableClockDrivenWaypoints.VelocityWrapper _attackNight;
    [JetBrains.Annotations.CanBeNull]
    private ModdableClockDrivenWaypoints.VelocityWrapper _reactDay;
    [JetBrains.Annotations.CanBeNull]
    private ModdableClockDrivenWaypoints.VelocityWrapper _reactNight;
    [JetBrains.Annotations.CanBeNull]
    private ModdableClockDrivenWaypoints.VelocityWrapper _wanderDay;
    [JetBrains.Annotations.CanBeNull]
    private ModdableClockDrivenWaypoints.VelocityWrapper _wanderNight;

    private void InitializeIfNull()
    {
      if ((Object) this.Flee != (Object) null)
      {
        if (this._fleeDay == null)
          this._fleeDay = new ModdableClockDrivenWaypoints.VelocityWrapper(this.Flee.dayParams.AvoidVelocity, "FleeVelocity.Day");
        if (this._fleeNight == null)
          this._fleeNight = new ModdableClockDrivenWaypoints.VelocityWrapper(this.Flee.nightParams.AvoidVelocity, "FleeVelocity.Night");
      }
      else if ((Object) this.FleeLowHealth != (Object) null && this._fleeBoth == null)
        this._fleeBoth = new ModdableClockDrivenWaypoints.VelocityWrapper(this.FleeLowHealth.newWapoints.AvoidVelocity, "FleeVelocity");
      if ((Object) this.Attack != (Object) null)
      {
        if (this._attackDay == null)
          this._attackDay = new ModdableClockDrivenWaypoints.VelocityWrapper(this.Attack.dayParams.ApproachVelocity, "AttackVelocity.Day");
        if (this._attackNight == null)
          this._attackNight = new ModdableClockDrivenWaypoints.VelocityWrapper(this.Attack.nightParams.ApproachVelocity, "AttackVelocity.Night");
      }
      if ((Object) this.React != (Object) null)
      {
        if (this._reactDay == null)
          this._reactDay = new ModdableClockDrivenWaypoints.VelocityWrapper(this.React.dayParams.ApproachVelocity, "ReactVelocity.Day");
        if (this._reactNight == null)
          this._reactNight = new ModdableClockDrivenWaypoints.VelocityWrapper(this.React.nightParams.ApproachVelocity, "ReactVelocity.Night");
      }
      if (!((Object) this.Wander != (Object) null))
        return;
      if (this._wanderDay == null)
        this._wanderDay = new ModdableClockDrivenWaypoints.VelocityWrapper(this.Wander.dayParams.ApproachVelocity, "WanderVelocity.Day");
      if (this._wanderNight != null)
        return;
      this._wanderNight = new ModdableClockDrivenWaypoints.VelocityWrapper(this.Wander.nightParams.ApproachVelocity, "WanderVelocity.Night");
    }

    public string ModHandlerName => "Movement";

    public void GetModDefaults(IList<ModEntry> defaultModEntries)
    {
      this.InitializeIfNull();
      this.GetModDefault(defaultModEntries, this._fleeDay);
      this.GetModDefault(defaultModEntries, this._fleeNight);
      this.GetModDefault(defaultModEntries, this._fleeBoth);
      this.GetModDefault(defaultModEntries, this._attackDay);
      this.GetModDefault(defaultModEntries, this._attackNight);
      this.GetModDefault(defaultModEntries, this._reactDay);
      this.GetModDefault(defaultModEntries, this._reactNight);
      this.GetModDefault(defaultModEntries, this._wanderDay);
      this.GetModDefault(defaultModEntries, this._wanderNight);
    }

    public void ApplyMod(string key, object value)
    {
      if (value == null || !(value is float num))
        return;
      this.InitializeIfNull();
      this.ApplyModIfKeyMatches(this._fleeDay, key, num);
      this.ApplyModIfKeyMatches(this._fleeNight, key, num);
      this.ApplyModIfKeyMatches(this._fleeBoth, key, num);
      this.ApplyModIfKeyMatches(this._attackDay, key, num);
      this.ApplyModIfKeyMatches(this._attackNight, key, num);
      this.ApplyModIfKeyMatches(this._reactDay, key, num);
      this.ApplyModIfKeyMatches(this._reactNight, key, num);
      this.ApplyModIfKeyMatches(this._wanderDay, key, num);
      this.ApplyModIfKeyMatches(this._wanderNight, key, num);
    }

    private void GetModDefault(
      IList<ModEntry> defaultModEntries,
      ModdableClockDrivenWaypoints.VelocityWrapper wrapper)
    {
      if (wrapper == null || !wrapper.IsModdable)
        return;
      defaultModEntries.Add(new ModEntry(wrapper.Name, (object) wrapper.Velocity));
    }

    private void ApplyModIfKeyMatches(
      ModdableClockDrivenWaypoints.VelocityWrapper wrapper,
      string key,
      float value)
    {
      if (wrapper == null || !wrapper.IsModdable || wrapper.Name != key)
        return;
      wrapper.Velocity = Mathf.Clamp(value, -100f, 100f);
    }

    private class VelocityWrapper
    {
      public readonly string Name;
      private readonly AnimationCurve _curve;
      private readonly bool _isModdable;

      public VelocityWrapper(AnimationCurve curve, string name)
      {
        this.Name = name;
        this._curve = curve;
        if (curve == null || curve.length <= 0)
          this._isModdable = false;
        else if (curve.length == 1)
        {
          this._isModdable = true;
        }
        else
        {
          this._isModdable = true;
          float num = curve[0].value;
          for (int index = 1; index < curve.length; ++index)
          {
            if ((double) Mathf.Abs(num - curve[index].value) > 0.01)
            {
              this._isModdable = false;
              break;
            }
          }
        }
      }

      public bool IsModdable => this._isModdable;

      public float Velocity
      {
        get => !this._isModdable ? 0.0f : this._curve[0].value;
        set
        {
          if (!this._isModdable)
            return;
          this._curve.keys = new Keyframe[1]
          {
            this._curve[0] with { value = value }
          };
        }
      }
    }
  }
}
