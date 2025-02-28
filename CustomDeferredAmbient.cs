// Decompiled with JetBrains decompiler
// Type: CustomDeferredAmbient
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Rendering;

#nullable disable
[ExecuteInEditMode]
public class CustomDeferredAmbient : GlobalCommandBufferImplementor
{
  public static CustomDeferredAmbient Instance;
  public Shader Shader;
  [Range(0.0f, 1f)]
  public float AmbientIntensity = 1f;
  [Range(0.0f, 1f)]
  public float ReflectionIntensity = 1f;
  public SphericalHarmonicsL2 ambientProbe;
  private Material _Material;

  public Material Material => this._Material;

  public override void OnEnable()
  {
    base.OnEnable();
    CustomDeferredAmbient.Instance = this;
    if ((Object) this._Material != (Object) null)
      Object.DestroyImmediate((Object) this._Material);
    if ((Object) this.Shader == (Object) null)
      this.Shader = Shader.Find("Custom/CustomDeferredAmbient");
    this._Material = new Material(this.Shader);
    this.UpdateCommandBufferImplementation = new GlobalCommandBufferImplementor.UpdateCommandBufferDelegate(this.UpdateCommandBuffer);
    this.RenderOrder = CameraEvent.AfterFinalPass;
  }

  public override void OnDisable()
  {
    base.OnDisable();
    CustomDeferredAmbient.Instance = (CustomDeferredAmbient) null;
    Object.DestroyImmediate((Object) this._Material);
  }

  public override void OnWillRenderObject()
  {
    if ((Object) this == (Object) null || !this.enabled)
      return;
    this.SphericalHarmonicL2ToMaterial(this.ambientProbe);
    this._Material.SetFloat("AmbientIntensity", this.AmbientIntensity);
    this._Material.SetFloat("ReflectionIntensity", this.ReflectionIntensity);
    base.OnWillRenderObject();
  }

  public override void OnRenderObject()
  {
    if ((Object) this == (Object) null || !this.enabled)
      return;
    base.OnRenderObject();
  }

  public void SphericalHarmonicL2ToMaterial(SphericalHarmonicsL2 fLight)
  {
    Vector4[] vector4Array = new Vector4[3];
    float num1 = Mathf.Sqrt(3.14159274f);
    float num2 = (float) (1.0 / (2.0 * (double) num1));
    float num3 = Mathf.Sqrt(3f) / (3f * num1);
    float num4 = Mathf.Sqrt(15f) / (8f * num1);
    float num5 = Mathf.Sqrt(5f) / (16f * num1);
    float num6 = 0.5f * num4;
    for (int rgb = 0; rgb < 3; ++rgb)
    {
      vector4Array[rgb].x = -num3 * fLight[rgb, 3];
      vector4Array[rgb].y = -num3 * fLight[rgb, 1];
      vector4Array[rgb].z = num3 * fLight[rgb, 2];
      vector4Array[rgb].w = (float) ((double) num2 * (double) fLight[rgb, 0] - (double) num5 * (double) fLight[rgb, 6]);
    }
    this._Material.SetVector("SHAr", vector4Array[0]);
    this._Material.SetVector("SHAg", vector4Array[1]);
    this._Material.SetVector("SHAb", vector4Array[2]);
    for (int rgb = 0; rgb < 3; ++rgb)
    {
      vector4Array[rgb].x = num4 * fLight[rgb, 4];
      vector4Array[rgb].y = -num4 * fLight[rgb, 5];
      vector4Array[rgb].z = 3f * num5 * fLight[rgb, 6];
      vector4Array[rgb].w = -num4 * fLight[rgb, 7];
    }
    this._Material.SetVector("SHBr", vector4Array[0]);
    this._Material.SetVector("SHBg", vector4Array[1]);
    this._Material.SetVector("SHBb", vector4Array[2]);
    vector4Array[0].x = num6 * fLight[0, 8];
    vector4Array[0].y = num6 * fLight[1, 8];
    vector4Array[0].z = num6 * fLight[2, 8];
    vector4Array[0].w = 1f;
    this._Material.SetVector("SHC", vector4Array[0]);
  }

  public void UpdateCommandBuffer(CommandBuffer _CommandBuffer)
  {
    _CommandBuffer.Clear();
    _CommandBuffer.Blit((RenderTargetIdentifier) BuiltinRenderTextureType.GBuffer0, (RenderTargetIdentifier) BuiltinRenderTextureType.CurrentActive, this.Material);
  }
}
