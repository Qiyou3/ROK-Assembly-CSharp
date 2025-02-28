// Decompiled with JetBrains decompiler
// Type: Fisheye
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[AddComponentMenu("Image Effects/Fisheye")]
[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
public class Fisheye : PostEffectsBase
{
  public float strengthX = 0.05f;
  public float strengthY = 0.05f;
  public Shader fishEyeShader;
  private Material fisheyeMaterial;

  public void OnDisable()
  {
    if (!(bool) (Object) this.fisheyeMaterial)
      return;
    Object.DestroyImmediate((Object) this.fisheyeMaterial);
  }

  public override bool CheckResources()
  {
    this.CheckSupport(false);
    this.fisheyeMaterial = this.CheckShaderAndCreateMaterial(this.fishEyeShader, this.fisheyeMaterial);
    if (!this.isSupported)
      this.ReportAutoDisable();
    return this.isSupported;
  }

  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (!this.CheckResources())
    {
      Graphics.Blit((Texture) source, destination);
    }
    else
    {
      float num1 = 5f / 32f;
      float num2 = (float) ((double) source.width * 1.0 / ((double) source.height * 1.0));
      this.fisheyeMaterial.SetVector("intensity", new Vector4(this.strengthX * num2 * num1, this.strengthY * num1, this.strengthX * num2 * num1, this.strengthY * num1));
      Graphics.Blit((Texture) source, destination, this.fisheyeMaterial);
    }
  }
}
