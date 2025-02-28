// Decompiled with JetBrains decompiler
// Type: EdgeDetectEffectNormals
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
[AddComponentMenu("Image Effects/Edge Detection (Geometry)")]
public class EdgeDetectEffectNormals : PostEffectsBase
{
  public EdgeDetectMode mode;
  public float sensitivityDepth = 1f;
  public float sensitivityNormals = 1f;
  public float edgesOnly;
  public Color edgesOnlyBgColor = Color.white;
  public Shader edgeDetectShader;
  private Material edgeDetectMaterial;

  public void OnDisable()
  {
    if (!(bool) (Object) this.edgeDetectMaterial)
      return;
    Object.DestroyImmediate((Object) this.edgeDetectMaterial);
  }

  public override bool CheckResources()
  {
    this.CheckSupport(true);
    this.edgeDetectMaterial = this.CheckShaderAndCreateMaterial(this.edgeDetectShader, this.edgeDetectMaterial);
    if (!this.isSupported)
      this.ReportAutoDisable();
    return this.isSupported;
  }

  [ImageEffectOpaque]
  public void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (!this.CheckResources())
    {
      Graphics.Blit((Texture) source, destination);
    }
    else
    {
      Vector2 vector2 = new Vector2(this.sensitivityDepth, this.sensitivityNormals);
      source.filterMode = FilterMode.Point;
      this.edgeDetectMaterial.SetVector("sensitivity", new Vector4(vector2.x, vector2.y, 1f, vector2.y));
      this.edgeDetectMaterial.SetFloat("_BgFade", this.edgesOnly);
      this.edgeDetectMaterial.SetVector("_BgColor", (Vector4) this.edgesOnlyBgColor);
      if (this.mode == EdgeDetectMode.Thin)
        Graphics.Blit((Texture) source, destination, this.edgeDetectMaterial, 0);
      else
        Graphics.Blit((Texture) source, destination, this.edgeDetectMaterial, 1);
    }
  }
}
