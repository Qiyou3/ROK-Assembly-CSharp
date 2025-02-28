// Decompiled with JetBrains decompiler
// Type: ColorCorrectionCurves
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ColorCorrectionCurves : PostEffectsBase
{
  public AnimationCurve redChannel;
  public AnimationCurve greenChannel;
  public AnimationCurve blueChannel;
  public bool useDepthCorrection;
  public AnimationCurve zCurve;
  public AnimationCurve depthRedChannel;
  public AnimationCurve depthGreenChannel;
  public AnimationCurve depthBlueChannel;
  private Material ccMaterial;
  private Material ccDepthMaterial;
  private Material selectiveCcMaterial;
  private Texture2D rgbChannelTex;
  private Texture2D rgbDepthChannelTex;
  private Texture2D zCurveTex;
  public bool selectiveCc;
  public Color selectiveFromColor = Color.white;
  public Color selectiveToColor = Color.white;
  public ColorCorrectionMode mode;
  public bool updateTextures = true;
  public Shader colorCorrectionCurvesShader;
  public Shader simpleColorCorrectionCurvesShader;
  public Shader colorCorrectionSelectiveShader;
  private bool updateTexturesOnStartup = true;

  public override void Start() => this.updateTexturesOnStartup = true;

  public void Awake()
  {
  }

  public void OnDisable()
  {
    if ((bool) (Object) this.ccMaterial)
      Object.DestroyImmediate((Object) this.ccMaterial);
    if ((bool) (Object) this.ccDepthMaterial)
      Object.DestroyImmediate((Object) this.ccDepthMaterial);
    if ((bool) (Object) this.selectiveCcMaterial)
      Object.DestroyImmediate((Object) this.selectiveCcMaterial);
    if ((bool) (Object) this.rgbChannelTex)
      Object.DestroyImmediate((Object) this.rgbChannelTex);
    if ((bool) (Object) this.rgbDepthChannelTex)
      Object.DestroyImmediate((Object) this.rgbDepthChannelTex);
    if (!(bool) (Object) this.zCurveTex)
      return;
    Object.DestroyImmediate((Object) this.zCurveTex);
  }

  public override bool CheckResources()
  {
    this.CheckSupport(this.mode == ColorCorrectionMode.Advanced);
    this.ccMaterial = this.CheckShaderAndCreateMaterial(this.simpleColorCorrectionCurvesShader, this.ccMaterial);
    this.ccDepthMaterial = this.CheckShaderAndCreateMaterial(this.colorCorrectionCurvesShader, this.ccDepthMaterial);
    this.selectiveCcMaterial = this.CheckShaderAndCreateMaterial(this.colorCorrectionSelectiveShader, this.selectiveCcMaterial);
    if (!(bool) (Object) this.rgbChannelTex)
      this.rgbChannelTex = new Texture2D(256, 4, TextureFormat.ARGB32, false, true);
    if (!(bool) (Object) this.rgbDepthChannelTex)
      this.rgbDepthChannelTex = new Texture2D(256, 4, TextureFormat.ARGB32, false, true);
    if (!(bool) (Object) this.zCurveTex)
      this.zCurveTex = new Texture2D(256, 1, TextureFormat.ARGB32, false, true);
    this.rgbChannelTex.hideFlags = HideFlags.DontSave;
    this.rgbDepthChannelTex.hideFlags = HideFlags.DontSave;
    this.zCurveTex.hideFlags = HideFlags.DontSave;
    this.rgbChannelTex.wrapMode = TextureWrapMode.Clamp;
    this.rgbDepthChannelTex.wrapMode = TextureWrapMode.Clamp;
    this.zCurveTex.wrapMode = TextureWrapMode.Clamp;
    if (!this.isSupported)
      this.ReportAutoDisable();
    return this.isSupported;
  }

  public void UpdateParameters()
  {
    if (this.redChannel == null || this.greenChannel == null || this.blueChannel == null)
      return;
    for (float time = 0.0f; (double) time <= 1.0; time += 0.003921569f)
    {
      float num1 = Mathf.Clamp(this.redChannel.Evaluate(time), 0.0f, 1f);
      float num2 = Mathf.Clamp(this.greenChannel.Evaluate(time), 0.0f, 1f);
      float num3 = Mathf.Clamp(this.blueChannel.Evaluate(time), 0.0f, 1f);
      this.rgbChannelTex.SetPixel(Mathf.FloorToInt(time * (float) byte.MaxValue), 0, new Color(num1, num1, num1));
      this.rgbChannelTex.SetPixel(Mathf.FloorToInt(time * (float) byte.MaxValue), 1, new Color(num2, num2, num2));
      this.rgbChannelTex.SetPixel(Mathf.FloorToInt(time * (float) byte.MaxValue), 2, new Color(num3, num3, num3));
      float num4 = Mathf.Clamp(this.zCurve.Evaluate(time), 0.0f, 1f);
      this.zCurveTex.SetPixel(Mathf.FloorToInt(time * (float) byte.MaxValue), 0, new Color(num4, num4, num4));
      float num5 = Mathf.Clamp(this.depthRedChannel.Evaluate(time), 0.0f, 1f);
      float num6 = Mathf.Clamp(this.depthGreenChannel.Evaluate(time), 0.0f, 1f);
      float num7 = Mathf.Clamp(this.depthBlueChannel.Evaluate(time), 0.0f, 1f);
      this.rgbDepthChannelTex.SetPixel(Mathf.FloorToInt(time * (float) byte.MaxValue), 0, new Color(num5, num5, num5));
      this.rgbDepthChannelTex.SetPixel(Mathf.FloorToInt(time * (float) byte.MaxValue), 1, new Color(num6, num6, num6));
      this.rgbDepthChannelTex.SetPixel(Mathf.FloorToInt(time * (float) byte.MaxValue), 2, new Color(num7, num7, num7));
    }
    this.rgbChannelTex.Apply();
    this.rgbDepthChannelTex.Apply();
    this.zCurveTex.Apply();
  }

  public void UpdateTextures() => this.UpdateParameters();

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (!this.CheckResources())
    {
      Graphics.Blit((Texture) source, destination);
    }
    else
    {
      if (this.updateTexturesOnStartup)
      {
        this.UpdateParameters();
        this.updateTexturesOnStartup = false;
      }
      if (this.useDepthCorrection)
        this.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
      RenderTexture renderTexture = destination;
      if (this.selectiveCc)
        renderTexture = RenderTexture.GetTemporary(source.width, source.height);
      if (this.useDepthCorrection)
      {
        this.ccDepthMaterial.SetTexture("_RgbTex", (Texture) this.rgbChannelTex);
        this.ccDepthMaterial.SetTexture("_ZCurve", (Texture) this.zCurveTex);
        this.ccDepthMaterial.SetTexture("_RgbDepthTex", (Texture) this.rgbDepthChannelTex);
        Graphics.Blit((Texture) source, renderTexture, this.ccDepthMaterial);
      }
      else
      {
        this.ccMaterial.SetTexture("_RgbTex", (Texture) this.rgbChannelTex);
        Graphics.Blit((Texture) source, renderTexture, this.ccMaterial);
      }
      if (!this.selectiveCc)
        return;
      this.selectiveCcMaterial.SetColor("selColor", this.selectiveFromColor);
      this.selectiveCcMaterial.SetColor("targetColor", this.selectiveToColor);
      Graphics.Blit((Texture) renderTexture, destination, this.selectiveCcMaterial);
      RenderTexture.ReleaseTemporary(renderTexture);
    }
  }
}
