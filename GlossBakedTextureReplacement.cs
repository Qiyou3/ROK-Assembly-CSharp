// Decompiled with JetBrains decompiler
// Type: GlossBakedTextureReplacement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[ExecuteInEditMode]
[AddComponentMenu("Relief Terrain/Helpers/Use baked gloss texture")]
public class GlossBakedTextureReplacement : MonoBehaviour
{
  public RTPGlossBaked glossBakedData;
  public bool RTPStandAloneShader;
  public int layerNumber = 1;
  public Material CustomMaterial;
  public Texture2D originalTexture;
  public bool resetGlossMultAndShaping;
  [NonSerialized]
  public Texture2D bakedTexture;

  public GlossBakedTextureReplacement()
  {
    this.bakedTexture = this.originalTexture = (Texture2D) null;
  }

  private void Start() => this.Refresh();

  private void Update()
  {
    if (Application.isPlaying)
      return;
    this.Refresh();
    if (!this.resetGlossMultAndShaping)
      return;
    this.resetGlossMultAndShaping = false;
    this.resetGlossMultAndShapingFun();
  }

  public void resetGlossMultAndShapingFun()
  {
    if ((UnityEngine.Object) this.glossBakedData == (UnityEngine.Object) null)
      return;
    Material material;
    if ((UnityEngine.Object) this.CustomMaterial != (UnityEngine.Object) null)
    {
      material = this.CustomMaterial;
    }
    else
    {
      if (!(bool) (UnityEngine.Object) this.GetComponent<Renderer>())
        return;
      material = this.GetComponent<Renderer>().sharedMaterial;
    }
    if (!(bool) (UnityEngine.Object) material)
      return;
    if (this.RTPStandAloneShader)
    {
      Vector4 vector4_1 = new Vector4(1f, 1f, 1f, 1f);
      Vector4 vector4_2 = new Vector4(0.5f, 0.5f, 0.5f, 0.5f);
      if (material.HasProperty("RTP_gloss_mult0123"))
      {
        Vector4 vector = material.GetVector("RTP_gloss_mult0123");
        if (this.layerNumber >= 1 && this.layerNumber <= 4)
          vector[this.layerNumber - 1] = 1f;
        material.SetVector("RTP_gloss_mult0123", vector);
      }
      if (!material.HasProperty("RTP_gloss_shaping0123"))
        return;
      Vector4 vector1 = material.GetVector("RTP_gloss_shaping0123");
      if (this.layerNumber >= 1 && this.layerNumber <= 4)
        vector1[this.layerNumber - 1] = 0.5f;
      material.SetVector("RTP_gloss_shaping0123", vector1);
    }
    else
    {
      string propertyName1 = "RTP_gloss_mult0";
      string propertyName2 = "RTP_gloss_shaping0";
      if (this.layerNumber == 2)
      {
        propertyName1 = "RTP_gloss_mult1";
        propertyName2 = "RTP_gloss_shaping1";
      }
      if (material.HasProperty(propertyName1))
        material.SetFloat(propertyName1, 1f);
      if (!material.HasProperty(propertyName2))
        return;
      material.SetFloat(propertyName2, 0.5f);
    }
  }

  public void Refresh()
  {
    if ((UnityEngine.Object) this.glossBakedData == (UnityEngine.Object) null)
      return;
    string propertyName = "_MainTex";
    if (this.RTPStandAloneShader)
    {
      propertyName = "_SplatA0";
      if (this.layerNumber == 2)
        propertyName = "_SplatA1";
      else if (this.layerNumber == 3)
        propertyName = "_SplatA2";
      else if (this.layerNumber == 4)
        propertyName = "_SplatA3";
    }
    else if (this.layerNumber == 2)
      propertyName = "_MainTex2";
    Material material;
    if ((UnityEngine.Object) this.CustomMaterial != (UnityEngine.Object) null)
    {
      material = this.CustomMaterial;
    }
    else
    {
      if (!(bool) (UnityEngine.Object) this.GetComponent<Renderer>())
        return;
      material = this.GetComponent<Renderer>().sharedMaterial;
    }
    if (!(bool) (UnityEngine.Object) material || !material.HasProperty(propertyName))
      return;
    if ((bool) (UnityEngine.Object) this.bakedTexture)
    {
      material.SetTexture(propertyName, (Texture) this.bakedTexture);
    }
    else
    {
      if ((UnityEngine.Object) this.originalTexture == (UnityEngine.Object) null)
        this.originalTexture = (Texture2D) material.GetTexture(propertyName);
      if (!((UnityEngine.Object) this.originalTexture != (UnityEngine.Object) null) || !((UnityEngine.Object) this.glossBakedData != (UnityEngine.Object) null) || this.glossBakedData.used_in_atlas || !this.glossBakedData.CheckSize(this.originalTexture))
        return;
      this.bakedTexture = this.glossBakedData.MakeTexture(this.originalTexture);
      if (!(bool) (UnityEngine.Object) this.bakedTexture)
        return;
      material.SetTexture(propertyName, (Texture) this.bakedTexture);
    }
  }
}
