// Decompiled with JetBrains decompiler
// Type: GunAimCrosshair
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch;
using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Modules.Inventory.Holdables;
using CodeHatch.Weapons;
using UnityEngine;

#nullable disable
public class GunAimCrosshair : EntityBehaviour
{
  public Transform crosshair;
  public float Spread;

  public void LateUpdate() => this.UpdateCrosshair();

  public void UpdateCrosshair()
  {
    bool flag = this.ShouldDisplayCrosshair();
    this.crosshair.GetComponent<Renderer>().enabled = flag;
    if (!flag)
      return;
    Quaternion rotation = this.Entity.GetOrCreate<LookBridge>().Rotation;
    this.transform.rotation = this.GetMacroRotationShake() * rotation;
    float num = this.CrosshairSize();
    this.crosshair.localScale = new Vector3(num, num, num);
  }

  private Quaternion GetMacroRotationShake()
  {
    BipedHolder bipedHolder = this.Entity.Get<BipedHolder>();
    if ((Object) bipedHolder == (Object) null)
      return Quaternion.identity;
    Quaternion macroRotationShake = Quaternion.identity;
    int holdableCount = bipedHolder.HoldableCount;
    for (int index = 0; index < holdableCount; ++index)
    {
      BipedHoldable holdable = bipedHolder.TryGetHoldable(index);
      if (!((Object) holdable == (Object) null))
      {
        GunRecoilAnimation gunRecoilAnimation = holdable.Entity.TryGet<GunRecoilAnimation>();
        if (!((Object) gunRecoilAnimation == (Object) null))
          macroRotationShake = gunRecoilAnimation.MacroRotationalShake() * macroRotationShake;
      }
    }
    return macroRotationShake;
  }

  private float CrosshairSize()
  {
    BipedHolder bipedHolder = this.Entity.Get<BipedHolder>();
    if ((Object) bipedHolder == (Object) null)
      return 0.2f;
    bool flag = false;
    float num1 = 0.0f;
    int holdableCount = bipedHolder.HoldableCount;
    for (int index = 0; index < holdableCount; ++index)
    {
      BipedHoldable holdable = bipedHolder.TryGetHoldable(index);
      if (!((Object) holdable == (Object) null))
      {
        GunMovementSpread gunMovementSpread = holdable.Entity.Get<GunMovementSpread>();
        if (!((Object) gunMovementSpread == (Object) null))
        {
          flag = true;
          float num2 = (float) ((double) gunMovementSpread.CombinedSpread * 3.1415927410125732 / 180.0) * this.crosshair.transform.localPosition.z + this.crosshair.GetComponent<GUIScalingCircle>().referenceScale;
          if ((double) num1 < (double) num2)
            num1 = num2;
        }
      }
    }
    return flag ? num1 : 0.2f;
  }

  private bool ShouldDisplayCrosshair()
  {
    BipedHolder bipedHolder = this.Entity.Get<BipedHolder>();
    if ((Object) bipedHolder == (Object) null)
      return false;
    BipedHoldable bipedHoldable = (BipedHoldable) null;
    int holdableCount = bipedHolder.HoldableCount;
    for (int index = 0; index < holdableCount; ++index)
    {
      bipedHoldable = bipedHolder.TryGetHoldable(index);
      if ((Object) bipedHoldable != (Object) null)
        break;
    }
    if ((Object) bipedHoldable == (Object) null)
      return false;
    Entity entity = bipedHoldable.Entity;
    if ((Object) entity.TryGet<GunRecoilAnimation>() == (Object) null)
      return false;
    GunReceiver gunReceiver = entity.TryGet<GunReceiver>();
    if ((Object) gunReceiver == (Object) null || !gunReceiver.CanFire())
      return false;
    GunSightUp gunSightUp = entity.TryGet<GunSightUp>();
    return !((Object) gunSightUp != (Object) null) || !gunSightUp.IsSightedUp() || !gunSightUp.hideGunAimCrosshair;
  }
}
