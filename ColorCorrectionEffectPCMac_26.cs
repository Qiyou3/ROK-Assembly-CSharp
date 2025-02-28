// Decompiled with JetBrains decompiler
// Type: ColorCorrectionEffectPCMac_26
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Color Correction PCMac")]
public class ColorCorrectionEffectPCMac_26 : ImageEffectBase
{
  public Texture textureRampPC;
  public Texture textureRampMac;
  public float rampOffsetR;
  public float rampOffsetG;
  public float rampOffsetB;

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    bool flag = false;
    switch (Application.platform)
    {
      case RuntimePlatform.OSXEditor:
      case RuntimePlatform.OSXPlayer:
      case RuntimePlatform.OSXWebPlayer:
      case RuntimePlatform.OSXDashboardPlayer:
        flag = true;
        break;
    }
    if (flag)
      this.material.SetTexture("_RampTex", this.textureRampMac);
    else
      this.material.SetTexture("_RampTex", this.textureRampPC);
    this.material.SetVector("_RampOffset", new Vector4(this.rampOffsetR, this.rampOffsetG, this.rampOffsetB, 0.0f));
    Graphics.Blit((Texture) source, destination, this.material);
  }
}
