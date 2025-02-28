// Decompiled with JetBrains decompiler
// Type: BaseLookBone
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch;
using CodeHatch.Common;
using CodeHatch.Common.Attributes;
using CodeHatch.Engine.Core.Cache;
using JetBrains.Annotations;
using UnityEngine;

#nullable disable
public abstract class BaseLookBone : EntityBehaviour
{
  protected const float MaxRotationRate = 0.1f;
  [CanBeNull]
  private LookBridge _lookBridge;
  public CurveOrConstant RotationTime;
  public CurveOrConstant RotationLimit;
  [NoEdit]
  public Quaternion RotationOffset = Quaternion.identity;
  [NoEdit]
  public Quaternion SourceRotation = Quaternion.identity;
  [NoEdit]
  public Quaternion TargetRotation = Quaternion.identity;
  [CanBeNull]
  public LookBone Parent;

  [CanBeNull]
  private LookBridge LookBridge
  {
    get
    {
      if ((Object) this._lookBridge == (Object) null)
      {
        Entity entity = this.NullCheck<Entity>(this.Entity, "Entity");
        if ((Object) entity == (Object) null)
          return (LookBridge) null;
        this._lookBridge = entity.GetOrCreate<LookBridge>();
      }
      return this._lookBridge;
    }
  }

  protected Vector3 Forward
  {
    get
    {
      LookBridge lookBridge = this.NullCheck<LookBridge>(this.LookBridge, "LookBridge");
      return (Object) lookBridge == (Object) null ? Vector3.forward : lookBridge.Forward;
    }
  }

  public void Awake()
  {
    this.RotationOffset = Quaternion.identity;
    this.SourceRotation = Quaternion.identity;
    this.TargetRotation = Quaternion.identity;
  }

  public void Start() => this.Awake();

  public void CaptureSourceRotation() => this.SourceRotation = this.transform.rotation;

  public void CaptureTargetRotation() => this.TargetRotation = this.transform.rotation;

  public void CaptureRotationOffset()
  {
    this.RotationOffset = this.transform.rotation * this.SourceRotation.GetInverse();
  }

  protected void LimitRotation(float interest)
  {
    if ((Object) this.Parent == (Object) null)
    {
      this.transform.rotation = Quaternion.RotateTowards(this.SourceRotation, this.transform.rotation, this.RotationLimit.Evaluate(interest));
    }
    else
    {
      Quaternion from = this.Parent.SourceRotation.GetInverse() * this.SourceRotation;
      this.transform.rotation = this.Parent.transform.rotation.GetInverse() * this.transform.rotation;
      this.transform.rotation = Quaternion.RotateTowards(from, this.transform.rotation, this.RotationLimit.Evaluate(interest));
      this.transform.rotation = this.Parent.transform.rotation * this.transform.rotation;
    }
  }

  public abstract void UpdateTargetRotationRecursively(float interest);

  public virtual void ApplySmoothingAndLimitsRecursively(float interest)
  {
    this.transform.rotation = Quaternion.Slerp(this.RotationOffset * this.SourceRotation, this.TargetRotation, Mathf.Min(0.1f, HalfLife.GetRate(this.RotationTime.Evaluate(interest))));
    this.LimitRotation(interest);
  }
}
