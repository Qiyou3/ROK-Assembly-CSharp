// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.BipedMotor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Common.Attributes;
using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Core.Utility.Attributes;
using CodeHatch.Engine.Modding.Abstract;
using CodeHatch.Engine.Modules.Inventory.Holdables;
using CodeHatch.Melee;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class BipedMotor : EntityBehaviour, IModable
  {
    [Designer]
    public float velocityLimit = 4f;
    [Designer]
    [RadialCurve]
    public AnimationCurve directionalVelocityLimit;
    [Separator("Acceleration")]
    [Designer]
    public float accelerationMultiplier = 20f;
    [Designer]
    public float accelerationLimit = 10f;
    [Separator("Lift & Ledge Climbing")]
    public float anticipation = 0.2f;
    public float offsetLimit = 0.5f;
    public float liftThreshold = 0.1f;
    private Vector3 _up;
    private Vector3 _desiredGroundVelocity;
    private Vector3 _desiredLiftVelocity;
    private Vector3 _desiredGroundAcceleration;
    private Vector3 _desiredLiftAcceleration;
    private MotorBridge _myMotorBridge;
    private LookBridge _myLookBridge;
    private ArmorManager _myArmorManager;
    private BipedBody _body;
    private BipedMotorReference _bipedMotorReference;
    private Vector3 _lastValidGroundNomral = Vector3.up;
    private bool _isGroundedOnValidGroundNormal;
    private CombatKicking _kicking;
    private bool _shouldLift;

    public Vector3 DesiredVelocity => this._desiredGroundVelocity;

    private HoldableShield TryGetShield()
    {
      BipedHolder bipedHolder = this.Entity.TryGet<BipedHolder>();
      if ((Object) bipedHolder == (Object) null)
        return (HoldableShield) null;
      for (int index = 0; index < bipedHolder.HoldableCount; ++index)
      {
        BipedHoldable holdable = bipedHolder.TryGetHoldable(index);
        if ((Object) holdable == (Object) null)
        {
          this.LogError<BipedMotor>("Holder.TryGetHoldable({0}) == null", (object) index);
        }
        else
        {
          Entity entity = holdable.Entity;
          if ((Object) entity == (Object) null)
          {
            this.LogError<BipedMotor>("Holder.TryGetHoldable({0}).Entity == null", (object) index);
          }
          else
          {
            HoldableShield shield = entity.TryGet<HoldableShield>();
            if ((Object) shield != (Object) null)
              return shield;
          }
        }
      }
      return (HoldableShield) null;
    }

    private CombatKicking Kicking
    {
      get
      {
        if ((Object) this._kicking == (Object) null)
          this._kicking = this.Entity.TryGet<CombatKicking>();
        return this._kicking;
      }
    }

    public void Start()
    {
      this._myArmorManager = this.Entity.TryGet<ArmorManager>();
      this._myMotorBridge = this.Entity.GetOrCreate<MotorBridge>();
      this._myLookBridge = this.Entity.GetOrCreate<LookBridge>();
      this._body = this.Entity.Get<BipedBody>();
      this._bipedMotorReference = this.Entity.TryGet<BipedMotorReference>();
    }

    public void FixedUpdate()
    {
      this._up = -Physics.gravity.normalized;
      this.UpdateGroundStuff();
      if (this.Entity.IsLocallyOwned)
        this.Local();
      else
        this.Remote();
    }

    private void Local()
    {
      this._desiredGroundVelocity = this._myMotorBridge.VelocityGround;
      this._desiredGroundVelocity -= this._up * Vector3.Dot(this._desiredGroundVelocity, this._up);
      float velocityLimit = this.velocityLimit;
      Vector3 forward = this._myLookBridge.Forward;
      float time = Vector3.Angle(this._desiredGroundVelocity, forward - this._up * Vector3.Dot(forward, this._up));
      float num1 = velocityLimit * Mathf.Clamp01(this.directionalVelocityLimit.Evaluate(time));
      if ((bool) (Object) this._myArmorManager)
        num1 *= this._myArmorManager.SpeedMultiplier;
      HoldableShield shield = this.TryGetShield();
      if ((bool) (Object) shield)
        num1 *= shield.SpeedMultiplier;
      CombatKicking kicking = this.Kicking;
      if ((Object) kicking != (Object) null)
        num1 = Mathf.Min(num1, Mathf.Lerp(num1, kicking.KickMoveSpeedLimit, kicking.KickMoveSpeedLimitWeight));
      this._desiredGroundVelocity = !this._myMotorBridge.UseMaximumVelocity ? this._desiredGroundVelocity.LimitMagnitude(num1) : this._desiredGroundVelocity.normalized * num1;
      if (this._isGroundedOnValidGroundNormal)
        this._desiredGroundVelocity = Vector3UtilEx.RayIntersectPlane(this._desiredGroundVelocity, this._up, Vector3.zero, this._lastValidGroundNomral);
      Vector3 velocity = this._body.Velocity;
      this._shouldLift = false;
      if ((Object) this._bipedMotorReference != (Object) null)
      {
        Vector3 lhs = (this._desiredGroundVelocity + velocity).normalized * Mathf.Max(this._desiredGroundVelocity.magnitude, velocity.magnitude) * this.anticipation;
        Vector3 offset = lhs - this._up * Vector3.Dot(lhs, this._up);
        float anticipation = this.anticipation;
        float magnitude = offset.magnitude;
        if ((double) magnitude > (double) this.offsetLimit)
        {
          float num2 = this.offsetLimit / magnitude;
          anticipation *= num2;
          offset *= num2;
        }
        this._bipedMotorReference.Move(ref offset);
        float num3 = Vector3.Dot(offset, this._up) - 0.08f - this.liftThreshold;
        if ((double) num3 > 0.0)
        {
          this._desiredLiftVelocity = this._up * num3 / anticipation;
          this._shouldLift = true;
        }
        else
          this._desiredLiftVelocity = Vector3.zero;
      }
      else
        this._desiredLiftVelocity = Vector3.zero;
      this._desiredGroundAcceleration = (this._desiredGroundVelocity - velocity) * this.accelerationMultiplier;
      this._desiredGroundAcceleration = !this._isGroundedOnValidGroundNormal ? this._desiredGroundAcceleration.Perpendicular(this._up) : Vector3UtilEx.RayIntersectPlane(this._desiredGroundAcceleration, this._up, Vector3.zero, this._lastValidGroundNomral);
      if (this._isGroundedOnValidGroundNormal)
        this._desiredGroundAcceleration -= Physics.gravity.Perpendicular(this._lastValidGroundNomral);
      if (!this._shouldLift)
      {
        this._desiredLiftAcceleration = Vector3.zero;
      }
      else
      {
        this._desiredLiftAcceleration = (this._desiredLiftVelocity - velocity) * this.accelerationMultiplier - Physics.gravity;
        float num4 = Vector3.Dot(this._desiredLiftAcceleration, this._up);
        this._desiredLiftAcceleration = (double) num4 > 0.0 ? this._up * num4 : Vector3.zero;
      }
      float a = Vector3.Dot(this._desiredGroundAcceleration, this._up);
      float b = Vector3.Dot(this._desiredLiftAcceleration, this._up);
      this._body.AddForce((this._desiredGroundAcceleration - this._up * a + this._up * Mathf.Max(0.0f, Mathf.Max(a, b))).LimitMagnitude(this.accelerationLimit) * this._myMotorBridge.Strength * this._myMotorBridge.Stability, ForceMode.Acceleration);
    }

    private void Remote()
    {
      this._desiredGroundVelocity = this._myMotorBridge.VelocityGround;
      this._desiredGroundVelocity -= this._up * Vector3.Dot(this._desiredGroundVelocity, this._up);
      if (this._isGroundedOnValidGroundNormal)
        this._desiredGroundVelocity = Vector3UtilEx.RayIntersectPlane(this._desiredGroundVelocity, this._up, Vector3.zero, this._lastValidGroundNomral);
      Vector3 velocity = this._body.Velocity;
      this._body.Velocity = this._desiredGroundVelocity with
      {
        y = velocity.y
      };
    }

    private void UpdateGroundStuff()
    {
      Vector3 normalized = this._myMotorBridge.LastGroundNormalTouched.normalized;
      if ((double) Vector3.Dot(this._up, normalized) < 0.0099999997764825821)
      {
        this._isGroundedOnValidGroundNormal = false;
      }
      else
      {
        this._lastValidGroundNomral = normalized;
        this._isGroundedOnValidGroundNormal = this._myMotorBridge.Grounded;
      }
    }

    public string ModHandlerName => "Movement";

    public void GetModDefaults(IList<ModEntry> defaultModList)
    {
      defaultModList.Add(new ModEntry("RunVelocity", (object) this.velocityLimit));
      defaultModList.Add(new ModEntry("AccelerationLimit", (object) this.accelerationLimit));
      defaultModList.Add(new ModEntry("AccelerationMultiplier", (object) this.accelerationMultiplier));
    }

    public void ApplyMod(string key, object value)
    {
      if (value == null)
        return;
      string key1 = key;
      if (key1 == null)
        return;
      // ISSUE: reference to a compiler-generated field
      if (BipedMotor.\u003C\u003Ef__switch\u0024map2A == null)
      {
        // ISSUE: reference to a compiler-generated field
        BipedMotor.\u003C\u003Ef__switch\u0024map2A = new Dictionary<string, int>(3)
        {
          {
            "RunVelocity",
            0
          },
          {
            "AccelerationMultiplier",
            1
          },
          {
            "AccelerationLimit",
            2
          }
        };
      }
      int num;
      // ISSUE: reference to a compiler-generated field
      if (!BipedMotor.\u003C\u003Ef__switch\u0024map2A.TryGetValue(key1, out num))
        return;
      switch (num)
      {
        case 0:
          this.velocityLimit = Mathf.Clamp((float) value, 0.0f, 100f);
          break;
        case 1:
          this.accelerationMultiplier = Mathf.Clamp((float) value, 0.0f, 50f);
          break;
        case 2:
          this.accelerationLimit = Mathf.Max((float) value, 0.0f);
          break;
      }
    }
  }
}
