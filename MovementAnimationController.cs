// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.MovementAnimationController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Engine.Core.Cache;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class MovementAnimationController : EntityBehaviour
  {
    public AnimationClip IdleAnimation;
    public AnimationClip MoveAnimation;
    public AnimationClip RunAnimation;
    public AnimationCurve AnimationSpeedAtDot = AnimationCurve.Linear(0.0f, 0.0f, 1f, 1f);
    public float RunAtSpeed = 5f;
    public float RunSpeedMultiplier;
    public float MoveSpeedMultiplier;
    public float MoveSpeedForIdle = 1f;
    public float MoveBlendSmoothingTime = 0.5f;
    public float IdleBlendSmoothingTime = 1f;
    public Rigidbody MainRigidbody;
    public Transform RootBodyDirection;
    public float DisableIdleBelowGripOf = 0.05f;
    private MonsterMotor _monsterMotor;
    private MotorBridge _motorBridge;
    private Animation _animation;
    private bool _started;
    protected bool allowMove = true;
    protected float dotSpeed;
    protected float moveSpeed;
    protected AnimationState runClip;
    protected AnimationState walkClip;
    protected AnimationState idleClip;

    protected MonsterMotor MonsterMotor
    {
      get
      {
        if ((Object) this._monsterMotor == (Object) null)
          this._monsterMotor = this.Entity.Get<MonsterMotor>();
        return this._monsterMotor;
      }
    }

    protected MotorBridge MotorBridge
    {
      get
      {
        if ((Object) this._motorBridge == (Object) null)
          this._motorBridge = this.Entity.GetOrCreate<MotorBridge>();
        return this._motorBridge;
      }
    }

    protected Animation Animation
    {
      get
      {
        if ((Object) this._animation == (Object) null)
          this._animation = this.Entity.Get<Animation>();
        return this._animation;
      }
    }

    public virtual void Start()
    {
      this._started = true;
      if ((Object) this.MainRigidbody == (Object) null)
        this.MainRigidbody = (Rigidbody) (GameObjectAttribute<Rigidbody>) this.Entity.Get<CodeHatch.Engine.Core.Cache.MainRigidbody>();
      if (!((Object) this.RootBodyDirection == (Object) null))
        return;
      this.RootBodyDirection = (Transform) (GameObjectAttribute<Transform>) this.Entity.Get<MainTransform>();
    }

    public virtual void FixedUpdate()
    {
      if (!(bool) (Object) this.Animation)
        return;
      this.dotSpeed = Vector3.Dot(this.MainRigidbody.velocity.normalized, this.RootBodyDirection.forward);
      this.moveSpeed = this.AnimationSpeedAtDot.Evaluate(Mathf.Abs(this.dotSpeed)) * this.MainRigidbody.velocity.magnitude;
      if ((double) this.dotSpeed < 0.0)
        this.moveSpeed *= -1f;
      if ((Object) this.RunAnimation != (Object) null && (TrackedReference) this.runClip == (TrackedReference) null)
        this.runClip = this.Animation[this.RunAnimation.name];
      if ((Object) this.IdleAnimation != (Object) null && (TrackedReference) this.idleClip == (TrackedReference) null)
        this.idleClip = this.Animation[this.IdleAnimation.name];
      if ((Object) this.MoveAnimation != (Object) null && (TrackedReference) this.walkClip == (TrackedReference) null)
        this.walkClip = this.Animation[this.MoveAnimation.name];
      if ((TrackedReference) this.runClip != (TrackedReference) null)
      {
        if ((double) this.moveSpeed > (double) this.RunAtSpeed && (Object) this.RunAnimation != (Object) null)
        {
          this.runClip.speed = this.moveSpeed * this.RunSpeedMultiplier;
          this.runClip.layer = 0;
          this.runClip.weight = 1f;
          this.runClip.enabled = true;
          if (!((TrackedReference) this.idleClip != (TrackedReference) null))
            return;
          this.idleClip.weight = 0.0f;
          return;
        }
        if ((Object) this.RunAnimation != (Object) null)
          this.runClip.weight = 0.0f;
      }
      if ((TrackedReference) this.walkClip != (TrackedReference) null)
      {
        this.walkClip.speed = this.moveSpeed * this.MoveSpeedMultiplier;
        this.walkClip.layer = 0;
        this.walkClip.weight = Mathf.Lerp(this.walkClip.weight, this.MotorBridge.Strength - this.Animation[this.IdleAnimation.name].weight, HalfLife.GetRate(this.MoveBlendSmoothingTime));
        this.walkClip.enabled = this.allowMove;
      }
      if (!((TrackedReference) this.idleClip != (TrackedReference) null))
        return;
      this.idleClip.layer = 1;
      this.idleClip.blendMode = AnimationBlendMode.Blend;
      this.idleClip.weight = Mathf.Min(this.MotorBridge.Strength, Mathf.Lerp(this.idleClip.weight, Mathf.Abs((float) (1.0 / (1.0 + (double) this.moveSpeed / (double) this.MoveSpeedForIdle))), HalfLife.GetRate(this.IdleBlendSmoothingTime)));
      this.idleClip.enabled = true;
    }

    public void OnEnable()
    {
      if (!this._started)
        return;
      Animation animation = this.Entity.TryGet<Animation>();
      if (!(bool) (Object) animation)
        return;
      animation.enabled = true;
    }

    public virtual void OnDisable()
    {
      if (!this._started)
        return;
      Animation animation = this.Entity.TryGet<Animation>();
      if (!(bool) (Object) animation)
        return;
      animation.enabled = false;
    }
  }
}
