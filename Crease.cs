// Decompiled with JetBrains decompiler
// Type: Crease
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("Image Effects/Crease")]
[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
public class Crease : PostEffectsBase
{
  public float intensity = 0.5f;
  public int softness = 1;
  public float spread = 1f;
  public Shader blurShader;
  private Material blurMaterial;
  public Shader depthFetchShader;
  private Material depthFetchMaterial;
  public Shader creaseApplyShader;
  private Material creaseApplyMaterial;

  public void OnDisable()
  {
    if ((bool) (Object) this.blurMaterial)
      Object.DestroyImmediate((Object) this.blurMaterial);
    if ((bool) (Object) this.depthFetchMaterial)
      Object.DestroyImmediate((Object) this.depthFetchMaterial);
    if (!(bool) (Object) this.creaseApplyMaterial)
      return;
    Object.DestroyImmediate((Object) this.creaseApplyMaterial);
  }

  public override bool CheckResources()
  {
    this.CheckSupport(true);
    this.blurMaterial = this.CheckShaderAndCreateMaterial(this.blurShader, this.blurMaterial);
    this.depthFetchMaterial = this.CheckShaderAndCreateMaterial(this.depthFetchShader, this.depthFetchMaterial);
    this.creaseApplyMaterial = this.CheckShaderAndCreateMaterial(this.creaseApplyShader, this.creaseApplyMaterial);
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
      float num1 = (float) (1.0 * (double) source.width / (1.0 * (double) source.height));
      float num2 = 1f / 512f;
      RenderTexture temporary1 = RenderTexture.GetTemporary(source.width, source.height, 0);
      RenderTexture temporary2 = RenderTexture.GetTemporary(source.width / 2, source.height / 2, 0);
      RenderTexture temporary3 = RenderTexture.GetTemporary(source.width / 2, source.height / 2, 0);
      Graphics.Blit((Texture) source, temporary1, this.depthFetchMaterial);
      Graphics.Blit((Texture) temporary1, temporary2);
      for (int index = 0; index < this.softness; ++index)
      {
        this.blurMaterial.SetVector("offsets", new Vector4(0.0f, this.spread * num2, 0.0f, 0.0f));
        Graphics.Blit((Texture) temporary2, temporary3, this.blurMaterial);
        this.blurMaterial.SetVector("offsets", new Vector4(this.spread * num2 / num1, 0.0f, 0.0f, 0.0f));
        Graphics.Blit((Texture) temporary3, temporary2, this.blurMaterial);
      }
      this.creaseApplyMaterial.SetTexture("_HrDepthTex", (Texture) temporary1);
      this.creaseApplyMaterial.SetTexture("_LrDepthTex", (Texture) temporary2);
      this.creaseApplyMaterial.SetFloat("intensity", this.intensity);
      Graphics.Blit((Texture) source, destination, this.creaseApplyMaterial);
      RenderTexture.ReleaseTemporary(temporary1);
      RenderTexture.ReleaseTemporary(temporary2);
      RenderTexture.ReleaseTemporary(temporary3);
    }
  }
}
