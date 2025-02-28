// Decompiled with JetBrains decompiler
// Type: DepthOfField34
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Configuration;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (Camera))]
[ExecuteInEditMode]
[Label("Effects Quality")]
[AddComponentMenu("Image Effects/Depth of Field (3.4)")]
[ConfigurableByName("enabled", "Depth of Field", false, new object[] {false, false, true, true})]
public class DepthOfField34 : PostEffectsBase, IConfigurable
{
  public static int SMOOTH_DOWNSAMPLE_PASS = 6;
  public static float BOKEH_EXTRA_BLUR = 2f;
  public Dof34QualitySetting quality = Dof34QualitySetting.OnlyBackground;
  public DofResolution resolution = DofResolution.Low;
  public bool simpleTweakMode = true;
  public float focalPoint = 1f;
  public float smoothness = 0.5f;
  public float focalZDistance;
  public float focalZStartCurve = 1f;
  public float focalZEndCurve = 1f;
  private float focalStartCurve = 2f;
  private float focalEndCurve = 2f;
  private float focalDistance01 = 0.1f;
  public Transform objectFocus;
  public float focalSize;
  public DofBlurriness bluriness = DofBlurriness.High;
  public float maxBlurSpread = 1.75f;
  public float foregroundBlurExtrude = 1.15f;
  public Shader dofBlurShader;
  private Material dofBlurMaterial;
  public Shader dofShader;
  private Material dofMaterial;
  public bool visualize;
  public BokehDestination bokehDestination = BokehDestination.Background;
  private float widthOverHeight = 1.25f;
  private float oneOverBaseSize = 1f / 512f;
  public bool bokeh;
  public bool bokehSupport = true;
  public Shader bokehShader;
  public Texture2D bokehTexture;
  public float bokehScale = 2.4f;
  public float bokehIntensity = 0.15f;
  public float bokehThreshholdContrast = 0.1f;
  public float bokehThreshholdLuminance = 0.55f;
  public int bokehDownsample = 1;
  private Material bokehMaterial;
  private RenderTexture foregroundTexture;
  private RenderTexture mediumRezWorkTexture;
  private RenderTexture finalDefocus;
  private RenderTexture lowRezWorkTexture;
  private RenderTexture bokehSource;
  private RenderTexture bokehSource2;

  public void Awake() => this.InitializeConfigurable();

  public void ApplyConfiguration()
  {
  }

  public void CreateMaterials()
  {
    this.dofBlurMaterial = this.CheckShaderAndCreateMaterial(this.dofBlurShader, this.dofBlurMaterial);
    this.dofMaterial = this.CheckShaderAndCreateMaterial(this.dofShader, this.dofMaterial);
    this.bokehSupport = this.bokehShader.isSupported;
    if (!this.bokeh || !this.bokehSupport || !(bool) (Object) this.bokehShader)
      return;
    this.bokehMaterial = this.CheckShaderAndCreateMaterial(this.bokehShader, this.bokehMaterial);
  }

  public override bool CheckResources()
  {
    this.CheckSupport(true);
    this.dofBlurMaterial = this.CheckShaderAndCreateMaterial(this.dofBlurShader, this.dofBlurMaterial);
    this.dofMaterial = this.CheckShaderAndCreateMaterial(this.dofShader, this.dofMaterial);
    this.bokehSupport = this.bokehShader.isSupported;
    if (this.bokeh && this.bokehSupport && (bool) (Object) this.bokehShader)
      this.bokehMaterial = this.CheckShaderAndCreateMaterial(this.bokehShader, this.bokehMaterial);
    if (!this.isSupported)
      this.ReportAutoDisable();
    return this.isSupported;
  }

  public void OnDisable()
  {
    Quads.Cleanup();
    if ((bool) (Object) this.dofBlurMaterial)
      Object.DestroyImmediate((Object) this.dofBlurMaterial);
    if ((bool) (Object) this.dofMaterial)
      Object.DestroyImmediate((Object) this.dofMaterial);
    if (!(bool) (Object) this.bokehMaterial)
      return;
    Object.DestroyImmediate((Object) this.bokehMaterial);
  }

  public override void OnEnable()
  {
    this.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
  }

  public float FocalDistance01(float worldDist)
  {
    return this.GetComponent<Camera>().WorldToViewportPoint((worldDist - this.GetComponent<Camera>().nearClipPlane) * this.GetComponent<Camera>().transform.forward + this.GetComponent<Camera>().transform.position).z / (this.GetComponent<Camera>().farClipPlane - this.GetComponent<Camera>().nearClipPlane);
  }

  public int GetDividerBasedOnQuality()
  {
    int dividerBasedOnQuality = 1;
    if (this.resolution == DofResolution.Medium)
      dividerBasedOnQuality = 2;
    else if (this.resolution == DofResolution.Low)
      dividerBasedOnQuality = 4;
    return dividerBasedOnQuality;
  }

  public int GetLowResolutionDividerBasedOnQuality(int baseDivider)
  {
    int dividerBasedOnQuality = baseDivider;
    if (this.resolution == DofResolution.Medium)
      dividerBasedOnQuality *= 2;
    if (this.resolution == DofResolution.Low)
      dividerBasedOnQuality *= 4;
    return dividerBasedOnQuality;
  }

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (!this.CheckResources())
    {
      Graphics.Blit((Texture) source, destination);
    }
    else
    {
      if ((double) this.smoothness < 0.10000000149011612)
        this.smoothness = 0.1f;
      this.bokeh = this.bokeh && this.bokehSupport;
      float num1 = !this.bokeh ? 1f : DepthOfField34.BOKEH_EXTRA_BLUR;
      bool flag = this.quality > Dof34QualitySetting.OnlyBackground;
      float num2 = this.focalSize / (this.GetComponent<Camera>().farClipPlane - this.GetComponent<Camera>().nearClipPlane);
      bool blurForeground;
      if (this.simpleTweakMode)
      {
        this.focalDistance01 = !(bool) (Object) this.objectFocus ? this.FocalDistance01(this.focalPoint) : this.GetComponent<Camera>().WorldToViewportPoint(this.objectFocus.position).z / this.GetComponent<Camera>().farClipPlane;
        this.focalStartCurve = this.focalDistance01 * this.smoothness;
        this.focalEndCurve = this.focalStartCurve;
        blurForeground = flag && (double) this.focalPoint > (double) this.GetComponent<Camera>().nearClipPlane + (double) Mathf.Epsilon;
      }
      else
      {
        if ((bool) (Object) this.objectFocus)
        {
          Vector3 viewportPoint = this.GetComponent<Camera>().WorldToViewportPoint(this.objectFocus.position);
          viewportPoint.z /= this.GetComponent<Camera>().farClipPlane;
          this.focalDistance01 = viewportPoint.z;
        }
        else
          this.focalDistance01 = this.FocalDistance01(this.focalZDistance);
        this.focalStartCurve = this.focalZStartCurve;
        this.focalEndCurve = this.focalZEndCurve;
        blurForeground = flag && (double) this.focalPoint > (double) this.GetComponent<Camera>().nearClipPlane + (double) Mathf.Epsilon;
      }
      this.widthOverHeight = (float) (1.0 * (double) source.width / (1.0 * (double) source.height));
      this.oneOverBaseSize = 1f / 512f;
      this.dofMaterial.SetFloat("_ForegroundBlurExtrude", this.foregroundBlurExtrude);
      this.dofMaterial.SetVector("_CurveParams", new Vector4(!this.simpleTweakMode ? this.focalStartCurve : 1f / this.focalStartCurve, !this.simpleTweakMode ? this.focalEndCurve : 1f / this.focalEndCurve, num2 * 0.5f, this.focalDistance01));
      this.dofMaterial.SetVector("_InvRenderTargetSize", new Vector4((float) (1.0 / (1.0 * (double) source.width)), (float) (1.0 / (1.0 * (double) source.height)), 0.0f, 0.0f));
      int dividerBasedOnQuality1 = this.GetDividerBasedOnQuality();
      int dividerBasedOnQuality2 = this.GetLowResolutionDividerBasedOnQuality(dividerBasedOnQuality1);
      this.AllocateTextures(blurForeground, source, dividerBasedOnQuality1, dividerBasedOnQuality2);
      Graphics.Blit((Texture) source, source, this.dofMaterial, 3);
      this.Downsample(source, this.mediumRezWorkTexture);
      this.Blur(this.mediumRezWorkTexture, this.mediumRezWorkTexture, DofBlurriness.Low, 4, this.maxBlurSpread);
      if (this.bokeh && (this.bokehDestination & BokehDestination.Background) != (BokehDestination) 0)
      {
        this.dofMaterial.SetVector("_Threshhold", new Vector4(this.bokehThreshholdContrast, this.bokehThreshholdLuminance, 0.95f, 0.0f));
        Graphics.Blit((Texture) this.mediumRezWorkTexture, this.bokehSource2, this.dofMaterial, 11);
        Graphics.Blit((Texture) this.mediumRezWorkTexture, this.lowRezWorkTexture);
        this.Blur(this.lowRezWorkTexture, this.lowRezWorkTexture, this.bluriness, 0, this.maxBlurSpread * num1);
      }
      else
      {
        this.Downsample(this.mediumRezWorkTexture, this.lowRezWorkTexture);
        this.Blur(this.lowRezWorkTexture, this.lowRezWorkTexture, this.bluriness, 0, this.maxBlurSpread);
      }
      this.dofBlurMaterial.SetTexture("_TapLow", (Texture) this.lowRezWorkTexture);
      this.dofBlurMaterial.SetTexture("_TapMedium", (Texture) this.mediumRezWorkTexture);
      Graphics.Blit((Texture) null, this.finalDefocus, this.dofBlurMaterial, 3);
      if (this.bokeh && (this.bokehDestination & BokehDestination.Background) != (BokehDestination) 0)
        this.AddBokeh(this.bokehSource2, this.bokehSource, this.finalDefocus);
      this.dofMaterial.SetTexture("_TapLowBackground", (Texture) this.finalDefocus);
      this.dofMaterial.SetTexture("_TapMedium", (Texture) this.mediumRezWorkTexture);
      Graphics.Blit((Texture) source, !blurForeground ? destination : this.foregroundTexture, this.dofMaterial, !this.visualize ? 0 : 2);
      if (blurForeground)
      {
        Graphics.Blit((Texture) this.foregroundTexture, source, this.dofMaterial, 5);
        this.Downsample(source, this.mediumRezWorkTexture);
        this.BlurFg(this.mediumRezWorkTexture, this.mediumRezWorkTexture, DofBlurriness.Low, 2, this.maxBlurSpread);
        if (this.bokeh && (this.bokehDestination & BokehDestination.Foreground) != (BokehDestination) 0)
        {
          this.dofMaterial.SetVector("_Threshhold", new Vector4(this.bokehThreshholdContrast * 0.5f, this.bokehThreshholdLuminance, 0.0f, 0.0f));
          Graphics.Blit((Texture) this.mediumRezWorkTexture, this.bokehSource2, this.dofMaterial, 11);
          Graphics.Blit((Texture) this.mediumRezWorkTexture, this.lowRezWorkTexture);
          this.BlurFg(this.lowRezWorkTexture, this.lowRezWorkTexture, this.bluriness, 1, this.maxBlurSpread * num1);
        }
        else
          this.BlurFg(this.mediumRezWorkTexture, this.lowRezWorkTexture, this.bluriness, 1, this.maxBlurSpread);
        Graphics.Blit((Texture) this.lowRezWorkTexture, this.finalDefocus);
        this.dofMaterial.SetTexture("_TapLowForeground", (Texture) this.finalDefocus);
        Graphics.Blit((Texture) source, destination, this.dofMaterial, !this.visualize ? 4 : 1);
        if (this.bokeh && (this.bokehDestination & BokehDestination.Foreground) != (BokehDestination) 0)
          this.AddBokeh(this.bokehSource2, this.bokehSource, destination);
      }
      this.ReleaseTextures();
    }
  }

  public void Blur(
    RenderTexture from,
    RenderTexture to,
    DofBlurriness iterations,
    int blurPass,
    float spread)
  {
    RenderTexture temporary = RenderTexture.GetTemporary(to.width, to.height);
    if (iterations > DofBlurriness.Low)
    {
      this.BlurHex(from, to, blurPass, spread, temporary);
      if (iterations > DofBlurriness.High)
      {
        this.dofBlurMaterial.SetVector("offsets", new Vector4(0.0f, spread * this.oneOverBaseSize, 0.0f, 0.0f));
        Graphics.Blit((Texture) to, temporary, this.dofBlurMaterial, blurPass);
        this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, 0.0f, 0.0f, 0.0f));
        Graphics.Blit((Texture) temporary, to, this.dofBlurMaterial, blurPass);
      }
    }
    else
    {
      this.dofBlurMaterial.SetVector("offsets", new Vector4(0.0f, spread * this.oneOverBaseSize, 0.0f, 0.0f));
      Graphics.Blit((Texture) from, temporary, this.dofBlurMaterial, blurPass);
      this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, 0.0f, 0.0f, 0.0f));
      Graphics.Blit((Texture) temporary, to, this.dofBlurMaterial, blurPass);
    }
    RenderTexture.ReleaseTemporary(temporary);
  }

  public void BlurFg(
    RenderTexture from,
    RenderTexture to,
    DofBlurriness iterations,
    int blurPass,
    float spread)
  {
    this.dofBlurMaterial.SetTexture("_TapHigh", (Texture) from);
    RenderTexture temporary = RenderTexture.GetTemporary(to.width, to.height);
    if (iterations > DofBlurriness.Low)
    {
      this.BlurHex(from, to, blurPass, spread, temporary);
      if (iterations > DofBlurriness.High)
      {
        this.dofBlurMaterial.SetVector("offsets", new Vector4(0.0f, spread * this.oneOverBaseSize, 0.0f, 0.0f));
        Graphics.Blit((Texture) to, temporary, this.dofBlurMaterial, blurPass);
        this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, 0.0f, 0.0f, 0.0f));
        Graphics.Blit((Texture) temporary, to, this.dofBlurMaterial, blurPass);
      }
    }
    else
    {
      this.dofBlurMaterial.SetVector("offsets", new Vector4(0.0f, spread * this.oneOverBaseSize, 0.0f, 0.0f));
      Graphics.Blit((Texture) from, temporary, this.dofBlurMaterial, blurPass);
      this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, 0.0f, 0.0f, 0.0f));
      Graphics.Blit((Texture) temporary, to, this.dofBlurMaterial, blurPass);
    }
    RenderTexture.ReleaseTemporary(temporary);
  }

  public void BlurHex(
    RenderTexture from,
    RenderTexture to,
    int blurPass,
    float spread,
    RenderTexture tmp)
  {
    this.dofBlurMaterial.SetVector("offsets", new Vector4(0.0f, spread * this.oneOverBaseSize, 0.0f, 0.0f));
    Graphics.Blit((Texture) from, tmp, this.dofBlurMaterial, blurPass);
    this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, 0.0f, 0.0f, 0.0f));
    Graphics.Blit((Texture) tmp, to, this.dofBlurMaterial, blurPass);
    this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, spread * this.oneOverBaseSize, 0.0f, 0.0f));
    Graphics.Blit((Texture) to, tmp, this.dofBlurMaterial, blurPass);
    this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, -spread * this.oneOverBaseSize, 0.0f, 0.0f));
    Graphics.Blit((Texture) tmp, to, this.dofBlurMaterial, blurPass);
  }

  public void Downsample(RenderTexture from, RenderTexture to)
  {
    this.dofMaterial.SetVector("_InvRenderTargetSize", new Vector4((float) (1.0 / (1.0 * (double) to.width)), (float) (1.0 / (1.0 * (double) to.height)), 0.0f, 0.0f));
    Graphics.Blit((Texture) from, to, this.dofMaterial, DepthOfField34.SMOOTH_DOWNSAMPLE_PASS);
  }

  public void AddBokeh(RenderTexture bokehInfo, RenderTexture tempTex, RenderTexture finalTarget)
  {
    if (!(bool) (Object) this.bokehMaterial)
      return;
    Mesh[] meshes = Quads.GetMeshes(tempTex.width, tempTex.height);
    RenderTexture.active = tempTex;
    GL.Clear(false, true, new Color(0.0f, 0.0f, 0.0f, 0.0f));
    GL.PushMatrix();
    GL.LoadIdentity();
    bokehInfo.filterMode = FilterMode.Point;
    float num = (float) ((double) bokehInfo.width * 1.0 / ((double) bokehInfo.height * 1.0));
    float x = (float) (2.0 / (1.0 * (double) bokehInfo.width)) + this.bokehScale * this.maxBlurSpread * DepthOfField34.BOKEH_EXTRA_BLUR * this.oneOverBaseSize;
    this.bokehMaterial.SetTexture("_Source", (Texture) bokehInfo);
    this.bokehMaterial.SetTexture("_MainTex", (Texture) this.bokehTexture);
    this.bokehMaterial.SetVector("_ArScale", new Vector4(x, x * num, 0.5f, 0.5f * num));
    this.bokehMaterial.SetFloat("_Intensity", this.bokehIntensity);
    this.bokehMaterial.SetPass(0);
    foreach (Mesh mesh in meshes)
    {
      if ((bool) (Object) mesh)
        Graphics.DrawMeshNow(mesh, Matrix4x4.identity);
    }
    GL.PopMatrix();
    Graphics.Blit((Texture) tempTex, finalTarget, this.dofMaterial, 8);
    bokehInfo.filterMode = FilterMode.Bilinear;
  }

  public void ReleaseTextures()
  {
    if ((bool) (Object) this.foregroundTexture)
      RenderTexture.ReleaseTemporary(this.foregroundTexture);
    if ((bool) (Object) this.finalDefocus)
      RenderTexture.ReleaseTemporary(this.finalDefocus);
    if ((bool) (Object) this.mediumRezWorkTexture)
      RenderTexture.ReleaseTemporary(this.mediumRezWorkTexture);
    if ((bool) (Object) this.lowRezWorkTexture)
      RenderTexture.ReleaseTemporary(this.lowRezWorkTexture);
    if ((bool) (Object) this.bokehSource)
      RenderTexture.ReleaseTemporary(this.bokehSource);
    if (!(bool) (Object) this.bokehSource2)
      return;
    RenderTexture.ReleaseTemporary(this.bokehSource2);
  }

  public void AllocateTextures(
    bool blurForeground,
    RenderTexture source,
    int divider,
    int lowTexDivider)
  {
    this.foregroundTexture = (RenderTexture) null;
    if (blurForeground)
      this.foregroundTexture = RenderTexture.GetTemporary(source.width, source.height, 0);
    this.mediumRezWorkTexture = RenderTexture.GetTemporary(source.width / divider, source.height / divider, 0);
    this.finalDefocus = RenderTexture.GetTemporary(source.width / divider, source.height / divider, 0);
    this.lowRezWorkTexture = RenderTexture.GetTemporary(source.width / lowTexDivider, source.height / lowTexDivider, 0);
    this.bokehSource = (RenderTexture) null;
    this.bokehSource2 = (RenderTexture) null;
    if (this.bokeh)
    {
      this.bokehSource = RenderTexture.GetTemporary(source.width / (lowTexDivider * this.bokehDownsample), source.height / (lowTexDivider * this.bokehDownsample), 0, RenderTextureFormat.ARGBHalf);
      this.bokehSource2 = RenderTexture.GetTemporary(source.width / (lowTexDivider * this.bokehDownsample), source.height / (lowTexDivider * this.bokehDownsample), 0, RenderTextureFormat.ARGBHalf);
      this.bokehSource.filterMode = FilterMode.Bilinear;
      this.bokehSource2.filterMode = FilterMode.Bilinear;
      RenderTexture.active = this.bokehSource2;
      GL.Clear(false, true, new Color(0.0f, 0.0f, 0.0f, 0.0f));
    }
    source.filterMode = FilterMode.Bilinear;
    this.finalDefocus.filterMode = FilterMode.Bilinear;
    this.mediumRezWorkTexture.filterMode = FilterMode.Bilinear;
    this.lowRezWorkTexture.filterMode = FilterMode.Bilinear;
    if (!(bool) (Object) this.foregroundTexture)
      return;
    this.foregroundTexture.filterMode = FilterMode.Bilinear;
  }
}
