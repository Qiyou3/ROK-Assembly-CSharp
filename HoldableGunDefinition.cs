// Decompiled with JetBrains decompiler
// Type: HoldableGunDefinition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch;
using CodeHatch.Common;
using CodeHatch.Engine.Core.Utility.Attributes;
using CodeHatch.Engine.Modules.Inventory.Holdable;
using SmartAssembly.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class HoldableGunDefinition : EntityDefinitionBase
{
  private bool _validated;
  [CanBeNull]
  [SerializeField]
  [DoNotObfuscate]
  private Transform buttAnchor;
  [CanBeNull]
  [DoNotObfuscate]
  [SerializeField]
  private Transform sightsPivot;
  [CanBeNull]
  [SerializeField]
  [DoNotObfuscate]
  private Transform emitPoint;
  private Transform _rotationPoint;
  private Vector3 _offset;
  public BipedLocomotionAnimation LocomotionAnimation;

  public Transform ButtAnchor
  {
    get
    {
      this.EnsureValid(this._validated);
      return (UnityEngine.Object) this.buttAnchor != (UnityEngine.Object) null ? this.buttAnchor : this.Entity.MainTransform;
    }
  }

  public bool ButtAnchorDefined
  {
    get
    {
      this.EnsureValid(this._validated);
      return (UnityEngine.Object) this.buttAnchor != (UnityEngine.Object) null;
    }
  }

  public Transform SightsPivot
  {
    get
    {
      this.EnsureValid(this._validated);
      return (UnityEngine.Object) this.sightsPivot != (UnityEngine.Object) null ? this.sightsPivot : this.Entity.MainTransform;
    }
  }

  public bool SightsPivotDefined
  {
    get
    {
      this.EnsureValid(this._validated);
      return (UnityEngine.Object) this.sightsPivot != (UnityEngine.Object) null;
    }
  }

  public Transform EmitPoint
  {
    get
    {
      this.EnsureValid(this._validated);
      return (UnityEngine.Object) this.emitPoint != (UnityEngine.Object) null ? this.emitPoint : this.Entity.MainTransform;
    }
  }

  public bool EmitPointDefined
  {
    get
    {
      this.EnsureValid(this._validated);
      return (UnityEngine.Object) this.emitPoint != (UnityEngine.Object) null;
    }
  }

  public Transform RotationPoint
  {
    get
    {
      this.EnsureValid((bool) (UnityEngine.Object) this._rotationPoint);
      return this._rotationPoint;
    }
  }

  public Vector3 RotationPointLocalOffset
  {
    get
    {
      this.EnsureValid((bool) (UnityEngine.Object) this._rotationPoint);
      return this._offset;
    }
  }

  protected override void Validate()
  {
    if ((UnityEngine.Object) this.buttAnchor == (UnityEngine.Object) null)
      this.buttAnchor = this.TryGetPivot(AnchorType.Butt);
    if ((UnityEngine.Object) this.sightsPivot == (UnityEngine.Object) null)
      this.sightsPivot = this.TryGetPivot(AnchorType.Sights);
    if ((UnityEngine.Object) this._rotationPoint == (UnityEngine.Object) null)
    {
      this._rotationPoint = new GameObject("Rotation Point").transform;
      if ((UnityEngine.Object) this.buttAnchor != (UnityEngine.Object) null && (UnityEngine.Object) this.sightsPivot != (UnityEngine.Object) null)
      {
        this._rotationPoint.parent = this.buttAnchor.parent;
        Vector3 position = this.buttAnchor.position;
        Vector3 direction = (this.sightsPivot.position - position).Perpendicular(this.Entity.MainTransform.forward);
        this._rotationPoint.position = position + direction;
        this._offset = this.Entity.MainTransform.InverseTransformDirection(direction);
        this._rotationPoint.rotation = this.sightsPivot.rotation;
      }
      else
      {
        this._rotationPoint.parent = this.Entity.MainTransform;
        this._rotationPoint.localPosition = Vector3.zero;
        this._rotationPoint.localRotation = Quaternion.identity;
        this._offset = Vector3.zero;
      }
    }
    this._validated = true;
  }

  private Transform TryGetPivot(AnchorType type)
  {
    ItemHoldableIkGoalPivot holdableIkGoalPivot = ((IEnumerable<ItemHoldableIkGoalPivot>) this.Entity.TryGetArray<ItemHoldableIkGoalPivot>()).FirstOrDefault<ItemHoldableIkGoalPivot>((Func<ItemHoldableIkGoalPivot, bool>) (p => p.pivotType == type));
    return (UnityEngine.Object) holdableIkGoalPivot != (UnityEngine.Object) null ? holdableIkGoalPivot.transform : (Transform) null;
  }
}
