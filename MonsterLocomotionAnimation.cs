// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.MonsterLocomotionAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Engine.Core.Cache;
using SmartAssembly.Attributes;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class MonsterLocomotionAnimation : EntityBehaviour
  {
    public AnimatorObject VelocityFloat = (AnimatorObject) "Velocity";
    public AnimatorObject IdleBool = (AnimatorObject) "Idle";
    [Range(0.0f, 10f)]
    public float VelocitySmoothing;
    [Range(0.0f, 1f)]
    public float MovingVelocityThreshold = 0.1f;
    [Range(0.0f, 5f)]
    public float IdleTimeout = 0.5f;
    private float _timer;
    [DoNotObfuscate]
    [CodeHatch.Engine.Core.Utility.Attributes.CanBeNull]
    [SerializeField]
    private Transform _axisProvider;
    [JetBrains.Annotations.CanBeNull]
    private Rigidbody _mainRigidbody;
    [JetBrains.Annotations.CanBeNull]
    private Animator _animator;

    private Transform AxisProvider
    {
      get
      {
        if (!((Object) this._axisProvider == (Object) null))
          return this._axisProvider;
        Rigidbody rigidbody = this.NullCheck<Rigidbody>(this.MainRigidbody, "MainRigidbody");
        if ((Object) rigidbody != (Object) null)
        {
          this._axisProvider = rigidbody.transform;
          return this._axisProvider;
        }
        Entity entity = this.NullCheck<Entity>(this.Entity, "Entity");
        if ((Object) entity != (Object) null)
        {
          this._axisProvider = entity.MainTransform;
          return this._axisProvider;
        }
        this._axisProvider = this.transform;
        return this._axisProvider;
      }
    }

    [JetBrains.Annotations.CanBeNull]
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

    [JetBrains.Annotations.CanBeNull]
    private Animator Animator
    {
      get
      {
        if ((Object) this._animator == (Object) null)
        {
          Entity entity = this.NullCheck<Entity>(this.Entity, "Entity");
          if ((Object) entity == (Object) null)
            return (Animator) null;
          this._animator = entity.Get<Animator>();
        }
        return this._animator;
      }
    }

    public void Update()
    {
      Animator animator = this.NullCheck<Animator>(this.Animator, "Animator");
      if ((Object) animator == (Object) null)
        return;
      Rigidbody rigidbody = this.NullCheck<Rigidbody>(this.MainRigidbody, "MainRigidbody");
      if ((Object) rigidbody == (Object) null)
        return;
      Vector3 velocity = rigidbody.velocity;
      float magnitude = velocity.magnitude;
      Transform transform = this.NullCheck<Transform>(this.AxisProvider, "AxisProvider");
      bool flag1 = (Object) transform == (Object) null || (double) Vector3.Dot(transform.forward, velocity) >= 0.0;
      bool flag2;
      if ((double) magnitude < (double) this.MovingVelocityThreshold)
      {
        flag2 = TimeUtil.ExceededTime(this._timer);
      }
      else
      {
        flag2 = false;
        this._timer = TimeUtil.ResetTimer(this.IdleTimeout);
      }
      animator.SetBool((int) this.IdleBool, flag2);
      animator.SetFloat((int) this.VelocityFloat, !flag1 ? -magnitude : magnitude, this.VelocitySmoothing, Time.deltaTime);
    }
  }
}
