// Decompiled with JetBrains decompiler
// Type: ColorCorrectionEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ColorCorrectionEffect : ImageEffectBase
{
  public Texture textureRamp;
  public float gamma = 1f;
  public float colorize;
  public float intensity = 1f;

  public void Awake()
  {
  }

  public void ApplyConfiguration()
  {
  }

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    this.material.SetTexture("_RampTex", this.textureRamp);
    this.material.SetFloat("_Gamma", this.gamma);
    this.material.SetFloat("_Colorize", this.colorize);
    this.material.SetFloat("_Intensity", this.intensity);
    Graphics.Blit((Texture) source, destination, this.material);
  }
}
