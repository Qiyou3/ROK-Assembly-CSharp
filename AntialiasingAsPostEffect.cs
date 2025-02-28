// Decompiled with JetBrains decompiler
// Type: AntialiasingAsPostEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Configuration;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (Camera))]
[Label("Effects Quality")]
[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Antialiasing (Fullscreen)")]
[ConfigurableByName("enabled", "Anti Aliasing", false, new object[] {false, true, true, true})]
public class AntialiasingAsPostEffect : PostEffectsBase, IConfigurable
{
  public AAMode mode = AAMode.FXAA3Console;
  public bool showGeneratedNormals;
  public float offsetScale = 0.2f;
  public float blurRadius = 18f;
  public float edgeThresholdMin = 0.05f;
  public float edgeThreshold = 0.2f;
  public float edgeSharpness = 4f;
  public bool dlaaSharp;
  public Shader ssaaShader;
  private Material ssaa;
  public Shader dlaaShader;
  private Material dlaa;
  public Shader nfaaShader;
  private Material nfaa;
  public Shader shaderFXAAPreset2;
  private Material materialFXAAPreset2;
  public Shader shaderFXAAPreset3;
  private Material materialFXAAPreset3;
  public Shader shaderFXAAII;
  private Material materialFXAAII;
  public Shader shaderFXAAIII;
  private Material materialFXAAIII;

  public Material CurrentAAMaterial()
  {
    switch (this.mode)
    {
      case AAMode.FXAA2:
        return this.materialFXAAII;
      case AAMode.FXAA3Console:
        return this.materialFXAAIII;
      case AAMode.FXAA1PresetA:
        return this.materialFXAAPreset2;
      case AAMode.FXAA1PresetB:
        return this.materialFXAAPreset3;
      case AAMode.NFAA:
        return this.nfaa;
      case AAMode.SSAA:
        return this.ssaa;
      case AAMode.DLAA:
        return this.dlaa;
      default:
        return (Material) null;
    }
  }

  public override bool CheckResources()
  {
    this.CheckSupport(false);
    this.materialFXAAPreset2 = this.CreateMaterial(this.shaderFXAAPreset2, this.materialFXAAPreset2);
    this.materialFXAAPreset3 = this.CreateMaterial(this.shaderFXAAPreset3, this.materialFXAAPreset3);
    this.materialFXAAII = this.CreateMaterial(this.shaderFXAAII, this.materialFXAAII);
    this.materialFXAAIII = this.CreateMaterial(this.shaderFXAAIII, this.materialFXAAIII);
    this.nfaa = this.CreateMaterial(this.nfaaShader, this.nfaa);
    this.ssaa = this.CreateMaterial(this.ssaaShader, this.ssaa);
    this.dlaa = this.CreateMaterial(this.dlaaShader, this.dlaa);
    if (!this.ssaaShader.isSupported)
    {
      this.NotSupported();
      this.ReportAutoDisable();
    }
    return this.isSupported;
  }

  public void Awake() => this.InitializeConfigurable();

  public void ApplyConfiguration()
  {
  }

  public void OnDisable()
  {
    if (!Application.isPlaying)
      return;
    if ((bool) (Object) this.materialFXAAPreset2)
      Object.Destroy((Object) this.materialFXAAPreset2);
    if ((bool) (Object) this.materialFXAAPreset3)
      Object.Destroy((Object) this.materialFXAAPreset3);
    if ((bool) (Object) this.materialFXAAII)
      Object.Destroy((Object) this.materialFXAAII);
    if ((bool) (Object) this.materialFXAAIII)
      Object.Destroy((Object) this.materialFXAAIII);
    if ((bool) (Object) this.nfaa)
      Object.Destroy((Object) this.nfaa);
    if ((bool) (Object) this.ssaa)
      Object.Destroy((Object) this.ssaa);
    if (!(bool) (Object) this.dlaa)
      return;
    Object.Destroy((Object) this.dlaa);
  }

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (!this.CheckResources())
      Graphics.Blit((Texture) source, destination);
    else if (this.mode == AAMode.FXAA3Console && (Object) this.materialFXAAIII != (Object) null)
    {
      this.materialFXAAIII.SetFloat("_EdgeThresholdMin", this.edgeThresholdMin);
      this.materialFXAAIII.SetFloat("_EdgeThreshold", this.edgeThreshold);
      this.materialFXAAIII.SetFloat("_EdgeSharpness", this.edgeSharpness);
      Graphics.Blit((Texture) source, destination, this.materialFXAAIII);
    }
    else if (this.mode == AAMode.FXAA1PresetB && (Object) this.materialFXAAPreset3 != (Object) null)
      Graphics.Blit((Texture) source, destination, this.materialFXAAPreset3);
    else if (this.mode == AAMode.FXAA1PresetA && (Object) this.materialFXAAPreset2 != (Object) null)
    {
      source.anisoLevel = 4;
      Graphics.Blit((Texture) source, destination, this.materialFXAAPreset2);
      source.anisoLevel = 0;
    }
    else if (this.mode == AAMode.FXAA2 && (Object) this.materialFXAAII != (Object) null)
      Graphics.Blit((Texture) source, destination, this.materialFXAAII);
    else if (this.mode == AAMode.SSAA && (Object) this.ssaa != (Object) null)
      Graphics.Blit((Texture) source, destination, this.ssaa);
    else if (this.mode == AAMode.DLAA && (Object) this.dlaa != (Object) null)
    {
      source.anisoLevel = 0;
      RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height);
      Graphics.Blit((Texture) source, temporary, this.dlaa, 0);
      Graphics.Blit((Texture) temporary, destination, this.dlaa, !this.dlaaSharp ? 1 : 2);
      RenderTexture.ReleaseTemporary(temporary);
    }
    else if (this.mode == AAMode.NFAA && (Object) this.nfaa != (Object) null)
    {
      source.anisoLevel = 0;
      this.nfaa.SetFloat("_OffsetScale", this.offsetScale);
      this.nfaa.SetFloat("_BlurRadius", this.blurRadius);
      Graphics.Blit((Texture) source, destination, this.nfaa, !this.showGeneratedNormals ? 0 : 1);
    }
    else
      Graphics.Blit((Texture) source, destination);
  }
}
