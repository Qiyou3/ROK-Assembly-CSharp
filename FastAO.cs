// Decompiled with JetBrains decompiler
// Type: FastAO
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Configuration;
using System;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (Camera))]
[ExecuteInEditMode]
[Label("Effects Quality")]
[AddComponentMenu("Image Effects/Custom/Fast Ambient Occlusion")]
public class FastAO : MonoBehaviour, IConfigurable
{
  public static FastAO Instance;
  public float maxDistance = 2000f;
  public float maxRadius = 0.25f;
  public float minOcclusionRadius = 0.05f;
  public float minRadiusInPixels = 1f;
  public float radiusForOcclusionDelta = 4f;
  public float radiusForBlurDelta = 8f;
  public int iterations = 7;
  public float power = 1f;
  [QualityLevels(new object[] {FastAO.Quality.Off, FastAO.Quality.Low, FastAO.Quality.Low, FastAO.Quality.High})]
  [DefaultValue(FastAO.Quality.Low)]
  [Label("Ambient Occlusion")]
  [Configurable]
  public FastAO.Quality quality;
  public float downSampleRate = 200f;
  public float sharpness = -0.25f;
  public Texture2D debugTexture;
  public Shader faoShader;
  private Material _faoMaterial;
  private RenderTexture _accumShadowBuffer;
  private RenderTexture _depthBuffer;
  private double _aspectRatio;
  public bool debug;

  public void Awake()
  {
    FastAO.Instance = this;
    this.InitializeConfigurable();
  }

  public void ApplyConfiguration()
  {
    switch (this.quality)
    {
      case FastAO.Quality.Off:
        this.enabled = false;
        break;
      case FastAO.Quality.Low:
        this.enabled = true;
        this.iterations = 3;
        this.maxRadius = 0.06f;
        this.downSampleRate = 3f;
        this.power = 4f;
        break;
      case FastAO.Quality.High:
        this.enabled = true;
        this.iterations = 5;
        this.maxRadius = 0.1f;
        this.downSampleRate = 5f;
        this.power = 6f;
        break;
    }
  }

  public void OnEnable() => this.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;

  private static Material CreateMaterial(Shader shader)
  {
    if (!(bool) (UnityEngine.Object) shader)
      return (Material) null;
    Material material = new Material(shader);
    material.hideFlags = HideFlags.HideAndDontSave;
    return material;
  }

  public void OnDisable()
  {
    if (!((UnityEngine.Object) this._faoMaterial != (UnityEngine.Object) null))
      return;
    UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this._faoMaterial);
    this._faoMaterial = (Material) null;
  }

  public void Start()
  {
    if (!SystemInfo.supportsImageEffects)
    {
      this.LogInfo<FastAO>("Could not use FastAO because: !SystemInfo.supportsImageEffects");
      this.enabled = false;
    }
    else if (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
    {
      this.LogInfo<FastAO>("Could not use FastAO because: !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth)");
      this.enabled = false;
    }
    else if (!this.faoShader.isSupported)
    {
      this.LogInfo<FastAO>("Could not use FastAO because: !faoShader.isSupported");
      this.enabled = false;
    }
    else
    {
      if (!(bool) (UnityEngine.Object) this._faoMaterial)
        this._faoMaterial = FastAO.CreateMaterial(this.faoShader);
      if (!((UnityEngine.Object) this._faoMaterial == (UnityEngine.Object) null))
        return;
      this.LogInfo<FastAO>("Could not use FastAO because: _faoMaterial == null");
      this.enabled = false;
    }
  }

  [ImageEffectOpaque]
  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (!(bool) (UnityEngine.Object) this._faoMaterial)
      this._faoMaterial = FastAO.CreateMaterial(this.faoShader);
    this._depthBuffer = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.RFloat);
    this._depthBuffer.filterMode = FilterMode.Point;
    Graphics.Blit((Texture) null, this._depthBuffer, this._faoMaterial, 0);
    this._accumShadowBuffer = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.RFloat);
    this._accumShadowBuffer.filterMode = FilterMode.Point;
    Graphics.Blit((Texture) null, this._accumShadowBuffer, this._faoMaterial, 1);
    this.IterativeAccumBlur();
    this._faoMaterial.SetTexture("_Accum", (Texture) this._accumShadowBuffer);
    this._faoMaterial.SetFloat("_Power", this.power);
    this._faoMaterial.SetFloat("_MaxDistance", this.maxDistance);
    Graphics.Blit((Texture) source, destination, this._faoMaterial, !this.debug ? 3 : 4);
    RenderTexture.ReleaseTemporary(this._depthBuffer);
    RenderTexture.ReleaseTemporary(this._accumShadowBuffer);
  }

  private void IterativeAccumBlur()
  {
    this._aspectRatio = (double) Screen.height / (double) Screen.width;
    float p = 1f / (float) (this.iterations - 1);
    float num1 = Mathf.Pow(this.minRadiusInPixels / (float) Screen.height, p) / Mathf.Pow(this.maxRadius, p);
    this._faoMaterial.SetFloat("_Sharpness", Mathf.Pow(10f, this.sharpness));
    this._faoMaterial.SetFloat("_OuterOccDelta", this.radiusForOcclusionDelta * this.maxRadius);
    float num2 = 1f;
    float f = Mathf.Pow(this.downSampleRate, (float) (1 / (this.iterations - 1)));
    int num3 = 0;
    float maxRadius = this.maxRadius;
    double angle = 0.0;
    float num4 = num2 * this.downSampleRate;
    while (num3 < this.iterations)
    {
      this._faoMaterial.SetFloat("_Radius", maxRadius);
      this._faoMaterial.SetFloat("_InnerOccDelta", this.radiusForOcclusionDelta * Mathf.Max(this.minOcclusionRadius, maxRadius));
      this._faoMaterial.SetFloat("_BlurDelta", this.radiusForBlurDelta * maxRadius);
      this.AssignSampleCoordinates(angle);
      this._faoMaterial.SetTexture("_Accum", (Texture) this._accumShadowBuffer);
      RenderTexture accumShadowBuffer = this._accumShadowBuffer;
      this._accumShadowBuffer = RenderTexture.GetTemporary((int) ((double) Screen.width / (double) num4), (int) ((double) Screen.height / (double) num4), 0, RenderTextureFormat.RFloat);
      RenderTexture.ReleaseTemporary(accumShadowBuffer);
      Graphics.Blit((Texture) this._depthBuffer, this._accumShadowBuffer, this._faoMaterial, 2);
      ++num3;
      maxRadius *= num1;
      angle += 0.29999540371611322;
      num4 = Mathf.Pow(f, (float) (this.iterations - 1 - num3));
    }
  }

  private void AssignSampleCoordinates(double angle)
  {
    Vector4 vector1;
    vector1.x = (float) (Math.Cos(angle) * this._aspectRatio);
    vector1.y = (float) Math.Sin(angle);
    vector1.z = -vector1.x;
    vector1.w = -vector1.y;
    this._faoMaterial.SetVector("_Sample1", vector1);
    double num = angle + Math.PI / 4.0;
    Vector4 vector2;
    vector2.x = (float) (Math.Cos(num) * this._aspectRatio) / 2f;
    vector2.y = (float) Math.Sin(num) / 2f;
    vector2.z = -vector2.x;
    vector2.w = -vector2.y;
    this._faoMaterial.SetVector("_Sample2", vector2);
  }

  private enum Pass
  {
    DepthCopy,
    Zero,
    BlurAccum,
    Composite,
    Debug,
  }

  public enum Quality
  {
    Off,
    Low,
    High,
  }
}
