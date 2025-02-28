// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.MonsterModdableMotorLimits
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Modding.Abstract;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class MonsterModdableMotorLimits : MonoBehaviour, IModable
  {
    private const string _neutralVelocityName = "Stamina.NeutralVelocity";
    private const string _maxVelocityName = "MaximumVelocity";
    private const string _maxVelocitySprintDurationName = "Stamina.MaximumVelocitySprintDuration";
    private const string _timeForFullRecoveryAtRestName = "Stamina.TimeForFullRecoveryAtRest";
    private const string _slowdownDuration = "Stamina.SlowdownDuration";
    [CodeHatch.Engine.Core.Utility.Attributes.CanBeNull]
    [JetBrains.Annotations.CanBeNull]
    public MonsterMotorStamina MonsterMotorStaminaClient;
    [CodeHatch.Engine.Core.Utility.Attributes.CanBeNull]
    [JetBrains.Annotations.CanBeNull]
    public MonsterMotorStamina MonsterMotorStaminaDedi;
    [CodeHatch.Engine.Core.Utility.Attributes.CanBeNull]
    [JetBrains.Annotations.CanBeNull]
    public MonsterMotor MonsterMotorClient;
    [JetBrains.Annotations.CanBeNull]
    [CodeHatch.Engine.Core.Utility.Attributes.CanBeNull]
    public MonsterMotor MonsterMotorDedi;

    public string ModHandlerName => "Movement";

    public void GetModDefaults(IList<ModEntry> defaultModEntries)
    {
      if ((Object) this.MonsterMotorStaminaDedi != (Object) null)
      {
        defaultModEntries.Add(new ModEntry("Stamina.NeutralVelocity", (object) this.MonsterMotorStaminaDedi.NeutralVelocity));
        defaultModEntries.Add(new ModEntry("MaximumVelocity", (object) this.MonsterMotorStaminaDedi.MaxVelocity));
        defaultModEntries.Add(new ModEntry("Stamina.MaximumVelocitySprintDuration", (object) this.MonsterMotorStaminaDedi.MaxVelocitySprintDuration));
        defaultModEntries.Add(new ModEntry("Stamina.TimeForFullRecoveryAtRest", (object) this.MonsterMotorStaminaDedi.TimeForFullRecoveryAtRest));
        defaultModEntries.Add(new ModEntry("Stamina.SlowdownDuration", (object) this.MonsterMotorStaminaDedi.SlowdownDuration));
      }
      else
      {
        if (!((Object) this.MonsterMotorDedi != (Object) null))
          return;
        defaultModEntries.Add(new ModEntry("MaximumVelocity", (object) this.MonsterMotorDedi.MaximumVelocityBase));
      }
    }

    public void ApplyMod(string key, object value)
    {
      if (value == null || !(value is float num1))
        return;
      string key1 = key;
      if (key1 != null)
      {
        // ISSUE: reference to a compiler-generated field
        if (MonsterModdableMotorLimits.\u003C\u003Ef__switch\u0024map19 == null)
        {
          // ISSUE: reference to a compiler-generated field
          MonsterModdableMotorLimits.\u003C\u003Ef__switch\u0024map19 = new Dictionary<string, int>(5)
          {
            {
              "Stamina.NeutralVelocity",
              0
            },
            {
              "MaximumVelocity",
              1
            },
            {
              "Stamina.MaximumVelocitySprintDuration",
              2
            },
            {
              "Stamina.TimeForFullRecoveryAtRest",
              3
            },
            {
              "Stamina.SlowdownDuration",
              4
            }
          };
        }
        int num2;
        // ISSUE: reference to a compiler-generated field
        if (MonsterModdableMotorLimits.\u003C\u003Ef__switch\u0024map19.TryGetValue(key1, out num2))
        {
          switch (num2)
          {
            case 0:
              if ((Object) this.MonsterMotorStaminaDedi != (Object) null)
                this.MonsterMotorStaminaDedi.NeutralVelocity = num1;
              if ((Object) this.MonsterMotorStaminaClient != (Object) null)
              {
                this.MonsterMotorStaminaClient.NeutralVelocity = num1;
                break;
              }
              break;
            case 1:
              float num3 = Mathf.Clamp(num1, 0.0f, 100f);
              if ((Object) this.MonsterMotorStaminaDedi != (Object) null)
                this.MonsterMotorStaminaDedi.MaxVelocity = num3;
              if ((Object) this.MonsterMotorStaminaClient != (Object) null)
                this.MonsterMotorStaminaClient.MaxVelocity = num3;
              if ((Object) this.MonsterMotorDedi != (Object) null)
                this.MonsterMotorDedi.MaximumVelocityBase = num3;
              if ((Object) this.MonsterMotorClient != (Object) null)
              {
                this.MonsterMotorClient.MaximumVelocityBase = num3;
                break;
              }
              break;
            case 2:
              if ((Object) this.MonsterMotorStaminaDedi != (Object) null)
                this.MonsterMotorStaminaDedi.MaxVelocitySprintDuration = num1;
              if ((Object) this.MonsterMotorStaminaClient != (Object) null)
              {
                this.MonsterMotorStaminaClient.MaxVelocitySprintDuration = num1;
                break;
              }
              break;
            case 3:
              if ((Object) this.MonsterMotorStaminaDedi != (Object) null)
                this.MonsterMotorStaminaDedi.TimeForFullRecoveryAtRest = num1;
              if ((Object) this.MonsterMotorStaminaClient != (Object) null)
              {
                this.MonsterMotorStaminaClient.TimeForFullRecoveryAtRest = num1;
                break;
              }
              break;
            case 4:
              if ((Object) this.MonsterMotorStaminaDedi != (Object) null)
                this.MonsterMotorStaminaDedi.SlowdownDuration = num1;
              if ((Object) this.MonsterMotorStaminaClient != (Object) null)
              {
                this.MonsterMotorStaminaClient.SlowdownDuration = num1;
                break;
              }
              break;
          }
        }
      }
      if ((Object) this.MonsterMotorStaminaDedi != (Object) null)
        this.MonsterMotorStaminaDedi.OnValidate();
      if (!((Object) this.MonsterMotorStaminaClient != (Object) null))
        return;
      this.MonsterMotorStaminaClient.OnValidate();
    }
  }
}
