// Decompiled with JetBrains decompiler
// Type: HumanEyeTonemapper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Configuration;
using UnityEngine;

#nullable disable
[Label("Window Settings")]
[AddComponentMenu("Image Effects/Custom/Human Eye Tonemapper")]
[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
public class HumanEyeTonemapper : MonoBehaviour, IConfigurable
{
  public Shader shader;
  private Material _material;
  private bool _supported;
  private RenderTexture _accumTexture;
  public Color blueShiftColor;
  public float blueShiftBase = 0.0001f;
  public float blueShiftPowerFactor = 200f;
  public bool applyAccumulationEffect = true;
  public float accumBase = 0.0001f;
  public float accumPowerFactor = 200f;
  public float afterimageLogarithmicDecay = 0.6f;
  public float afterimageLinearDecay = 0.0003f;
  public Texture2D noise;
  public float noiseIntensity = 0.0002f;
  public bool useLocalAdaptation;
  [Range(0.1f, 1f)]
  public float downsampleScale = 0.5f;
  public float delta;
  [Range(1f, 20f)]
  public int iterations;
  public float iterationScaleFactor = 3f;
  public float radius;
  [Tooltip("Maximum for range that can be fit into the visible gamut of your monitor. Light intensities above this range will appear too bright to view clearly.")]
  public float maxEye = 0.2f;
  [Tooltip("Minimum for range that can be fit into the visible gamut of your monitor. Light intensities below this range will appear too dark to view clearly.")]
  public float minEye = 1f / 1000f;
  [Tooltip("Maximum for range of in-game brightness that can be reproduced by your monitor.")]
  public float maxMonitor = 0.09f;
  [Tooltip("Minumum for range of in-game brightness that can be reproduced by your monitor.")]
  public float minMonitor = 3f / 1000f;
  [DefaultValue(9.5f)]
  [Configurable]
  [Range(0.15f, 15f)]
  [Label("Brightness")]
  [QualityLevels(new object[] {7f, 8f, 8f, 9f})]
  [Tooltip("Brightness correction that is applied to the final image.")]
  public float brightnessSetting = 8f;
  public float brightnessScale = 3.66666675f;
  public bool viewAdaptation;

  public float Brightness => this.brightnessScale * this.brightnessSetting;

  public void OnEnable()
  {
    this.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
    this.Start();
  }

  private static Material CreateMaterial(Shader shader)
  {
    if (!(bool) (Object) shader)
      return (Material) null;
    Material material = new Material(shader);
    material.hideFlags = HideFlags.HideAndDontSave;
    return material;
  }

  public void OnDisable()
  {
    if (!((Object) this._material != (Object) null))
      return;
    Object.DestroyImmediate((Object) this._material);
    this._material = (Material) null;
  }

  public void Awake() => this.InitializeConfigurable();

  public void Start()
  {
    if (!SystemInfo.supportsImageEffects)
    {
      this.LogInfo<HumanEyeTonemapper>("Could not use effect because: !SystemInfo.supportsImageEffects");
      this._supported = false;
    }
    else if (!this.shader.isSupported)
    {
      this.LogInfo<HumanEyeTonemapper>("Could not use effect because: !shader.isSupported");
      this._supported = false;
    }
    else
    {
      if (!(bool) (Object) this._material)
        this._material = HumanEyeTonemapper.CreateMaterial(this.shader);
      if ((Object) this._material == (Object) null)
      {
        this.LogInfo<HumanEyeTonemapper>("Could not use effect because: _material == null");
        this._supported = false;
      }
      else
        this._supported = true;
    }
  }

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (!this._supported)
      return;
    this.CreateNewMaterialIfNull();
    this.ApplyToneMapping(source, destination);
  }

  private void ApplyToneMapping(RenderTexture source, RenderTexture destination)
  {
    RenderTexture source1 = !this.applyAccumulationEffect ? source : this.UpdateAccumTexture(source);
    if (this.useLocalAdaptation)
      this.BlitCompositeWithAdaptation(source1, destination);
    else
      this.BlitCompositeNoAdaptation(source1, destination);
  }

  private void BlitCompositeWithAdaptation(RenderTexture source, RenderTexture destination)
  {
    RenderTexture blurredSource;
    RenderTexture blurredSource2;
    this.CreateTwoTemporaryBlurTexturesForSource(source, out blurredSource, out blurredSource2);
    this.BlitLuminance(source, blurredSource);
    this._material.SetTexture("_Blurred", (Texture) this.BlurLuminanceMap(blurredSource, blurredSource2));
    if (this.viewAdaptation)
      this.BlitViewAdaptationTexture(source, destination);
    else
      this.BlitComposite(source, destination);
    RenderTexture.ReleaseTemporary(blurredSource);
    RenderTexture.ReleaseTemporary(blurredSource2);
  }

  private void BlitComposite(RenderTexture source, RenderTexture destination)
  {
    this._material.SetColor("_BlueShiftColor", this.blueShiftColor);
    this._material.SetFloat("_BlueShiftBase", this.blueShiftBase);
    this._material.SetFloat("_BlueShiftPowerFactor", this.blueShiftPowerFactor);
    Graphics.Blit((Texture) source, destination, this._material, 1);
  }

  private void BlitViewAdaptationTexture(RenderTexture source, RenderTexture destination)
  {
    this._material.SetColor("_BlueShiftColor", this.blueShiftColor);
    this._material.SetFloat("_BlueShiftBase", this.blueShiftBase);
    this._material.SetFloat("_BlueShiftPowerFactor", this.blueShiftPowerFactor);
    Graphics.Blit((Texture) source, destination, this._material, 3);
  }

  private RenderTexture BlurLuminanceMap(RenderTexture blurredLumMapA, RenderTexture blurredLumMapB)
  {
    int num = 0;
    float radius = this.radius;
    float delta = this.delta;
    while (num < this.iterations)
    {
      this._material.SetFloat("_Radius", radius);
      this._material.SetFloat("_Delta", delta);
      if (num % 2 == 0)
        Graphics.Blit((Texture) blurredLumMapA, blurredLumMapB, this._material, 0);
      else
        Graphics.Blit((Texture) blurredLumMapB, blurredLumMapA, this._material, 0);
      ++num;
      radius /= this.iterationScaleFactor;
    }
    return num % 2 != 0 ? blurredLumMapB : blurredLumMapA;
  }

  private void BlitLuminance(RenderTexture source, RenderTexture blurredLumMapA)
  {
    this._material.SetFloat("_MaxAdapt", this.maxEye);
    this._material.SetFloat("_MinAdapt", this.minEye);
    float num1 = Mathf.Log(1f / this.minMonitor);
    float num2 = Mathf.Log(1f / this.maxMonitor);
    float p = (float) (((double) num1 + (double) num2) / 2.0);
    float num3 = (float) (((double) num1 - (double) num2) / 2.0);
    this._material.SetFloat("_LogCenter", p);
    this._material.SetFloat("_LogRange", num3);
    this._material.SetFloat("_MiddleGrey", Mathf.Pow(2.71828f, p) / this.Brightness);
    Graphics.Blit((Texture) source, blurredLumMapA, this._material, 2);
  }

  private void CreateTwoTemporaryBlurTexturesForSource(
    RenderTexture source,
    out RenderTexture blurredSource,
    out RenderTexture blurredSource2)
  {
    int blurTextureWidth = (int) ((double) source.width * (double) this.downsampleScale);
    int blurTextureHeight = (int) ((double) source.height * (double) this.downsampleScale);
    FilterMode blurredFilterMode = blurTextureWidth != source.width || blurTextureHeight != source.height ? FilterMode.Bilinear : FilterMode.Point;
    blurredSource = HumanEyeTonemapper.CreateTemporaryBlurRenderTexture(source, blurTextureWidth, blurTextureHeight, blurredFilterMode);
    blurredSource2 = HumanEyeTonemapper.CreateTemporaryBlurRenderTexture(source, blurTextureWidth, blurTextureHeight, blurredFilterMode);
  }

  private static RenderTexture CreateTemporaryBlurRenderTexture(
    RenderTexture source,
    int blurTextureWidth,
    int blurTextureHeight,
    FilterMode blurredFilterMode)
  {
    RenderTexture temporary = RenderTexture.GetTemporary(blurTextureWidth, blurTextureHeight, 0, source.format);
    temporary.filterMode = blurredFilterMode;
    return temporary;
  }

  private void BlitCompositeNoAdaptation(RenderTexture source, RenderTexture destination)
  {
    this._material.SetFloat("_MiddleGrey", 1f / this.Brightness);
    Graphics.Blit((Texture) source, destination, this._material, 4);
  }

  private RenderTexture UpdateAccumTexture(RenderTexture source)
  {
    this.CreateNewAccumTextureIfInvalid(source);
    this._material.SetTexture("_Noise", (Texture) this.noise);
    this._material.SetVector("_NoiseScaleOffset", new Vector4((float) Screen.width / (float) this.noise.width, (float) Screen.height / (float) this.noise.height, Random.value, Random.value));
    this._material.SetFloat("_NoiseIntensity", this.noiseIntensity);
    this._material.SetTexture("_Accum", (Texture) this._accumTexture);
    this._material.SetFloat("_AccumBase", this.accumBase);
    this._material.SetFloat("_AccumPowerFactor", this.accumPowerFactor);
    this._material.SetFloat("_AfterimageLogarithmicDecay", this.afterimageLogarithmicDecay);
    this._material.SetFloat("_AfterimageLinearDecay", this.afterimageLinearDecay);
    Graphics.Blit((Texture) source, this._accumTexture, this._material, 5);
    return this._accumTexture;
  }

  private void CreateNewMaterialIfNull()
  {
    if ((bool) (Object) this._material)
      return;
    this._material = HumanEyeTonemapper.CreateMaterial(this.shader);
  }

  private void CreateNewAccumTextureIfInvalid(RenderTexture source)
  {
    if (!((Object) this._accumTexture == (Object) null) && this._accumTexture.width == source.width && this._accumTexture.height == source.height && this._accumTexture.format == source.format)
      return;
    if ((Object) this._accumTexture != (Object) null)
    {
      this._accumTexture.Release();
      Object.DestroyImmediate((Object) this._accumTexture);
    }
    this._accumTexture = new RenderTexture(source.width, source.height, source.depth, source.format);
    Graphics.Blit((Texture) source, this._accumTexture);
  }

  public void ApplyConfiguration()
  {
  }

  private enum Pass
  {
    Blur,
    Composite,
    Luminance,
    ViewAdaptation,
    CompositeNoAdaptation,
    Accumulation,
  }
}
