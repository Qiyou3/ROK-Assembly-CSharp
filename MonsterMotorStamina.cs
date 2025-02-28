// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.MonsterMotorStamina
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Engine.Core.Cache;
using JetBrains.Annotations;
using SmartAssembly.Attributes;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class MonsterMotorStamina : EntityBehaviour
  {
    private const float _epsilon = 0.01f;
    [Range(0.01f, 10f)]
    [DoNotObfuscate]
    [SerializeField]
    [UsedImplicitly]
    public float NeutralVelocity = 3.5f;
    [Range(0.01f, 30f)]
    [UsedImplicitly]
    [SerializeField]
    [DoNotObfuscate]
    public float MaxVelocity = 10f;
    [Range(0.01f, 30f)]
    [DoNotObfuscate]
    [SerializeField]
    [UsedImplicitly]
    public float MaxVelocitySprintDuration = 5f;
    [DoNotObfuscate]
    [SerializeField]
    [Range(0.01f, 60f)]
    [UsedImplicitly]
    public float TimeForFullRecoveryAtRest = 10f;
    [DoNotObfuscate]
    [Range(0.0f, 10f)]
    [UsedImplicitly]
    [SerializeField]
    public float SlowdownDuration = 2f;
    private float _stamina = 1f;
    [CanBeNull]
    private MonsterMotor _motor;
    [CanBeNull]
    private Rigidbody _mainRigidbody;

    [CanBeNull]
    protected MonsterMotor Motor
    {
      get
      {
        if ((Object) this._motor == (Object) null)
        {
          Entity entity = this.NullCheck<Entity>(this.Entity, "Entity");
          if ((Object) entity == (Object) null)
            return (MonsterMotor) null;
          this._motor = entity.Get<MonsterMotor>();
        }
        return this._motor;
      }
    }

    [CanBeNull]
    protected Rigidbody MainRigidbody
    {
      get
      {
        if ((Object) this._mainRigidbody == (Object) null)
        {
          Entity entity = this.NullCheck<Entity>(this.Entity, "Entity");
          if ((Object) entity == (Object) null)
            return (Rigidbody) null;
          this._mainRigidbody = (Rigidbody) (GameObjectAttribute<Rigidbody>) entity.Get<CodeHatch.Engine.Core.Cache.MainRigidbody>();
        }
        return this._mainRigidbody;
      }
    }

    public void OnValidate()
    {
      if ((double) this.NeutralVelocity < 0.0099999997764825821)
        this.NeutralVelocity = 0.01f;
      if ((double) this.MaxVelocity < (double) this.NeutralVelocity + 0.0099999997764825821)
        this.MaxVelocity = this.NeutralVelocity + 0.01f;
      if ((double) this.MaxVelocitySprintDuration < 0.0099999997764825821)
        this.MaxVelocitySprintDuration = 0.01f;
      if ((double) this.TimeForFullRecoveryAtRest < 0.0099999997764825821)
        this.TimeForFullRecoveryAtRest = 0.01f;
      if ((double) this.SlowdownDuration > (double) this.MaxVelocitySprintDuration)
        this.SlowdownDuration = this.MaxVelocitySprintDuration;
      if ((double) this.SlowdownDuration >= 0.0)
        return;
      this.SlowdownDuration = 0.0f;
    }

    public void Update()
    {
      Entity entity = this.NullCheck<Entity>(this.Entity, "Entity");
      if ((Object) entity == (Object) null || !entity.IsLocallyControlled)
        return;
      Rigidbody rigidbody = this.NullCheck<Rigidbody>(this.MainRigidbody, "MainRigidbody");
      if ((Object) rigidbody == (Object) null)
        return;
      MonsterMotor monsterMotor = this.NullCheck<MonsterMotor>(this.Motor, "Motor");
      if ((Object) monsterMotor == (Object) null)
        return;
      float magnitude = rigidbody.velocity.magnitude;
      if ((double) magnitude >= (double) this.NeutralVelocity)
      {
        float fl = (float) (((double) magnitude - (double) this.NeutralVelocity) / ((double) this.MaxVelocity - (double) this.NeutralVelocity)) / this.MaxVelocitySprintDuration;
        if (fl.IsReal())
        {
          this._stamina -= fl * Time.deltaTime;
          this._stamina = Mathf.Clamp01(this._stamina);
        }
      }
      else
      {
        float fl = (this.NeutralVelocity - magnitude) / this.NeutralVelocity / this.TimeForFullRecoveryAtRest;
        if (fl.IsReal())
        {
          this._stamina += fl * Time.deltaTime;
          this._stamina = Mathf.Clamp01(this._stamina);
        }
      }
      float num = this._stamina / Mathf.Max(Time.deltaTime, this.SlowdownDuration);
      monsterMotor.MaximumVelocityBase = Mathf.Clamp((this.MaxVelocity - this.NeutralVelocity) * this.MaxVelocitySprintDuration * num + this.NeutralVelocity, 0.0f, this.MaxVelocity);
    }
  }
}
