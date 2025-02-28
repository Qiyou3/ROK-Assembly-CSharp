// Decompiled with JetBrains decompiler
// Type: ContrastEnhance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Contrast Enhance (Unsharp Mask)")]
[RequireComponent(typeof (Camera))]
public class ContrastEnhance : PostEffectsBase
{
  public float intensity = 0.5f;
  public float threshhold;
  private Material separableBlurMaterial;
  private Material contrastCompositeMaterial;
  public float blurSpread = 1f;
  public Shader separableBlurShader;
  public Shader contrastCompositeShader;

  public void OnDisable()
  {
    if ((bool) (Object) this.contrastCompositeMaterial)
      Object.DestroyImmediate((Object) this.contrastCompositeMaterial);
    if (!(bool) (Object) this.separableBlurMaterial)
      return;
    Object.DestroyImmediate((Object) this.separableBlurMaterial);
  }

  public override bool CheckResources()
  {
    this.CheckSupport(false);
    this.contrastCompositeMaterial = this.CheckShaderAndCreateMaterial(this.contrastCompositeShader, this.contrastCompositeMaterial);
    this.separableBlurMaterial = this.CheckShaderAndCreateMaterial(this.separableBlurShader, this.separableBlurMaterial);
    if (!this.isSupported)
      this.ReportAutoDisable();
    return this.isSupported;
  }

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (!this.CheckResources())
    {
      Graphics.Blit((Texture) source, destination);
    }
    else
    {
      RenderTexture temporary1 = RenderTexture.GetTemporary(source.width / 2, source.height / 2, 0);
      RenderTexture temporary2 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0);
      RenderTexture temporary3 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0);
      Graphics.Blit((Texture) source, temporary1);
      Graphics.Blit((Texture) temporary1, temporary2);
      this.separableBlurMaterial.SetVector("offsets", new Vector4(0.0f, this.blurSpread * 1f / (float) temporary2.height, 0.0f, 0.0f));
      Graphics.Blit((Texture) temporary2, temporary3, this.separableBlurMaterial);
      this.separableBlurMaterial.SetVector("offsets", new Vector4(this.blurSpread * 1f / (float) temporary2.width, 0.0f, 0.0f, 0.0f));
      Graphics.Blit((Texture) temporary3, temporary2, this.separableBlurMaterial);
      this.contrastCompositeMaterial.SetTexture("_MainTexBlurred", (Texture) temporary2);
      this.contrastCompositeMaterial.SetFloat("intensity", this.intensity);
      this.contrastCompositeMaterial.SetFloat("threshhold", this.threshhold);
      Graphics.Blit((Texture) source, destination, this.contrastCompositeMaterial);
      RenderTexture.ReleaseTemporary(temporary1);
      RenderTexture.ReleaseTemporary(temporary2);
      RenderTexture.ReleaseTemporary(temporary3);
    }
  }
}
