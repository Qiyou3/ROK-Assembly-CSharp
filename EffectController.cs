// Decompiled with JetBrains decompiler
// Type: EffectController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class EffectController : MonoBehaviour
{
  public Generate2DReflection generate2dReflection;
  public GlowEffectThreshold_26 glowEffect;
  public MotionBlurEdge motionBlur;
  public ColorCorrectionEffect colorCorrection;

  public void Update()
  {
    if (QualitySettings.GetQualityLevel() < 3)
    {
      if ((bool) (Object) this.generate2dReflection)
        this.generate2dReflection.enabled = false;
      if ((bool) (Object) this.glowEffect)
        this.glowEffect.enabled = false;
      if ((bool) (Object) this.motionBlur)
        this.motionBlur.enabled = false;
      if (!(bool) (Object) this.colorCorrection)
        return;
      this.colorCorrection.enabled = false;
    }
    else
    {
      if ((bool) (Object) this.generate2dReflection)
        this.generate2dReflection.enabled = true;
      if ((bool) (Object) this.glowEffect)
        this.glowEffect.enabled = true;
      if ((bool) (Object) this.motionBlur)
        this.motionBlur.enabled = true;
      if (!(bool) (Object) this.colorCorrection)
        return;
      this.colorCorrection.enabled = true;
    }
  }
}
