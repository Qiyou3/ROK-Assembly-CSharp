// Decompiled with JetBrains decompiler
// Type: GunShake
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Modules.Inventory.Holdables;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class GunShake : EntityBehaviour
{
  public AnimationCurve shotShakeAnimationX;
  public AnimationCurve shotShakeAnimationZ;
  private List<float> _shotTimes = new List<float>();
  private float _magnitude;
  private bool rumbling;
  public float followGunRecoilAmount = 0.5f;

  public Quaternion GetRotation()
  {
    BipedHolder bipedHolder = this.Entity.Get<BipedHolder>();
    if ((Object) bipedHolder == (Object) null)
      return Quaternion.identity;
    Quaternion rotation = Quaternion.identity;
    int holdableCount = bipedHolder.HoldableCount;
    for (int index = 0; index < holdableCount; ++index)
    {
      GunRecoilAnimation gunRecoilAnimation = bipedHolder.TryGetHoldable(index).Entity.TryGet<GunRecoilAnimation>();
      if (!((Object) gunRecoilAnimation == (Object) null))
        rotation = gunRecoilAnimation.MacroRotationalShake() * rotation;
    }
    return rotation;
  }

  public float rotateOffset()
  {
    this.LogError<GunShake>("TODO: Re-implement rotateOffset().");
    return 0.0f;
  }

  public Quaternion ProcessRotation(Quaternion rotation)
  {
    float x = this.shotShakeAnimationX.EvaluateTotal(this._shotTimes, Time.time) * this._magnitude;
    float z = this.shotShakeAnimationZ.EvaluateTotal(this._shotTimes, Time.time) * this._magnitude;
    rotation = Quaternion.Slerp(Quaternion.identity, this.GetRotation(), this.followGunRecoilAmount) * rotation * Quaternion.Euler(new Vector3(x, 0.0f, z));
    return rotation;
  }

  public void Shake(float magnitude)
  {
    this._magnitude = magnitude;
    this._shotTimes.Add(-Time.time);
    if (this._shotTimes.Count <= 10)
      return;
    this._shotTimes.RemoveAt(0);
  }

  public void Rumble(float magnitude)
  {
    this._magnitude = magnitude;
    this.rumbling = true;
  }
}
