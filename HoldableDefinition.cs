// Decompiled with JetBrains decompiler
// Type: HoldableDefinition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Utility.Attributes;
using CodeHatch.Engine.Modules.Inventory.Holdable;
using SmartAssembly.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class HoldableDefinition : EntityDefinitionBase
{
  private bool _validated;
  [DoNotObfuscate]
  [CanBeNull]
  [SerializeField]
  private Transform leftHandAnchor;
  [DoNotObfuscate]
  [CanBeNull]
  [SerializeField]
  private Transform rightHandAnchor;

  public Transform LeftHandAnchor
  {
    get
    {
      this.EnsureValid(this._validated);
      return (UnityEngine.Object) this.leftHandAnchor != (UnityEngine.Object) null ? this.leftHandAnchor : this.Entity.MainTransform;
    }
  }

  public bool LeftHandAnchorDefined
  {
    get
    {
      this.EnsureValid(this._validated);
      return (UnityEngine.Object) this.leftHandAnchor != (UnityEngine.Object) null;
    }
  }

  public Transform RightHandAnchor
  {
    get
    {
      this.EnsureValid(this._validated);
      return (UnityEngine.Object) this.rightHandAnchor != (UnityEngine.Object) null ? this.rightHandAnchor : this.Entity.MainTransform;
    }
  }

  public bool RightHandAnchorDefined
  {
    get
    {
      this.EnsureValid(this._validated);
      return (UnityEngine.Object) this.rightHandAnchor != (UnityEngine.Object) null;
    }
  }

  protected override void Validate()
  {
    if ((UnityEngine.Object) this.leftHandAnchor == (UnityEngine.Object) null)
      this.leftHandAnchor = this.TryGetPivot(AnchorType.LeftHand);
    if ((UnityEngine.Object) this.rightHandAnchor == (UnityEngine.Object) null)
      this.rightHandAnchor = this.TryGetPivot(AnchorType.RightHand);
    this._validated = true;
  }

  private Transform TryGetPivot(AnchorType type)
  {
    ItemHoldableIkGoalPivot holdableIkGoalPivot = ((IEnumerable<ItemHoldableIkGoalPivot>) this.Entity.TryGetArray<ItemHoldableIkGoalPivot>()).FirstOrDefault<ItemHoldableIkGoalPivot>((Func<ItemHoldableIkGoalPivot, bool>) (p => p.pivotType == type));
    return (UnityEngine.Object) holdableIkGoalPivot != (UnityEngine.Object) null ? holdableIkGoalPivot.transform : (Transform) null;
  }

  public Transform GetAnchor(CodeHatch.Side hand)
  {
    switch (hand)
    {
      case CodeHatch.Side.Left:
        return this.LeftHandAnchor;
      case CodeHatch.Side.Right:
        return this.RightHandAnchor;
      default:
        throw new ArgumentOutOfRangeException(nameof (hand));
    }
  }
}
