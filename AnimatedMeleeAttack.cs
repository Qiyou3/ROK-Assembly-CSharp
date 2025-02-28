// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.AnimatedMeleeAttack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Damaging;
using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Core.Utility.Attributes;
using CodeHatch.Engine.Networking;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class AnimatedMeleeAttack : AttackBase, IDamager
  {
    public Animation _anim;
    public string[] attackAnimations;
    public float animationAttackTime;
    public float crossFadeTime = 0.5f;
    public Transform lookTransform;
    public bool useDot = true;
    [Tooltip("The type of damage done.")]
    [SerializeField]
    [FlagEnum(typeof (DamageType))]
    [Designer]
    private DamageType _damageTypes = DamageType.Melee;
    [Designer]
    public float damage = 20f;
    [Designer]
    public float forceMultiplier = 20f;
    [Designer]
    public float damageRange = 2f;
    private float stopAttackAt;
    private int rand;

    public DamageType DamageTypes
    {
      get => this._damageTypes;
      set => this._damageTypes = value;
    }

    protected float lastAttackTime { get; set; }

    protected bool isAttacking { get; set; }

    protected Location lastAttackLocation { get; set; }

    public Entity DamageSource
    {
      get => this.Entity;
      set => this.LogError<AnimatedMeleeAttack>("Cannot set damage source!");
    }

    public override void TriggerAttack(Location location)
    {
      this.rand = Random.Range(0, this.attackAnimations.Length);
      this.isAttacking = true;
      this.lastAttackLocation = location;
      this.lastAttackTime = Time.time;
      this.stopAttackAt = Time.time + this._anim[this.attackAnimations[this.rand]].clip.length;
    }

    public virtual void Update()
    {
      AnimationState animationState = this._anim[this.attackAnimations[this.rand]];
      animationState.layer = 2;
      animationState.blendMode = AnimationBlendMode.Blend;
      animationState.speed = 1f;
      if (this.isAttacking)
      {
        animationState.weight = Mathf.Lerp(this._anim[this.attackAnimations[this.rand]].weight, 1f, 1f);
        animationState.enabled = true;
        if ((double) this.stopAttackAt < (double) Time.time)
          this.isAttacking = false;
        if ((double) this.lastAttackTime + (double) this.animationAttackTime < (double) Time.time)
        {
          Entity entity = Player.Local.CurrentCharacter.Entity;
          if ((Object) entity == (Object) null || (double) Vector3.Distance(entity.MainTransform.position, this.Entity.MainTransform.position) > (double) this.damageRange)
            return;
          Vector3 lhs = entity.MainTransform.position - this.Entity.MainTransform.position;
          if ((double) Vector3.Dot(lhs, this.lookTransform.forward) > 0.0 || !this.useDot)
          {
            Collider collider = (Collider) (GameObjectAttribute<Collider>) entity.TryGet<MainCollider>();
            if ((Object) collider == (Object) null)
              return;
            List<AnimatedMeleeAttack.DamageCollider> damageRecievers = AnimatedMeleeAttack.GetDamageRecievers(collider);
            if (damageRecievers.Count > 0)
            {
              Rigidbody rigidbody = collider.gameObject.FirstComponentAncestor<Rigidbody>();
              if ((Object) rigidbody != (Object) null)
                rigidbody.AddForceAtPosition(lhs.normalized * this.forceMultiplier, collider.transform.position, ForceMode.VelocityChange);
              this.ApplyDamage(damageRecievers);
              this.isAttacking = false;
            }
          }
        }
      }
      if ((double) this.stopAttackAt - (double) animationState.length * 0.40000000596046448 >= (double) Time.time)
        return;
      animationState.weight = Mathf.Lerp(this._anim[this.attackAnimations[this.rand]].weight, 0.0f, HalfLife.GetRate(animationState.length * 0.4f));
      if ((double) animationState.weight >= 0.1)
        return;
      animationState.speed = 0.0f;
    }

    public void ApplyDamage(List<AnimatedMeleeAttack.DamageCollider> recievers)
    {
      for (int index = 0; index < recievers.Count; ++index)
      {
        IDamageReceiver receiver = recievers[index].Receiver;
        if (receiver != null)
        {
          Damage damage = new Damage((Object) this, this.DamageSource)
          {
            Amount = this.damage,
            DamageTypes = this._damageTypes
          };
          receiver.OnDamage(damage);
        }
      }
    }

    private static List<AnimatedMeleeAttack.DamageCollider> GetDamageRecievers(Collider collider)
    {
      List<AnimatedMeleeAttack.DamageCollider> damageRecievers = new List<AnimatedMeleeAttack.DamageCollider>();
      if ((Object) collider != (Object) null)
      {
        IDamageReceiver receiver = collider.gameObject.FirstImplementingAncestor<IDamageReceiver>();
        if (receiver != null)
        {
          bool flag = true;
          for (int index = 0; index < damageRecievers.Count; ++index)
          {
            if (damageRecievers[index].Receiver == receiver)
            {
              flag = false;
              break;
            }
          }
          if (flag)
            damageRecievers.Add(new AnimatedMeleeAttack.DamageCollider(receiver, collider));
        }
      }
      return damageRecievers;
    }

    public class DamageCollider
    {
      public DamageCollider(IDamageReceiver receiver, Collider collider)
      {
        this.Receiver = receiver;
        this.Collider = collider;
      }

      public IDamageReceiver Receiver { get; private set; }

      public Collider Collider { get; private set; }
    }
  }
}
