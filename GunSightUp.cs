// Decompiled with JetBrains decompiler
// Type: GunSightUp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Core.Input;
using CodeHatch.Engine.Modules.Inventory.Holdables;
using UnityEngine;

#nullable disable
public class GunSightUp : EntityBehaviour, IHolderListener
{
  public KeyButton sightUpButton = new KeyButton("Walk", KeyCode.Mouse1);
  public AnimationCurve sightUpPositionAnimation;
  public AnimationCurve sightUpVelocityAnimation;
  public bool hideGunAimCrosshair = true;
  private float _velocityState;
  private float _positionState;

  public Entity HeldBy { get; set; }

  public float SightUpAmount { get; private set; }

  public bool IsSightedUp()
  {
    return (double) this.SightUpAmount * (double) PlayerCameraSwitchUtil.FirstPersonRatio > 0.5;
  }

  public void OnEnable()
  {
    this._velocityState = 0.0f;
    this._positionState = 0.0f;
  }

  public void Update()
  {
    bool flag = this.sightUpButton.Get();
    if ((double) this._positionState >= (double) this.sightUpPositionAnimation.GetMaxTime() || (double) this._positionState <= (double) this.sightUpPositionAnimation.GetMinTime())
      this._velocityState = 0.0f;
    this._velocityState = Mathf.Clamp(this._velocityState + (!flag ? -1f : 1f) * Time.deltaTime, this.sightUpVelocityAnimation.GetMinTime(), this.sightUpVelocityAnimation.GetMaxTime());
    this._positionState = Mathf.Clamp(this._positionState + this.sightUpVelocityAnimation.Evaluate(this._velocityState) * Time.deltaTime, this.sightUpPositionAnimation.GetMinTime(), this.sightUpPositionAnimation.GetMaxTime());
    this.SightUpAmount = this.sightUpPositionAnimation.Evaluate(this._positionState);
    float zoom1 = 40f;
    if (this.Entity.Has<GunSight>())
      zoom1 = this.Entity.Get<GunSight>().zoomLevel;
    if (!((Object) this.HeldBy != (Object) null) || !this.HeldBy.Has<Zoom>())
      return;
    foreach (Zoom zoom2 in this.HeldBy.GetArray<Zoom>())
      zoom2.SetNewZoom(zoom1, this.SightUpAmount);
  }
}
