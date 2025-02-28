// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.BipedJumping
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Modding.Abstract;
using CodeHatch.Networking.Events;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class BipedJumping : EntityBehaviour, IModable
  {
    private const string _jumpVelocityName = "Velocity";
    private const string _earlyJumpForgivenessTimeName = "EarlyForgivenessTime";
    private const string _jumpCooldownTimeName = "CooldownTime";
    private const string _stabilityThresholdName = "StabilityThreshold";
    private MotorBridge _myMotorBridge;
    private BipedStability _myBipedStability;
    private BipedBody _myBipedBody;
    private BipedAnimator bipedAnimator;
    [Designer]
    public float jumpVelocity = 5f;
    [Designer]
    public float earlyJumpForgivenessTime = 0.25f;
    [Designer]
    public float jumpCooldownTime = 0.25f;
    public float stabilityThreshold = 0.1f;
    public AnimationClipEx animation;
    private float _lastJumpRequestedTime;
    private bool _jumpRequested;
    private float _lastJumpTime;

    public MotorBridge MyMotorBridge
    {
      get
      {
        if ((Object) this._myMotorBridge == (Object) null)
          this._myMotorBridge = this.Entity.GetOrCreate<MotorBridge>();
        return this._myMotorBridge;
      }
    }

    public BipedStability MyBipedStability
    {
      get
      {
        if ((Object) this._myBipedStability == (Object) null)
          this._myBipedStability = this.Entity.GetOrCreate<BipedStability>();
        return this._myBipedStability;
      }
    }

    public BipedBody MyBipedBody
    {
      get
      {
        if ((Object) this._myBipedBody == (Object) null)
          this._myBipedBody = this.Entity.Get<BipedBody>();
        return this._myBipedBody;
      }
    }

    public virtual BipedAnimator BipedAnimator
    {
      get
      {
        if ((Object) this.bipedAnimator == (Object) null)
          this.bipedAnimator = this.Entity.Get<BipedAnimator>();
        return this.bipedAnimator;
      }
    }

    private float TimeSinceLastJumpRequest => Time.time - this._lastJumpRequestedTime;

    private float TimeSinceLastJump => Time.time - this._lastJumpTime;

    public void Update()
    {
      if (this.MyMotorBridge.Jump)
      {
        this._lastJumpRequestedTime = Time.time;
        this._jumpRequested = true;
      }
      if (!this._jumpRequested)
        return;
      if ((double) this.TimeSinceLastJumpRequest > (double) this.earlyJumpForgivenessTime)
        this._jumpRequested = false;
      else if ((double) this.TimeSinceLastJump <= (double) this.jumpCooldownTime)
      {
        this._jumpRequested = false;
      }
      else
      {
        if (!this.MyMotorBridge.Grounded || (double) this.MyMotorBridge.Stability < (double) this.stabilityThreshold)
          return;
        this.Jump();
      }
    }

    private void Jump()
    {
      this.ApplyJumpForces();
      this._jumpRequested = false;
      this._lastJumpTime = Time.time;
      EventManager.CallEvent((BaseEvent) new EntityJumpEvent(this.Entity, true));
      EventManager.CallEvent((BaseEvent) new EntityJumpEvent(this.Entity, false));
    }

    private void ApplyJumpForces()
    {
      Vector3 normalized = Physics.gravity.normalized;
      this.MyBipedBody.Torso.velocity += -normalized * this.jumpVelocity - this.MyBipedBody.Torso.velocity.Parallel(normalized);
      this.MyBipedBody.Legs.velocity += -normalized * this.jumpVelocity - this.MyBipedBody.Legs.velocity.Parallel(normalized);
    }

    public string ModHandlerName => "Movement.Jumping";

    public void GetModDefaults(IList<ModEntry> defaultModList)
    {
      defaultModList.Add(new ModEntry("Velocity", (object) this.jumpVelocity));
      defaultModList.Add(new ModEntry("EarlyForgivenessTime", (object) this.earlyJumpForgivenessTime));
      defaultModList.Add(new ModEntry("CooldownTime", (object) this.jumpCooldownTime));
      defaultModList.Add(new ModEntry("StabilityThreshold", (object) this.stabilityThreshold));
    }

    public void ApplyMod(string key, object value)
    {
      if (value == null)
        return;
      string key1 = key;
      if (key1 == null)
        return;
      // ISSUE: reference to a compiler-generated field
      if (BipedJumping.\u003C\u003Ef__switch\u0024map28 == null)
      {
        // ISSUE: reference to a compiler-generated field
        BipedJumping.\u003C\u003Ef__switch\u0024map28 = new Dictionary<string, int>(4)
        {
          {
            "Velocity",
            0
          },
          {
            "EarlyForgivenessTime",
            1
          },
          {
            "CooldownTime",
            2
          },
          {
            "StabilityThreshold",
            3
          }
        };
      }
      int num;
      // ISSUE: reference to a compiler-generated field
      if (!BipedJumping.\u003C\u003Ef__switch\u0024map28.TryGetValue(key1, out num))
        return;
      switch (num)
      {
        case 0:
          this.jumpVelocity = Mathf.Clamp((float) value, 0.0f, 100f);
          break;
        case 1:
          this.earlyJumpForgivenessTime = Mathf.Max((float) value, 0.0f);
          break;
        case 2:
          this.jumpCooldownTime = Mathf.Max((float) value, 0.0f);
          break;
        case 3:
          this.stabilityThreshold = Mathf.Clamp01((float) value);
          break;
      }
    }
  }
}
