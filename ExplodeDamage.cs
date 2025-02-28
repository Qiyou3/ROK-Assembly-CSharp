// Decompiled with JetBrains decompiler
// Type: ExplodeDamage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Blocks;
using CodeHatch.Common;
using CodeHatch.Damaging;
using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Core.Utility.Attributes;
using CodeHatch.Engine.Networking;
using CodeHatch.Networking.Events;
using CodeHatch.Networking.Events.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

#nullable disable
public class ExplodeDamage : EntityBehaviour, IDamager
{
  [Tooltip("The physical layers that will be tested for collision with this explosion.")]
  [Designer]
  public LayerMask explosionLayer;
  [Tooltip("The radius of this explosion in metres.")]
  [Designer]
  public float radius = 10f;
  [Tooltip("The velocity that this explosion will travel in metres per second.")]
  [Designer]
  public float velocity = 25f;
  [Tooltip("Whether or not this explosion will attempt to damage objects.")]
  [Designer]
  public bool CanApplyDamage = true;
  [Designer]
  [SerializeField]
  [Tooltip("The type of damage done.")]
  [FlagEnum(typeof (DamageType))]
  protected DamageType _damageTypes = DamageType.Explosion;
  [Tooltip("The effective percentage of damage dealt from the origin to the radius.")]
  [Designer]
  public AnimationCurve DamageCurve = new AnimationCurve(new Keyframe[3]
  {
    new Keyframe(0.0f, 1f),
    new Keyframe(0.05f, 1f),
    new Keyframe(1f, 0.0f)
  });
  [Designer]
  [Tooltip("The base damage dealt by this explosion.")]
  public float damage = 20f;
  [Tooltip("Whether or not this explosion will attempt to push objects.")]
  [Designer]
  public bool CanApplyForce = true;
  [Tooltip("The base force of this explosion.")]
  [Designer]
  public float force = 50000f;
  [Tooltip("A static upward force of this explosion.")]
  [Designer]
  public float upwardsModifier = 5f;
  protected RaycastHit _hitCache;
  protected static int _structureLayers;

  public DamageType DamageTypes
  {
    get => this._damageTypes;
    set => this._damageTypes = value;
  }

  public Entity DamageSource { get; set; }

  public GameObject CachedGameObject { get; protected set; }

  public Vector3 CachedPosition { get; protected set; }

  public Bounds CachedBounds { get; protected set; }

  public Bounds CachedBlocks { get; protected set; }

  public void Start()
  {
    if (!Player.IsLocalServer)
      return;
    if (!this.CanApplyForce && !this.CanApplyDamage)
    {
      UnityEngine.Debug.Log((object) string.Format("Explosion applies no effects, revise {0} component on {1}.", (object) this.name, (object) this.gameObject.name));
    }
    else
    {
      if ((UnityEngine.Object) this.DamageSource == (UnityEngine.Object) null)
        this.DamageSource = this.Entity.Owner.Entity;
      this.CachedGameObject = this.gameObject;
      this.CachedPosition = this.transform.position;
      this.CachedBounds = this.GetTilesetBounds();
      if (ExplodeDamage._structureLayers == 0)
      {
        ExplodeDamage._structureLayers |= 1 << LayerMask.NameToLayer("Blocks");
        ExplodeDamage._structureLayers |= 1 << LayerMask.NameToLayer("Terrain");
        ExplodeDamage._structureLayers |= 1 << LayerMask.NameToLayer("Terrain Objects");
        ExplodeDamage._structureLayers |= 1 << LayerMask.NameToLayer("Environment");
      }
      this.StartCoroutine(this.ApplyEffectsOverTime());
    }
  }

  [DebuggerHidden]
  protected IEnumerator ApplyEffectsOverTime()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new ExplodeDamage.\u003CApplyEffectsOverTime\u003Ec__Iterator1D3()
    {
      \u003C\u003Ef__this = this
    };
  }

  protected Bounds GetTilesetBounds()
  {
    float x = BlockManager.AllCubeGrids[0].transform.localScale.x;
    float num = Mathf.Ceil(this.radius / x);
    return new Bounds(new Vector3(Mathf.Floor(this.CachedPosition.x / x) + 0.5f, Mathf.Floor(this.CachedPosition.y / x) + 0.5f, Mathf.Floor(this.CachedPosition.z / x) + 0.5f) * x, Vector3.one * (float) ((double) num * (double) x * 2.0));
  }

  protected List<ExplodeDamage.CompoundCollider> GetCompoundColliders()
  {
    ExplodeDamage.ColliderInfo[] colliderInfo = this.GetColliderInfo();
    ExplodeDamage.CompoundCollider compoundCollider1 = new ExplodeDamage.CompoundCollider((GameObject) null, (ExplodeDamage.ReceiverInfo) null);
    List<ExplodeDamage.CompoundCollider> compoundColliders = new List<ExplodeDamage.CompoundCollider>();
    for (int index1 = 0; index1 < colliderInfo.Length; ++index1)
    {
      if (!((UnityEngine.Object) colliderInfo[index1].Collider == (UnityEngine.Object) null))
      {
        GameObject gameObject = colliderInfo[index1].Collider.gameObject;
        if ((UnityEngine.Object) gameObject == (UnityEngine.Object) compoundCollider1.GameObject)
        {
          compoundCollider1.Colliders.Add(colliderInfo[index1]);
        }
        else
        {
          bool flag = true;
          for (int index2 = compoundColliders.Count - 1; index2 >= 0; --index2)
          {
            if ((UnityEngine.Object) compoundColliders[index2].GameObject == (UnityEngine.Object) gameObject)
            {
              compoundColliders[index2].Colliders.Add(colliderInfo[index1]);
              flag = false;
              compoundCollider1 = compoundColliders[index2];
              break;
            }
          }
          if (flag)
          {
            IDamageReceiver receiver = gameObject.FirstImplementingAncestor<IDamageReceiver>();
            ExplodeDamage.CompoundCollider compoundCollider2 = (ExplodeDamage.CompoundCollider) null;
            for (int index3 = compoundColliders.Count - 1; index3 >= 0; --index3)
            {
              if (compoundColliders[index3].ReceiverInfo.Receiver == receiver)
              {
                compoundCollider2 = new ExplodeDamage.CompoundCollider(gameObject, compoundColliders[index3].ReceiverInfo);
                ++compoundColliders[index3].ReceiverInfo.ReferenceCount;
                break;
              }
            }
            if (compoundCollider2 == null)
              compoundCollider2 = new ExplodeDamage.CompoundCollider(gameObject, new ExplodeDamage.ReceiverInfo(receiver));
            compoundCollider2.Colliders.Add(colliderInfo[index1]);
            compoundColliders.Add(compoundCollider2);
            compoundCollider1 = compoundCollider2;
          }
        }
      }
    }
    return compoundColliders;
  }

  protected ExplodeDamage.ColliderInfo[] GetColliderInfo()
  {
    Collider[] colliderArray = Physics.OverlapSphere(this.CachedPosition, this.radius, (int) this.explosionLayer);
    ExplodeDamage.ColliderInfo[] array = new ExplodeDamage.ColliderInfo[colliderArray.Length];
    for (int index = 0; index < colliderArray.Length; ++index)
      array[index] = new ExplodeDamage.ColliderInfo(colliderArray[index], this.CachedPosition);
    array.Quicksort<ExplodeDamage.ColliderInfo>();
    return array;
  }

  protected bool CanHit(Entity entity)
  {
    if ((UnityEngine.Object) this.Entity == (UnityEngine.Object) null)
      return false;
    RaycastHit hitInfo;
    if (!Physics.Raycast(new Ray(this.CachedPosition, entity.Position - this.CachedPosition), out hitInfo, Vector3.Distance(this.CachedPosition, entity.Position), ExplodeDamage._structureLayers))
      return true;
    this._hitCache = hitInfo;
    return false;
  }

  protected bool CanHit(ExplodeDamage.CompoundCollider compoundCollider)
  {
    if (compoundCollider == null || (UnityEngine.Object) compoundCollider.GameObject == (UnityEngine.Object) null || compoundCollider.ReceiverInfo.Receiver == null || compoundCollider.ReceiverInfo.WasDamaged)
      return false;
    if (compoundCollider.GameObject.layer == BlockManager.CubeLayer)
      return true;
    for (int index = 0; index < compoundCollider.Colliders.Count; ++index)
    {
      if (!((UnityEngine.Object) compoundCollider.Colliders[index].Collider == (UnityEngine.Object) null))
      {
        if (compoundCollider.Colliders[index].Collider.bounds.Contains(this.CachedPosition))
        {
          this._hitCache = new RaycastHit()
          {
            distance = 0.0f,
            point = this.CachedPosition,
            normal = this.CachedPosition - compoundCollider.Colliders[index].Collider.bounds.center
          };
          return true;
        }
        Ray ray = new Ray(this.CachedPosition, compoundCollider.Colliders[index].Collider.bounds.center - this.CachedPosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, this.radius, (int) this.explosionLayer) && compoundCollider.ContainsCollider(hitInfo.collider))
        {
          this._hitCache = hitInfo;
          return true;
        }
        ray = new Ray(this.CachedPosition, compoundCollider.Colliders[index].Closest - this.CachedPosition);
        if (Physics.Raycast(ray, out hitInfo, this.radius, (int) this.explosionLayer) && compoundCollider.ContainsCollider(hitInfo.collider))
        {
          this._hitCache = hitInfo;
          return true;
        }
      }
    }
    return false;
  }

  protected void ApplyForce(Rigidbody rigidbody)
  {
    if ((UnityEngine.Object) rigidbody == (UnityEngine.Object) null || rigidbody.isKinematic || (double) rigidbody.mass < 9.9999997473787516E-05)
      return;
    rigidbody.AddExplosionForce(this.force, this.CachedPosition, this.radius, this.upwardsModifier);
  }

  protected void ApplyDamage(Entity entity)
  {
    if ((UnityEngine.Object) this.Entity == (UnityEngine.Object) null)
      return;
    float multiplier = this.MultiplierAtPosition(entity.Position);
    if ((double) multiplier <= 0.0)
      return;
    EventManager.CallEvent((BaseEvent) new EntityDamageEvent(entity, this.CreateDamage(multiplier)));
  }

  protected void ApplyDamage(ExplodeDamage.CompoundCollider compoundCollider)
  {
    if (compoundCollider == null || compoundCollider.ReceiverInfo.Receiver == null || compoundCollider.ReceiverInfo.WasDamaged)
      return;
    RLECollider receiver = compoundCollider.ReceiverInfo.Receiver as RLECollider;
    if ((UnityEngine.Object) receiver != (UnityEngine.Object) null)
    {
      this.CachedBlocks.Encapsulate(receiver.AttachedCollider.bounds);
      compoundCollider.ReceiverInfo.WasDamaged = true;
    }
    else if (compoundCollider.GameObject.layer == BlockManager.CubeLayer && !BlockManager.IsTaggedNonOctPrefab(compoundCollider.GameObject))
    {
      for (int index = 0; index < compoundCollider.Colliders.Count; ++index)
        this.CachedBlocks.Encapsulate(compoundCollider.Colliders[index].Collider.bounds);
      compoundCollider.ReceiverInfo.WasDamaged = true;
    }
    else
    {
      float multiplier = this.MultiplierAtDistance(this._hitCache.distance);
      if ((double) multiplier <= 0.0)
        return;
      compoundCollider.ReceiverInfo.Receiver.OnDamage(this.CreateDamage(multiplier));
      compoundCollider.ReceiverInfo.WasDamaged = true;
    }
  }

  protected void DamageBlocks(Bounds bounds)
  {
    RootCubeGrid allCubeGrid = BlockManager.AllCubeGrids[0];
    float x1 = allCubeGrid.transform.localScale.x;
    int num1 = Mathf.RoundToInt(bounds.size.x / x1);
    int num2 = Mathf.RoundToInt(bounds.size.y / x1);
    int num3 = Mathf.RoundToInt(bounds.size.z / x1);
    List<TilesetColliderCube> tilesetColliderCubeList = new List<TilesetColliderCube>();
    List<Damage> damageList = new List<Damage>();
    float x2 = (float) ((double) bounds.center.x - (double) num1 / 2.0 * (double) x1 + (num1 % 2 != 1 ? (double) x1 / 2.0 : 0.0));
    float num4 = (float) ((double) bounds.center.y - (double) num2 / 2.0 * (double) x1 + (num2 % 2 != 1 ? (double) x1 / 2.0 : 0.0));
    float num5 = (float) ((double) bounds.center.z - (double) num3 / 2.0 * (double) x1 + (num3 % 2 != 1 ? (double) x1 / 2.0 : 0.0));
    int num6 = 0;
    while (num6 < num1)
    {
      float y = num4;
      int num7 = 0;
      while (num7 < num2)
      {
        float z = num5;
        int num8 = 0;
        while (num8 < num3)
        {
          Vector3 vector3 = new Vector3(x2, y, z);
          TilesetColliderCube centralPrefabAtLocal = allCubeGrid.GetCentralPrefabAtLocal(allCubeGrid.WorldToLocalCoordinate(vector3));
          if (!((UnityEngine.Object) centralPrefabAtLocal == (UnityEngine.Object) null))
          {
            float multiplier = this.MultiplierAtPosition(vector3);
            if ((double) multiplier > 0.0)
            {
              damageList.Add(this.CreateDamage(multiplier, vector3));
              tilesetColliderCubeList.Add(centralPrefabAtLocal);
            }
          }
          ++num8;
          z += x1;
        }
        ++num7;
        y += x1;
      }
      ++num6;
      x2 += x1;
    }
    for (int index = 0; index < tilesetColliderCubeList.Count; ++index)
      tilesetColliderCubeList[index].OnDamage(damageList[index], CodeHatch.Damaging.DamageSource.Damager);
  }

  protected float MultiplierAtPosition(Vector3 position)
  {
    return this.MultiplierAtDistance(Vector3.Distance(this.CachedPosition, position));
  }

  protected float MultiplierAtDistance(float distance)
  {
    return this.DamageCurve.Evaluate(Mathf.Clamp01(distance / this.radius));
  }

  protected Damage CreateDamage(float multiplier)
  {
    return new Damage((UnityEngine.Object) this.CachedGameObject, this.DamageSource)
    {
      Amount = this.damage * multiplier,
      DamageTypes = this.DamageTypes,
      Damager = (UnityEngine.Object) this
    };
  }

  protected Damage CreateDamage(float multiplier, Vector3 position)
  {
    return new Damage((UnityEngine.Object) this.CachedGameObject, this.DamageSource)
    {
      Amount = this.damage * multiplier,
      DamageTypes = this.DamageTypes,
      Damager = (UnityEngine.Object) this,
      point = position
    };
  }

  public class ColliderInfo : IComparable<ExplodeDamage.ColliderInfo>
  {
    public ColliderInfo(Collider collider, Vector3 origin)
    {
      this.Collider = collider;
      this.Closest = this.Collider.ClosestPointOnBounds(origin);
      this.Distance = !this.Collider.bounds.Contains(origin) ? Vector3.Distance(origin, this.Closest) : 0.0f;
    }

    public Collider Collider { get; private set; }

    public Vector3 Closest { get; private set; }

    public float Distance { get; private set; }

    public int CompareTo(ExplodeDamage.ColliderInfo info)
    {
      if ((double) this.Distance > (double) info.Distance)
        return 1;
      return (double) this.Distance < (double) info.Distance ? -1 : 0;
    }
  }

  public class ReceiverInfo
  {
    public int ReferenceCount;
    public bool WasDamaged;

    public ReceiverInfo(IDamageReceiver receiver)
    {
      this.Receiver = receiver;
      this.ReferenceCount = this.Receiver == null ? 0 : 1;
    }

    public IDamageReceiver Receiver { get; private set; }
  }

  public class CompoundCollider
  {
    public CompoundCollider(GameObject gameObject, ExplodeDamage.ReceiverInfo receiverInfo)
    {
      this.GameObject = gameObject;
      this.ReceiverInfo = receiverInfo;
      this.Colliders = new List<ExplodeDamage.ColliderInfo>();
    }

    public GameObject GameObject { get; private set; }

    public ExplodeDamage.ReceiverInfo ReceiverInfo { get; private set; }

    public List<ExplodeDamage.ColliderInfo> Colliders { get; private set; }

    public bool IsStructure(Collider collider)
    {
      for (int index = 0; index < this.Colliders.Count; ++index)
      {
        if ((UnityEngine.Object) this.Colliders[index].Collider == (UnityEngine.Object) collider)
          return true;
      }
      return false;
    }

    public bool ContainsCollider(Collider collider)
    {
      for (int index = 0; index < this.Colliders.Count; ++index)
      {
        if ((UnityEngine.Object) this.Colliders[index].Collider == (UnityEngine.Object) collider)
          return true;
      }
      return false;
    }
  }
}
