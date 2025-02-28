﻿// Decompiled with JetBrains decompiler
// Type: BlurEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("Image Effects/Blur")]
[ExecuteInEditMode]
public class BlurEffect : MonoBehaviour
{
  public int iterations = 3;
  public float blurSpread = 0.6f;
  public Shader blurShader;
  private static Material m_Material;

  protected Material material
  {
    get
    {
      if ((Object) BlurEffect.m_Material == (Object) null)
      {
        BlurEffect.m_Material = new Material(this.blurShader);
        BlurEffect.m_Material.hideFlags = HideFlags.DontSave;
      }
      return BlurEffect.m_Material;
    }
  }

  public void OnDisable()
  {
    if (!(bool) (Object) BlurEffect.m_Material)
      return;
    Object.DestroyImmediate((Object) BlurEffect.m_Material);
  }

  public void Start()
  {
    if (!SystemInfo.supportsImageEffects)
    {
      this.enabled = false;
    }
    else
    {
      if ((bool) (Object) this.blurShader && this.material.shader.isSupported)
        return;
      this.enabled = false;
    }
  }

  public void FourTapCone(RenderTexture source, RenderTexture dest, int iteration)
  {
    float num = (float) (0.5 + (double) iteration * (double) this.blurSpread);
    Graphics.BlitMultiTap((Texture) source, dest, this.material, new Vector2(-num, -num), new Vector2(-num, num), new Vector2(num, num), new Vector2(num, -num));
  }

  private void DownSample4x(RenderTexture source, RenderTexture dest)
  {
    float num = 1f;
    Graphics.BlitMultiTap((Texture) source, dest, this.material, new Vector2(-num, -num), new Vector2(-num, num), new Vector2(num, num), new Vector2(num, -num));
  }

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    RenderTexture temporary1 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0, source.format);
    RenderTexture temporary2 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0, source.format);
    this.DownSample4x(source, temporary1);
    bool flag = true;
    for (int iteration = 0; iteration < this.iterations; ++iteration)
    {
      if (flag)
        this.FourTapCone(temporary1, temporary2, iteration);
      else
        this.FourTapCone(temporary2, temporary1, iteration);
      flag = !flag;
    }
    if (flag)
      Graphics.Blit((Texture) temporary1, destination);
    else
      Graphics.Blit((Texture) temporary2, destination);
    RenderTexture.ReleaseTemporary(temporary1);
    RenderTexture.ReleaseTemporary(temporary2);
  }

  public RenderTexture GetBlurred(RenderTexture source)
  {
    RenderTexture temporary1 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0, source.format);
    RenderTexture temporary2 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0, source.format);
    this.DownSample4x(source, temporary1);
    bool flag = true;
    for (int iteration = 0; iteration < this.iterations; ++iteration)
    {
      if (flag)
        this.FourTapCone(temporary1, temporary2, iteration);
      else
        this.FourTapCone(temporary2, temporary1, iteration);
      flag = !flag;
    }
    if (flag)
    {
      RenderTexture.ReleaseTemporary(temporary2);
      return temporary2;
    }
    RenderTexture.ReleaseTemporary(temporary1);
    return temporary1;
  }
}
