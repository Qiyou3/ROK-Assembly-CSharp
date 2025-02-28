// Decompiled with JetBrains decompiler
// Type: HologramGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using BlurHelper;
using CodeHatch.Common;
using CodeHatch.UnityQualitySettings;
using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class HologramGUI : ImageEffectBase
{
  public Camera m_guiCamera;
  public RenderTexture guiRenderTexture;
  public Camera m_worldGuiCamera;
  public RenderTexture worldGuiRenderTexture;
  public Texture2D noise;
  public Texture2D bar;
  public Texture2D scanlines;
  public Texture2D pixels;
  public float intensity = 2f;
  public float intensityAlpha = 2f;
  public float intensitySmear = 1f;
  public float intensityPixels = 0.5f;
  public float intensityShadow = 1f;
  public float intensityShadowMax = 0.75f;
  public float minDirty = 0.1f;
  public float noiseIntensity = 0.33333f;
  public float scanlinesIntensity = 0.33333f;
  public float barIntensity = 0.33333f;
  public float scanlinesAccel = 1f;
  private float scanlinesVel;
  private float scanlinesPos;
  public float barScrollSpeed = 1f;
  private float barScroll;
  public Vector2 uvOffset;
  public Blurrer blurrer;
  public ShaderLOD.Quality simplifyAtShaderQuality = ShaderLOD.Quality.Medium;

  public Camera guiCamera
  {
    get
    {
      if ((Object) this.m_guiCamera == (Object) null)
        this.m_guiCamera = Tags.FindComponentWithTag<Camera>("GUI Camera");
      return this.m_guiCamera;
    }
  }

  public Camera worldGuiCamera
  {
    get
    {
      if ((Object) this.m_worldGuiCamera == (Object) null)
        this.m_worldGuiCamera = Tags.FindComponentWithTag<Camera>("World GUI Camera");
      return this.m_worldGuiCamera;
    }
  }

  private bool Simplified
  {
    get
    {
      return (bool) (Object) ShaderLOD.Instance && ShaderLOD.Instance.ShaderQuality <= this.simplifyAtShaderQuality;
    }
  }

  public void Awake()
  {
    if ((Object) this.guiRenderTexture != (Object) null)
      Object.DestroyImmediate((Object) this.guiRenderTexture);
    if (!((Object) this.worldGuiRenderTexture != (Object) null))
      return;
    Object.DestroyImmediate((Object) this.worldGuiRenderTexture);
  }

  public void OnDestroy()
  {
    this.guiRenderTexture = !((Object) this.guiCamera != (Object) null) ? (RenderTexture) null : this.guiCamera.targetTexture;
    if ((Object) this.guiRenderTexture != (Object) null)
    {
      this.guiCamera.targetTexture = (RenderTexture) null;
      Object.DestroyImmediate((Object) this.guiRenderTexture);
    }
    this.worldGuiRenderTexture = !((Object) this.worldGuiCamera != (Object) null) ? (RenderTexture) null : this.worldGuiCamera.targetTexture;
    if (!((Object) this.worldGuiRenderTexture != (Object) null))
      return;
    this.worldGuiCamera.targetTexture = (RenderTexture) null;
    Object.DestroyImmediate((Object) this.worldGuiRenderTexture);
  }

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if ((Object) this.m_guiCamera == (Object) null)
    {
      this.m_guiCamera = Tags.FindComponentWithTag<Camera>("GUI Camera");
      if ((Object) this.m_guiCamera == (Object) null)
      {
        Graphics.Blit((Texture) source, destination);
        return;
      }
    }
    this.guiRenderTexture = this.guiCamera.targetTexture;
    if (((Object) this.guiRenderTexture == (Object) null || this.guiRenderTexture.width != source.width || this.guiRenderTexture.height != source.height) && (Object) this.guiRenderTexture != (Object) RenderTexture.active)
    {
      Object.DestroyImmediate((Object) this.guiRenderTexture);
      this.guiRenderTexture = new RenderTexture(source.width, source.height, 0, source.format);
      this.guiRenderTexture.name = "GUI Render Texture (For GUI Effects)";
      this.guiCamera.targetTexture = this.guiRenderTexture;
    }
    this.guiCamera.aspect = (float) this.guiRenderTexture.width / (float) this.guiRenderTexture.height;
    if ((Object) this.worldGuiCamera != (Object) null)
    {
      this.worldGuiRenderTexture = this.worldGuiCamera.targetTexture;
      if (((Object) this.worldGuiRenderTexture == (Object) null || this.worldGuiRenderTexture.width != source.width || this.worldGuiRenderTexture.height != source.height) && (Object) this.worldGuiRenderTexture != (Object) RenderTexture.active)
      {
        Object.DestroyImmediate((Object) this.worldGuiRenderTexture);
        this.worldGuiRenderTexture = new RenderTexture(source.width, source.height, 0, source.format);
        this.worldGuiRenderTexture.name = "World GUI Render Texture (For GUI Effects)";
        this.worldGuiCamera.targetTexture = this.worldGuiRenderTexture;
      }
      this.worldGuiCamera.aspect = (float) this.worldGuiRenderTexture.width / (float) this.worldGuiRenderTexture.height;
      this.material.SetTexture("_WorldGuiTex", (Texture) this.worldGuiRenderTexture);
      Graphics.Blit((Texture) this.guiRenderTexture, this.guiRenderTexture, this.material, 1);
    }
    if (this.Simplified)
    {
      this.material.SetTexture("_GuiTex", (Texture) this.guiRenderTexture);
      Graphics.Blit((Texture) source, destination, this.material, 2);
    }
    else
    {
      float num = this.noiseIntensity + this.scanlinesIntensity + this.barIntensity;
      if ((double) num == 0.0)
      {
        this.noiseIntensity = 0.33333f;
        this.scanlinesIntensity = 0.33333f;
        this.barIntensity = 0.33333f;
      }
      else
      {
        this.noiseIntensity /= num;
        this.scanlinesIntensity /= num;
        this.barIntensity /= num;
      }
      this.scanlinesPos += this.scanlinesVel + this.scanlinesAccel;
      this.scanlinesVel += this.scanlinesAccel;
      this.scanlinesPos -= Mathf.Round(this.scanlinesPos);
      this.scanlinesVel -= Mathf.Round(this.scanlinesVel);
      this.barScroll += Time.deltaTime * this.barScrollSpeed;
      this.barScroll -= Mathf.Round(this.barScroll);
      this.material.SetTexture("_GuiTex", (Texture) this.guiRenderTexture);
      this.material.SetTexture("_NoiseTex", (Texture) this.noise);
      this.material.SetTexture("_BarTex", (Texture) this.bar);
      this.material.SetTexture("_ScanlinesTex", (Texture) this.scanlines);
      this.material.SetTexture("_PixelsTex", (Texture) this.pixels);
      if ((Object) this.noise != (Object) null)
        this.material.SetVector("_NoiseScale", new Vector4((float) this.noise.width, (float) this.noise.height, Random.value, Random.value));
      if ((Object) this.scanlines != (Object) null)
        this.material.SetVector("_ScanlinesScale", new Vector4((float) this.scanlines.width, (float) this.scanlines.height, this.scanlinesPos, 0.0f));
      if ((Object) this.bar != (Object) null)
        this.material.SetVector("_BarScale", new Vector4((float) this.bar.width, (float) this.bar.height, 0.0f, this.barScroll));
      if ((Object) this.pixels != (Object) null)
        this.material.SetVector("_PixelsScale", new Vector4((float) this.pixels.width, (float) this.pixels.height, 0.0f, 0.0f));
      this.material.SetFloat("_NoiseIntensity", this.noiseIntensity);
      this.material.SetFloat("_ScanlinesIntensity", this.scanlinesIntensity);
      this.material.SetFloat("_BarIntensity", this.barIntensity);
      this.material.SetFloat("_MinDirty", this.minDirty);
      this.material.SetFloat("_Intensity", this.intensity);
      this.material.SetFloat("_IntensityAlpha", this.intensityAlpha);
      this.material.SetFloat("_IntensityPixels", this.intensityPixels);
      this.material.SetFloat("_IntensityShadow", this.intensityShadow);
      this.material.SetFloat("_IntensityShadowMax", this.intensityShadowMax);
      this.material.SetFloat("_IntensitySmear", this.intensitySmear);
      this.material.SetVector("_UvOffset", (Vector4) this.uvOffset);
      RenderTexture temporaryBlurred = this.blurrer.GetTemporaryBlurred(this.guiRenderTexture);
      this.material.SetTexture("_GuiTexBlurred", (Texture) temporaryBlurred);
      if ((bool) (Object) temporaryBlurred)
        RenderTexture.ReleaseTemporary(temporaryBlurred);
      Graphics.Blit((Texture) source, destination, this.material, 0);
    }
  }
}
