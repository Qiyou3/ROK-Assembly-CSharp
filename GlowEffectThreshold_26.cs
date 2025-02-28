// Decompiled with JetBrains decompiler
// Type: GlowEffectThreshold_26
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
[AddComponentMenu("Image Effects/GlowThreshold_26")]
public class GlowEffectThreshold_26 : MonoBehaviour
{
  public float glowThreshold = 0.25f;
  public float glowIntensity = 1.5f;
  public int blurIterations = 3;
  public float blurSpread = 0.7f;
  public Color glowTint = new Color(1f, 1f, 1f, 0.0f);
  private static string compositeMatString = "Shader \"GlowCompose\" {\n\tProperties {\n\t\t_Color (\"Glow Amount\", Color) = (1,1,1,1)\n\t\t_MainTex (\"\", RECT) = \"white\" {}\n\t}\n\tSubShader {\n\t\tPass {\n\t\t\tZTest Always Cull Off ZWrite Off Fog { Mode Off }\n\t\t\tBlend One One\n\t\t\tSetTexture [_MainTex] {constantColor [_Color] combine constant * texture DOUBLE}\n\t\t}\n\t}\n\tFallback off\n}";
  private static Material m_CompositeMaterial;
  private static string blurMatString = "Shader \"GlowConeTap\" {\n\tProperties {\n\t\t_Color (\"Blur Boost\", Color) = (0,0,0,0.25)\n\t\t_MainTex (\"\", RECT) = \"white\" {}\n\t}\n\tSubShader {\n\t\tPass {\n\t\t\tZTest Always Cull Off ZWrite Off Fog { Mode Off }\n\t\t\tSetTexture [_MainTex] {constantColor [_Color] combine texture * constant alpha}\n\t\t\tSetTexture [_MainTex] {constantColor [_Color] combine texture * constant + previous}\n\t\t\tSetTexture [_MainTex] {constantColor [_Color] combine texture * constant + previous}\n\t\t\tSetTexture [_MainTex] {constantColor [_Color] combine texture * constant + previous}\n\t\t}\n\t}\n\tFallback off\n}";
  private static Material m_BlurMaterial;
  public Shader downsampleShader;
  private Material m_DownsampleMaterial;

  protected static Material compositeMaterial
  {
    get
    {
      if ((Object) GlowEffectThreshold_26.m_CompositeMaterial == (Object) null)
      {
        GlowEffectThreshold_26.m_CompositeMaterial = new Material(GlowEffectThreshold_26.compositeMatString);
        GlowEffectThreshold_26.m_CompositeMaterial.hideFlags = HideFlags.HideAndDontSave;
        GlowEffectThreshold_26.m_CompositeMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
      }
      return GlowEffectThreshold_26.m_CompositeMaterial;
    }
  }

  protected static Material blurMaterial
  {
    get
    {
      if ((Object) GlowEffectThreshold_26.m_BlurMaterial == (Object) null)
      {
        GlowEffectThreshold_26.m_BlurMaterial = new Material(GlowEffectThreshold_26.blurMatString);
        GlowEffectThreshold_26.m_BlurMaterial.hideFlags = HideFlags.HideAndDontSave;
        GlowEffectThreshold_26.m_BlurMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
      }
      return GlowEffectThreshold_26.m_BlurMaterial;
    }
  }

  protected Material downsampleMaterial
  {
    get
    {
      if ((Object) this.m_DownsampleMaterial == (Object) null)
      {
        this.m_DownsampleMaterial = new Material(this.downsampleShader);
        this.m_DownsampleMaterial.hideFlags = HideFlags.HideAndDontSave;
      }
      return this.m_DownsampleMaterial;
    }
  }

  public void OnDisable()
  {
    if ((bool) (Object) GlowEffectThreshold_26.m_CompositeMaterial)
    {
      Object.DestroyImmediate((Object) GlowEffectThreshold_26.m_CompositeMaterial.shader);
      Object.DestroyImmediate((Object) GlowEffectThreshold_26.m_CompositeMaterial);
    }
    if ((bool) (Object) GlowEffectThreshold_26.m_BlurMaterial)
    {
      Object.DestroyImmediate((Object) GlowEffectThreshold_26.m_BlurMaterial.shader);
      Object.DestroyImmediate((Object) GlowEffectThreshold_26.m_BlurMaterial);
    }
    if (!(bool) (Object) this.m_DownsampleMaterial)
      return;
    Object.DestroyImmediate((Object) this.m_DownsampleMaterial);
  }

  public void Start()
  {
    if (!SystemInfo.supportsImageEffects)
      this.enabled = false;
    else if ((Object) this.downsampleShader == (Object) null)
    {
      this.LogInfo<GlowEffectThreshold_26>("No downsample shader assigned! Disabling glow.");
      this.enabled = false;
    }
    else
    {
      if (!GlowEffectThreshold_26.blurMaterial.shader.isSupported)
        this.enabled = false;
      if (!GlowEffectThreshold_26.compositeMaterial.shader.isSupported)
        this.enabled = false;
      if (this.downsampleMaterial.shader.isSupported)
        return;
      this.enabled = false;
    }
  }

  public void FourTapCone(RenderTexture source, RenderTexture dest, int iteration)
  {
    float num = (float) (0.5 + (double) iteration * (double) this.blurSpread);
    Graphics.BlitMultiTap((Texture) source, dest, GlowEffectThreshold_26.blurMaterial, new Vector2(-num, -num), new Vector2(-num, num), new Vector2(num, num), new Vector2(num, -num));
  }

  private void DownSample4x(RenderTexture source, RenderTexture dest)
  {
    this.downsampleMaterial.SetFloat("_GlowThreshold", this.glowThreshold);
    this.downsampleMaterial.color = new Color(this.glowTint.r, this.glowTint.g, this.glowTint.b, this.glowTint.a / 4f);
    Graphics.Blit((Texture) source, dest, this.downsampleMaterial);
  }

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    this.glowThreshold = Mathf.Clamp(this.glowThreshold, 0.0f, 1f);
    this.glowIntensity = Mathf.Clamp(this.glowIntensity, 0.0f, 10f);
    this.blurIterations = Mathf.Clamp(this.blurIterations, 0, 30);
    this.blurSpread = Mathf.Clamp(this.blurSpread, 0.5f, 1f);
    RenderTexture temporary1 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0);
    RenderTexture temporary2 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0);
    this.DownSample4x(source, temporary1);
    GlowEffectThreshold_26.blurMaterial.color = new Color(1f, 1f, 1f, 0.25f + Mathf.Clamp01((float) (((double) this.glowIntensity - 1.0) / 4.0)));
    bool flag = true;
    for (int iteration = 0; iteration < this.blurIterations; ++iteration)
    {
      if (flag)
        this.FourTapCone(temporary1, temporary2, iteration);
      else
        this.FourTapCone(temporary2, temporary1, iteration);
      flag = !flag;
    }
    Graphics.Blit((Texture) source, destination);
    if (flag)
      this.BlitGlow(temporary1, destination);
    else
      this.BlitGlow(temporary2, destination);
    RenderTexture.ReleaseTemporary(temporary1);
    RenderTexture.ReleaseTemporary(temporary2);
  }

  public void BlitGlow(RenderTexture source, RenderTexture dest)
  {
    GlowEffectThreshold_26.compositeMaterial.color = new Color(1f, 1f, 1f, Mathf.Clamp01(this.glowIntensity));
    Graphics.Blit((Texture) source, dest, GlowEffectThreshold_26.compositeMaterial);
  }
}
