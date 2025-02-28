// Decompiled with JetBrains decompiler
// Type: BloomAndLensFlares
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Configuration;
using UnityEngine;

#nullable disable
[ConfigurableByName("enabled", "Bloom & Lens Flare", false, new object[] {true, true, true, true})]
[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
[AddComponentMenu("Image Effects/Bloom (HDR, Lens Flares)")]
[Label("Effects Quality")]
public class BloomAndLensFlares : PostEffectsBase, IConfigurable
{
  public TweakMode34 tweakMode;
  public BloomScreenBlendMode screenBlendMode = BloomScreenBlendMode.Add;
  public HDRBloomMode hdr;
  private bool doHdr;
  public float sepBlurSpread = 1.5f;
  public float useSrcAlphaAsMask = 0.5f;
  public float bloomIntensity = 1f;
  public float bloomThreshhold = 0.5f;
  public int bloomBlurIterations = 2;
  public bool lensflares;
  public int hollywoodFlareBlurIterations = 2;
  public LensflareStyle34 lensflareMode = LensflareStyle34.Anamorphic;
  public float hollyStretchWidth = 3.5f;
  public float lensflareIntensity = 1f;
  public float lensflareThreshhold = 0.3f;
  public Color flareColorA = new Color(0.4f, 0.4f, 0.8f, 0.75f);
  public Color flareColorB = new Color(0.4f, 0.8f, 0.8f, 0.75f);
  public Color flareColorC = new Color(0.8f, 0.4f, 0.8f, 0.75f);
  public Color flareColorD = new Color(0.8f, 0.4f, 0.0f, 0.75f);
  public float blurWidth = 1f;
  public Texture2D lensFlareVignetteMask;
  public Shader lensFlareShader;
  private Material lensFlareMaterial;
  public Shader vignetteShader;
  private Material vignetteMaterial;
  public Shader separableBlurShader;
  private Material separableBlurMaterial;
  public Shader addBrightStuffOneOneShader;
  private Material addBrightStuffBlendOneOneMaterial;
  public Shader screenBlendShader;
  private Material screenBlend;
  public Shader hollywoodFlaresShader;
  private Material hollywoodFlaresMaterial;
  public Shader brightPassFilterShader;
  private Material brightPassFilterMaterial;

  public void Awake() => this.InitializeConfigurable();

  public void ApplyConfiguration()
  {
  }

  public void OnDisable()
  {
    if ((bool) (Object) this.screenBlend)
      Object.DestroyImmediate((Object) this.screenBlend);
    if ((bool) (Object) this.lensFlareMaterial)
      Object.DestroyImmediate((Object) this.lensFlareMaterial);
    if ((bool) (Object) this.vignetteMaterial)
      Object.DestroyImmediate((Object) this.vignetteMaterial);
    if ((bool) (Object) this.separableBlurMaterial)
      Object.DestroyImmediate((Object) this.separableBlurMaterial);
    if ((bool) (Object) this.addBrightStuffBlendOneOneMaterial)
      Object.DestroyImmediate((Object) this.addBrightStuffBlendOneOneMaterial);
    if ((bool) (Object) this.hollywoodFlaresMaterial)
      Object.DestroyImmediate((Object) this.hollywoodFlaresMaterial);
    if (!(bool) (Object) this.brightPassFilterMaterial)
      return;
    Object.DestroyImmediate((Object) this.brightPassFilterMaterial);
  }

  public override bool CheckResources()
  {
    this.CheckSupport(false);
    this.screenBlend = this.CheckShaderAndCreateMaterial(this.screenBlendShader, this.screenBlend);
    this.lensFlareMaterial = this.CheckShaderAndCreateMaterial(this.lensFlareShader, this.lensFlareMaterial);
    this.vignetteMaterial = this.CheckShaderAndCreateMaterial(this.vignetteShader, this.vignetteMaterial);
    this.separableBlurMaterial = this.CheckShaderAndCreateMaterial(this.separableBlurShader, this.separableBlurMaterial);
    this.addBrightStuffBlendOneOneMaterial = this.CheckShaderAndCreateMaterial(this.addBrightStuffOneOneShader, this.addBrightStuffBlendOneOneMaterial);
    this.hollywoodFlaresMaterial = this.CheckShaderAndCreateMaterial(this.hollywoodFlaresShader, this.hollywoodFlaresMaterial);
    this.brightPassFilterMaterial = this.CheckShaderAndCreateMaterial(this.brightPassFilterShader, this.brightPassFilterMaterial);
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
      this.doHdr = false;
      this.doHdr = this.hdr != HDRBloomMode.Auto ? this.hdr == HDRBloomMode.On : source.format == RenderTextureFormat.ARGBHalf && this.GetComponent<Camera>().hdr;
      this.doHdr = this.doHdr && this.supportHDRTextures;
      BloomScreenBlendMode pass = this.screenBlendMode;
      if (this.doHdr)
        pass = BloomScreenBlendMode.Add;
      RenderTextureFormat format = !this.doHdr ? RenderTextureFormat.Default : RenderTextureFormat.ARGBHalf;
      RenderTexture temporary1 = RenderTexture.GetTemporary(source.width / 2, source.height / 2, 0, format);
      RenderTexture temporary2 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0, format);
      RenderTexture temporary3 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0, format);
      RenderTexture temporary4 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0, format);
      float num1 = (float) (1.0 * (double) source.width / (1.0 * (double) source.height));
      float num2 = 1f / 512f;
      Graphics.Blit((Texture) source, temporary1, this.screenBlend, 2);
      Graphics.Blit((Texture) temporary1, temporary2, this.screenBlend, 2);
      RenderTexture.ReleaseTemporary(temporary1);
      this.BrightFilter(this.bloomThreshhold, this.useSrcAlphaAsMask, temporary2, temporary3);
      if (this.bloomBlurIterations < 1)
        this.bloomBlurIterations = 1;
      for (int index = 0; index < this.bloomBlurIterations; ++index)
      {
        float num3 = (float) (1.0 + (double) index * 0.5) * this.sepBlurSpread;
        this.separableBlurMaterial.SetVector("offsets", new Vector4(0.0f, num3 * num2, 0.0f, 0.0f));
        Graphics.Blit(index != 0 ? (Texture) temporary2 : (Texture) temporary3, temporary4, this.separableBlurMaterial);
        this.separableBlurMaterial.SetVector("offsets", new Vector4(num3 / num1 * num2, 0.0f, 0.0f, 0.0f));
        Graphics.Blit((Texture) temporary4, temporary2, this.separableBlurMaterial);
      }
      if (this.lensflares)
      {
        if (this.lensflareMode == LensflareStyle34.Ghosting)
        {
          this.BrightFilter(this.lensflareThreshhold, 0.0f, temporary2, temporary4);
          this.Vignette(0.975f, temporary4, temporary3);
          this.BlendFlares(temporary3, temporary2);
        }
        else
        {
          this.hollywoodFlaresMaterial.SetVector("_Threshhold", new Vector4(this.lensflareThreshhold, (float) (1.0 / (1.0 - (double) this.lensflareThreshhold)), 0.0f, 0.0f));
          this.hollywoodFlaresMaterial.SetVector("tintColor", new Vector4(this.flareColorA.r, this.flareColorA.g, this.flareColorA.b, this.flareColorA.a) * this.flareColorA.a * this.lensflareIntensity);
          Graphics.Blit((Texture) temporary4, temporary3, this.hollywoodFlaresMaterial, 2);
          Graphics.Blit((Texture) temporary3, temporary4, this.hollywoodFlaresMaterial, 3);
          this.hollywoodFlaresMaterial.SetVector("offsets", new Vector4(this.sepBlurSpread * 1f / num1 * num2, 0.0f, 0.0f, 0.0f));
          this.hollywoodFlaresMaterial.SetFloat("stretchWidth", this.hollyStretchWidth);
          Graphics.Blit((Texture) temporary4, temporary3, this.hollywoodFlaresMaterial, 1);
          this.hollywoodFlaresMaterial.SetFloat("stretchWidth", this.hollyStretchWidth * 2f);
          Graphics.Blit((Texture) temporary3, temporary4, this.hollywoodFlaresMaterial, 1);
          this.hollywoodFlaresMaterial.SetFloat("stretchWidth", this.hollyStretchWidth * 4f);
          Graphics.Blit((Texture) temporary4, temporary3, this.hollywoodFlaresMaterial, 1);
          if (this.lensflareMode == LensflareStyle34.Anamorphic)
          {
            for (int index = 0; index < this.hollywoodFlareBlurIterations; ++index)
            {
              this.separableBlurMaterial.SetVector("offsets", new Vector4(this.hollyStretchWidth * 2f / num1 * num2, 0.0f, 0.0f, 0.0f));
              Graphics.Blit((Texture) temporary3, temporary4, this.separableBlurMaterial);
              this.separableBlurMaterial.SetVector("offsets", new Vector4(this.hollyStretchWidth * 2f / num1 * num2, 0.0f, 0.0f, 0.0f));
              Graphics.Blit((Texture) temporary4, temporary3, this.separableBlurMaterial);
            }
            this.AddTo(1f, temporary3, temporary2);
          }
          else
          {
            for (int index = 0; index < this.hollywoodFlareBlurIterations; ++index)
            {
              this.separableBlurMaterial.SetVector("offsets", new Vector4(this.hollyStretchWidth * 2f / num1 * num2, 0.0f, 0.0f, 0.0f));
              Graphics.Blit((Texture) temporary3, temporary4, this.separableBlurMaterial);
              this.separableBlurMaterial.SetVector("offsets", new Vector4(this.hollyStretchWidth * 2f / num1 * num2, 0.0f, 0.0f, 0.0f));
              Graphics.Blit((Texture) temporary4, temporary3, this.separableBlurMaterial);
            }
            this.Vignette(1f, temporary3, temporary4);
            this.BlendFlares(temporary4, temporary3);
            this.AddTo(1f, temporary3, temporary2);
          }
        }
      }
      this.screenBlend.SetFloat("_Intensity", this.bloomIntensity);
      this.screenBlend.SetTexture("_ColorBuffer", (Texture) source);
      Graphics.Blit((Texture) temporary2, destination, this.screenBlend, (int) pass);
      RenderTexture.ReleaseTemporary(temporary2);
      RenderTexture.ReleaseTemporary(temporary3);
      RenderTexture.ReleaseTemporary(temporary4);
    }
  }

  private void AddTo(float intensity_, RenderTexture from, RenderTexture to)
  {
    this.addBrightStuffBlendOneOneMaterial.SetFloat("_Intensity", intensity_);
    Graphics.Blit((Texture) from, to, this.addBrightStuffBlendOneOneMaterial);
  }

  private void BlendFlares(RenderTexture from, RenderTexture to)
  {
    this.lensFlareMaterial.SetVector("colorA", new Vector4(this.flareColorA.r, this.flareColorA.g, this.flareColorA.b, this.flareColorA.a) * this.lensflareIntensity);
    this.lensFlareMaterial.SetVector("colorB", new Vector4(this.flareColorB.r, this.flareColorB.g, this.flareColorB.b, this.flareColorB.a) * this.lensflareIntensity);
    this.lensFlareMaterial.SetVector("colorC", new Vector4(this.flareColorC.r, this.flareColorC.g, this.flareColorC.b, this.flareColorC.a) * this.lensflareIntensity);
    this.lensFlareMaterial.SetVector("colorD", new Vector4(this.flareColorD.r, this.flareColorD.g, this.flareColorD.b, this.flareColorD.a) * this.lensflareIntensity);
    Graphics.Blit((Texture) from, to, this.lensFlareMaterial);
  }

  private void BrightFilter(
    float thresh,
    float useAlphaAsMask,
    RenderTexture from,
    RenderTexture to)
  {
    if (this.doHdr)
      this.brightPassFilterMaterial.SetVector("threshhold", new Vector4(thresh, 1f, 0.0f, 0.0f));
    else
      this.brightPassFilterMaterial.SetVector("threshhold", new Vector4(thresh, (float) (1.0 / (1.0 - (double) thresh)), 0.0f, 0.0f));
    this.brightPassFilterMaterial.SetFloat("useSrcAlphaAsMask", useAlphaAsMask);
    Graphics.Blit((Texture) from, to, this.brightPassFilterMaterial);
  }

  private void Vignette(float amount, RenderTexture from, RenderTexture to)
  {
    if ((bool) (Object) this.lensFlareVignetteMask)
    {
      this.screenBlend.SetTexture("_ColorBuffer", (Texture) this.lensFlareVignetteMask);
      Graphics.Blit((Texture) from, to, this.screenBlend, 3);
    }
    else
    {
      this.vignetteMaterial.SetFloat("vignetteIntensity", amount);
      Graphics.Blit((Texture) from, to, this.vignetteMaterial);
    }
  }
}
