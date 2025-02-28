// Decompiled with JetBrains decompiler
// Type: CodeHatch.AI.MonsterAttackAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Damaging;
using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Core.Utility.Attributes;
using CodeHatch.Engine.Modding.Abstract;
using CodeHatch.Engine.Networking;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace CodeHatch.AI
{
  public class MonsterAttackAnimation : AttackBase, IDamager, IModable
  {
    public AnimatorObject AttackTrigger = (AnimatorObject) "Attack";
    public AnimatorObject AttackVariationInt = (AnimatorObject) "Attack Variation";
    public AnimatorObject DamageBool = (AnimatorObject) nameof (Damage);
    [FlagEnum(typeof (DamageType))]
    [Designer]
    [SerializeField]
    [Tooltip("The type of damage done.")]
    private DamageType _damageTypes = DamageType.Melee;
    [Designer]
    public float Damage = 20f;
    [Designer]
    public float ForceMultiplier = 20f;
    [Designer]
    public float DamageRange = 2f;
    [Tooltip("The number of attack variations provided by the Animator.")]
    public int AttackVariations = 1;
    public Transform LookTransform;
    public bool FrontalAttacksOnly = true;
    [Tooltip("These are used to determine the closest bone that was attacked, does distance checks from these points to the major body bones")]
    public Transform[] attackPoints;
    [JetBrains.Annotations.CanBeNull]
    private Animator _animator;
    private bool _attackCausedDamage;

    public DamageType DamageTypes
    {
      get => this._damageTypes;
      set => this._damageTypes = value;
    }

    public Entity DamageSource
    {
      get => this.Entity;
      set => this.LogError<MonsterAttackAnimation>("Cannot set damage source!");
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

    public override void TriggerAttack(Location location)
    {
      Animator animator = this.NullCheck<Animator>(this.Animator, "NullCheck");
      if ((Object) animator == (Object) null)
        return;
      if (this.AttackVariations > 1)
        animator.SetInteger((int) this.AttackVariationInt, Random.Range(0, this.AttackVariations));
      animator.SetTrigger((int) this.AttackTrigger);
      this._attackCausedDamage = false;
    }

    public virtual void Update()
    {
      if (this._attackCausedDamage)
        return;
      Animator animator = this.NullCheck<Animator>(this.Animator, "NullCheck");
      if ((Object) animator == (Object) null || !animator.GetBool((int) this.DamageBool))
        return;
      Entity entity1 = Player.Local.CurrentCharacter.Entity;
      if ((Object) entity1 == (Object) null)
        return;
      Entity entity2 = this.NullCheck<Entity>(this.Entity, "Entity");
      if ((Object) entity2 == (Object) null)
        return;
      Vector3 position1 = entity1.Position;
      Vector3 position2 = entity2.Position;
      if ((double) Vector3.Distance(position1, position2) > (double) this.DamageRange)
        return;
      Vector3 lhs = position1 - position2;
      bool flag = (double) Vector3.Dot(lhs, this.LookTransform.forward) > 0.0;
      if (this.FrontalAttacksOnly && !flag)
        return;
      Collider collider = (Collider) (GameObjectAttribute<Collider>) entity1.TryGet<MainCollider>();
      if ((Object) collider == (Object) null)
        return;
      IDamageReceiver componentInParent = collider.GetComponentInParent<IDamageReceiver>();
      if (componentInParent == null)
        return;
      Rigidbody attachedRigidbody = collider.attachedRigidbody;
      if ((Object) attachedRigidbody != (Object) null)
        attachedRigidbody.AddForceAtPosition(lhs.normalized * this.ForceMultiplier, collider.transform.position, ForceMode.VelocityChange);
      HumanBodyBones humanBodyBones = HumanBodyBones.Chest;
      if (this.attackPoints != null && this.attackPoints.Length > 0)
      {
        CharacterDefinition characterDefinition = entity1.GetOrCreate<CharacterDefinition>();
        float num = float.MaxValue;
        Vector3 position3 = characterDefinition.GetTransform(HumanBodyBones.Chest).position;
        Vector3 position4 = characterDefinition.GetTransform(HumanBodyBones.LeftFoot).position;
        Vector3 position5 = characterDefinition.GetTransform(HumanBodyBones.RightFoot).position;
        Vector3 position6 = characterDefinition.GetTransform(HumanBodyBones.LeftLowerArm).position;
        Vector3 position7 = characterDefinition.GetTransform(HumanBodyBones.RightLowerArm).position;
        Vector3 position8 = characterDefinition.GetTransform(HumanBodyBones.LeftLowerLeg).position;
        Vector3 position9 = characterDefinition.GetTransform(HumanBodyBones.RightLowerLeg).position;
        Vector3 position10 = characterDefinition.GetTransform(HumanBodyBones.LeftHand).position;
        Vector3 position11 = characterDefinition.GetTransform(HumanBodyBones.RightHand).position;
        for (int index = 0; index < this.attackPoints.Length; ++index)
        {
          float sqrMagnitude1 = (this.attackPoints[index].position - position3).sqrMagnitude;
          if ((double) sqrMagnitude1 < (double) num)
          {
            num = sqrMagnitude1;
            humanBodyBones = HumanBodyBones.Chest;
          }
          float sqrMagnitude2 = (this.attackPoints[index].position - position4).sqrMagnitude;
          if ((double) sqrMagnitude2 < (double) num)
          {
            num = sqrMagnitude2;
            humanBodyBones = HumanBodyBones.LeftFoot;
          }
          float sqrMagnitude3 = (this.attackPoints[index].position - position5).sqrMagnitude;
          if ((double) sqrMagnitude3 < (double) num)
          {
            num = sqrMagnitude3;
            humanBodyBones = HumanBodyBones.RightFoot;
          }
          float sqrMagnitude4 = (this.attackPoints[index].position - position6).sqrMagnitude;
          if ((double) sqrMagnitude4 < (double) num)
          {
            num = sqrMagnitude4;
            humanBodyBones = HumanBodyBones.LeftLowerArm;
          }
          float sqrMagnitude5 = (this.attackPoints[index].position - position7).sqrMagnitude;
          if ((double) sqrMagnitude5 < (double) num)
          {
            num = sqrMagnitude5;
            humanBodyBones = HumanBodyBones.RightLowerArm;
          }
          float sqrMagnitude6 = (this.attackPoints[index].position - position8).sqrMagnitude;
          if ((double) sqrMagnitude6 < (double) num)
          {
            num = sqrMagnitude6;
            humanBodyBones = HumanBodyBones.LeftLowerLeg;
          }
          float sqrMagnitude7 = (this.attackPoints[index].position - position9).sqrMagnitude;
          if ((double) sqrMagnitude7 < (double) num)
          {
            num = sqrMagnitude7;
            humanBodyBones = HumanBodyBones.RightLowerLeg;
          }
          float sqrMagnitude8 = (this.attackPoints[index].position - position10).sqrMagnitude;
          if ((double) sqrMagnitude8 < (double) num)
          {
            num = sqrMagnitude8;
            humanBodyBones = HumanBodyBones.LeftHand;
          }
          float sqrMagnitude9 = (this.attackPoints[index].position - position11).sqrMagnitude;
          if ((double) sqrMagnitude9 < (double) num)
          {
            num = sqrMagnitude9;
            humanBodyBones = HumanBodyBones.RightHand;
          }
        }
      }
      CodeHatch.Damaging.Damage damage = new CodeHatch.Damaging.Damage((Object) this, this.DamageSource)
      {
        Amount = this.Damage,
        DamageTypes = this._damageTypes,
        HitBoxBone = humanBodyBones
      };
      componentInParent.OnDamage(damage);
      this._attackCausedDamage = true;
    }

    public string ModHandlerName => "Attack";

    public void GetModDefaults(IList<ModEntry> defaultModEntries)
    {
      SetAttackFromBundle component = this.GetComponent<SetAttackFromBundle>();
      defaultModEntries.Add(new ModEntry("AttackRange", (object) this.DamageRange));
      defaultModEntries.Add(new ModEntry("DamageAmount", (object) (float) (!((Object) component == (Object) null) ? (double) component.bundle.AttackDamage : (double) this.Damage)));
      defaultModEntries.Add(new ModEntry("DamageForce", (object) (float) (!((Object) component == (Object) null) ? (double) component.bundle.AttackForce : (double) this.ForceMultiplier)));
    }

    public void ApplyMod(string key, object value)
    {
      if (value == null)
        return;
      SetAttackFromBundle component = this.GetComponent<SetAttackFromBundle>();
      string key1 = key;
      if (key1 == null)
        return;
      // ISSUE: reference to a compiler-generated field
      if (MonsterAttackAnimation.\u003C\u003Ef__switch\u0024map18 == null)
      {
        // ISSUE: reference to a compiler-generated field
        MonsterAttackAnimation.\u003C\u003Ef__switch\u0024map18 = new Dictionary<string, int>(3)
        {
          {
            "AttackRange",
            0
          },
          {
            "DamageAmount",
            1
          },
          {
            "DamageForce",
            2
          }
        };
      }
      int num;
      // ISSUE: reference to a compiler-generated field
      if (!MonsterAttackAnimation.\u003C\u003Ef__switch\u0024map18.TryGetValue(key1, out num))
        return;
      switch (num)
      {
        case 0:
          this.DamageRange = (float) value;
          break;
        case 1:
          this.Damage = (float) value;
          if (!((Object) component != (Object) null))
            break;
          component.bundle.AttackDamage = this.Damage;
          break;
        case 2:
          this.ForceMultiplier = (float) value;
          if (!((Object) component != (Object) null))
            break;
          component.bundle.AttackForce = this.ForceMultiplier;
          break;
      }
    }
  }
}
