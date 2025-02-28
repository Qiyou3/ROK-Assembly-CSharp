// Decompiled with JetBrains decompiler
// Type: DepthOfFieldScatter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DepthOfFieldScatter : PostEffectsBase
{
  public bool visualizeFocus;
  public float focalLength = 10f;
  public float focalSize = 0.05f;
  public float aperture = 10f;
  public Transform focalTransform;
  public float maxBlurSize = 2f;
  public DepthOfFieldScatter.BlurQuality blurQuality = DepthOfFieldScatter.BlurQuality.Medium;
  public DepthOfFieldScatter.BlurResolution blurResolution = DepthOfFieldScatter.BlurResolution.Low;
  public bool foregroundBlur;
  public float foregroundOverlap = 0.55f;
  public Shader dofHdrShader;
  private float focalDistance01 = 10f;
  private Material dofHdrMaterial;

  public void Awake()
  {
  }

  public void ApplyConfiguration()
  {
  }

  public override bool CheckResources()
  {
    this.CheckSupport(true);
    this.dofHdrMaterial = this.CheckShaderAndCreateMaterial(this.dofHdrShader, this.dofHdrMaterial);
    if (!this.isSupported)
      this.ReportAutoDisable();
    return this.isSupported;
  }

  public float FocalDistance01(float worldDist)
  {
    return this.GetComponent<Camera>().WorldToViewportPoint((worldDist - this.GetComponent<Camera>().nearClipPlane) * this.GetComponent<Camera>().transform.forward + this.GetComponent<Camera>().transform.position).z / (this.GetComponent<Camera>().farClipPlane - this.GetComponent<Camera>().nearClipPlane);
  }

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (!this.CheckResources())
    {
      Graphics.Blit((Texture) source, destination);
    }
    else
    {
      float maxBlurSize = this.maxBlurSize;
      int num1 = this.blurResolution != DepthOfFieldScatter.BlurResolution.High ? 2 : 1;
      if ((double) this.aperture < 0.0)
        this.aperture = 0.0f;
      if ((double) this.maxBlurSize < 0.0)
        this.maxBlurSize = 0.0f;
      this.focalSize = Mathf.Clamp(this.focalSize, 0.0f, 0.3f);
      this.focalDistance01 = !(bool) (Object) this.focalTransform ? this.FocalDistance01(this.focalLength) : this.GetComponent<Camera>().WorldToViewportPoint(this.focalTransform.position).z / this.GetComponent<Camera>().farClipPlane;
      RenderTexture temporary1 = num1 <= 1 ? (RenderTexture) null : RenderTexture.GetTemporary(source.width / num1, source.height / num1, 0, source.format);
      if ((bool) (Object) temporary1)
        temporary1.filterMode = FilterMode.Bilinear;
      RenderTexture temporary2 = RenderTexture.GetTemporary(source.width / (2 * num1), source.height / (2 * num1), 0, source.format);
      RenderTexture temporary3 = RenderTexture.GetTemporary(source.width / (2 * num1), source.height / (2 * num1), 0, source.format);
      if ((bool) (Object) temporary2)
        temporary2.filterMode = FilterMode.Bilinear;
      if ((bool) (Object) temporary3)
        temporary3.filterMode = FilterMode.Bilinear;
      this.dofHdrMaterial.SetVector("_CurveParams", new Vector4(0.0f, this.focalSize, this.aperture / 10f, this.focalDistance01));
      if (this.foregroundBlur)
      {
        RenderTexture temporary4 = RenderTexture.GetTemporary(source.width / (2 * num1), source.height / (2 * num1), 0, source.format);
        Graphics.Blit((Texture) source, temporary3, this.dofHdrMaterial, 4);
        this.dofHdrMaterial.SetTexture("_FgOverlap", (Texture) temporary3);
        float num2 = (float) ((double) maxBlurSize * (double) this.foregroundOverlap * 0.22499999403953552);
        this.dofHdrMaterial.SetVector("_Offsets", new Vector4(0.0f, num2, 0.0f, num2));
        Graphics.Blit((Texture) temporary3, temporary4, this.dofHdrMaterial, 2);
        this.dofHdrMaterial.SetVector("_Offsets", new Vector4(num2, 0.0f, 0.0f, num2));
        Graphics.Blit((Texture) temporary4, temporary2, this.dofHdrMaterial, 2);
        this.dofHdrMaterial.SetTexture("_FgOverlap", (Texture) null);
        Graphics.Blit((Texture) temporary2, source, this.dofHdrMaterial, 7);
        RenderTexture.ReleaseTemporary(temporary4);
      }
      else
        this.dofHdrMaterial.SetTexture("_FgOverlap", (Texture) null);
      Graphics.Blit((Texture) source, source, this.dofHdrMaterial, !this.foregroundBlur ? 0 : 3);
      RenderTexture renderTexture = source;
      if (num1 > 1)
      {
        Graphics.Blit((Texture) source, temporary1, this.dofHdrMaterial, 6);
        renderTexture = temporary1;
      }
      Graphics.Blit((Texture) renderTexture, temporary3, this.dofHdrMaterial, 6);
      Graphics.Blit((Texture) temporary3, renderTexture, this.dofHdrMaterial, 8);
      int pass = 10;
      switch (this.blurQuality)
      {
        case DepthOfFieldScatter.BlurQuality.Low:
          pass = num1 <= 1 ? 10 : 13;
          break;
        case DepthOfFieldScatter.BlurQuality.Medium:
          pass = num1 <= 1 ? 11 : 12;
          break;
        case DepthOfFieldScatter.BlurQuality.High:
          pass = num1 <= 1 ? 14 : 15;
          break;
        default:
          this.LogInfo<DepthOfFieldScatter>("DOF couldn't find valid blur quality setting", (object) this.transform);
          break;
      }
      if (this.visualizeFocus)
      {
        Graphics.Blit((Texture) source, destination, this.dofHdrMaterial, 1);
      }
      else
      {
        this.dofHdrMaterial.SetVector("_Offsets", new Vector4(0.0f, 0.0f, 0.0f, maxBlurSize));
        this.dofHdrMaterial.SetTexture("_LowRez", (Texture) renderTexture);
        Graphics.Blit((Texture) source, destination, this.dofHdrMaterial, pass);
      }
      if ((bool) (Object) temporary2)
        RenderTexture.ReleaseTemporary(temporary2);
      if ((bool) (Object) temporary3)
        RenderTexture.ReleaseTemporary(temporary3);
      if (!(bool) (Object) temporary1)
        return;
      RenderTexture.ReleaseTemporary(temporary1);
    }
  }

  public enum BlurQuality
  {
    Low,
    Medium,
    High,
  }

  public enum BlurResolution
  {
    High,
    Low,
  }
}
