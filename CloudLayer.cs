// Decompiled with JetBrains decompiler
// Type: CloudLayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using System;
using UnityEngine;

#nullable disable
public class CloudLayer : MonoBehaviour
{
  public bool cloudFog_warn = true;
  public CloudFog m_cloudFog;
  public bool skyDome_warn = true;
  public SkyDome m_skyDome;
  public bool cloudParticles_warn = true;
  public CloudParticles m_cloudParticles;
  public Camera m_camera;
  public CloudLayer.Layer mainLayer;
  public CloudLayer.Layer[] layers;
  public CloudLayer.Quality[] quality;
  public int qualityLevelUsed;
  public bool render = true;
  public float altitude;
  public float thickness;
  public float tileSize;
  public float particleRange;
  public float windMultiplier;
  public float coverage;
  public float densityLog10;
  public float smoothness = 0.5f;
  public float parallaxFactor = 7.5f;
  public float coverage_2;
  public float densityLog10_2;
  public float smoothness_2;
  private string[] _matrixLayerNames;
  private string[] _invRotMatrixLayerNames;
  private string[] _offsetAlphaLayerNames;

  private float AlphaMin => 1f - this.coverage - this.coverage_2;

  private float AlphaRange => 1f / Mathf.Pow(10f, this.densityLog10 + this.densityLog10_2);

  public CloudFog cloudFog
  {
    get
    {
      if ((UnityEngine.Object) this.m_cloudFog == (UnityEngine.Object) null)
      {
        this.m_cloudFog = this.GetComponentInChildren<CloudFog>();
        if ((UnityEngine.Object) this.m_cloudFog == (UnityEngine.Object) null)
        {
          if (this.cloudFog_warn)
          {
            this.cloudFog_warn = false;
            this.LogWarning<CloudLayer>("{0}.m_cloudFog == null", (object) this.gameObject.GetFullName());
          }
        }
        else
          this.cloudFog_warn = true;
      }
      return this.m_cloudFog;
    }
  }

  public SkyDome skyDome
  {
    get
    {
      if ((UnityEngine.Object) this.m_skyDome == (UnityEngine.Object) null)
      {
        this.m_skyDome = this.GetComponentInChildren<SkyDome>();
        if ((UnityEngine.Object) this.m_skyDome == (UnityEngine.Object) null)
        {
          if (this.skyDome_warn)
          {
            this.skyDome_warn = false;
            this.LogWarning<CloudLayer>("{0}.m_skyDome == null", (object) this.gameObject.GetFullName());
          }
        }
        else
          this.skyDome_warn = true;
      }
      return this.m_skyDome;
    }
  }

  public CloudParticles cloudParticles
  {
    get
    {
      if ((UnityEngine.Object) this.m_cloudParticles == (UnityEngine.Object) null)
      {
        this.m_cloudParticles = this.GetComponentInChildren<CloudParticles>();
        if ((UnityEngine.Object) this.m_cloudParticles == (UnityEngine.Object) null)
        {
          if (this.cloudParticles_warn)
          {
            this.cloudParticles_warn = false;
            this.LogWarning<CloudLayer>("{0}.m_cloudParticles == null", (object) this.gameObject.GetFullName());
          }
        }
        else
          this.cloudParticles_warn = true;
      }
      return this.m_cloudParticles;
    }
  }

  public Camera camera
  {
    get
    {
      if ((UnityEngine.Object) this.m_camera == (UnityEngine.Object) null)
      {
        this.m_camera = Camera.main;
        if ((UnityEngine.Object) this.m_camera == (UnityEngine.Object) null)
          this.m_camera = UnityEngine.Object.FindObjectOfType(typeof (Camera)) as Camera;
        if ((UnityEngine.Object) this.m_camera == (UnityEngine.Object) null)
          this.LogWarning<CloudLayer>("m_camera == null");
      }
      return this.m_camera;
    }
  }

  public int qualitySettingsLevel
  {
    get => this.qualityLevelUsed;
    set => this.qualityLevelUsed = 4 - value;
  }

  private void ApplyMaterialValues(Material material, CloudLayer.Quality quality, Shader shader)
  {
    material.shader = shader;
    float num = 1f;
    for (int index = 0; index < quality.layers && index < this.layers.Length; ++index)
    {
      num += this.layers[index].opacity;
      material.SetVector(this._matrixLayerNames[index], this.layers[index].GetMatrix());
      material.SetVector(this._invRotMatrixLayerNames[index], this.layers[index].GetInvRotMatrix());
      material.SetVector(this._offsetAlphaLayerNames[index], this.layers[index].GetOffsetAlpha(this.mainLayer));
    }
    material.SetFloat("_AlphaMin", this.AlphaMin);
    material.SetFloat("_AlphaRange", this.AlphaRange);
    material.SetFloat("_TotalOpacity", num);
  }

  public void Awake()
  {
  }

  public void ApplyConfiguration()
  {
  }

  public void Start()
  {
    this._matrixLayerNames = new string[this.layers.Length];
    this._invRotMatrixLayerNames = new string[this.layers.Length];
    this._offsetAlphaLayerNames = new string[this.layers.Length];
    for (int index = 0; index < this.layers.Length; ++index)
    {
      int num = index + 2;
      this._matrixLayerNames[index] = "_Matrix" + (object) num;
      this._invRotMatrixLayerNames[index] = "_InvRotMatrix" + (object) num;
      this._offsetAlphaLayerNames[index] = "_OffsetAlpha" + (object) num;
    }
    for (int index = 0; index < this.layers.Length; ++index)
      this.layers[index].Init(this);
    this.mainLayer.Init(this);
  }

  public void LateUpdate()
  {
    Camera main = Camera.main;
    if ((UnityEngine.Object) main == (UnityEngine.Object) null)
      return;
    if (!this.render)
    {
      this.gameObject.SetActiveRecursively(false);
      this.gameObject.active = true;
    }
    else
    {
      this.gameObject.SetActiveRecursively(true);
      this.gameObject.active = true;
    }
    for (int index = 0; index < this.layers.Length; ++index)
      this.layers[index].UpdateWindOffset();
    this.mainLayer.UpdateWindOffset();
    this.qualityLevelUsed = Mathf.Clamp(this.qualityLevelUsed, 0, this.quality.Length - 1);
    float num = Mathf.Abs(main.transform.position.y - this.altitude);
    if ((bool) (UnityEngine.Object) this.skyDome)
    {
      this.skyDome.domeAltitude = this.altitude;
      this.skyDome.skyMaterialTileSize = this.tileSize;
      this.skyDome.skyMaterialFadeInDistance = this.particleRange;
      this.skyDome.skyMaterialFadeOutDistance = this.tileSize * 3f;
      this.skyDome.skyMaterialTileOffset = this.mainLayer.offset;
      Renderer[] renderers = this.skyDome.Renderers;
      if ((double) num > (double) this.skyDome.skyMaterialFadeOutDistance && Application.isPlaying)
      {
        this.skyDome.enabled = false;
        for (int index = 0; index < renderers.Length; ++index)
          renderers[index].enabled = false;
      }
      else
      {
        this.skyDome.enabled = true;
        foreach (Material material in this.skyDome.Materials)
        {
          this.ApplyMaterialValues(material, this.quality[this.qualityLevelUsed], this.quality[this.qualityLevelUsed].domeShader);
          material.SetFloat("_Parallax", this.thickness * -this.parallaxFactor / this.tileSize);
          Vector4 vector = material.GetVector("_DitherSizeOffset");
          Vector2 vector2 = UnityEngine.Random.insideUnitCircle.normalized / 2f;
          vector.z = Mathf.Repeat(vector.x + vector2.x, 1f);
          vector.w = Mathf.Repeat(vector.y + vector2.y, 1f);
          material.SetVector("_DitherSizeOffset", vector);
        }
      }
    }
    if ((bool) (UnityEngine.Object) this.cloudFog)
    {
      this.cloudFog.centerAltitude = this.altitude;
      this.cloudFog.thickness = this.thickness / 2f;
      this.cloudFog.skyMaterialTileSize = this.tileSize;
      this.cloudFog.skyMaterialTileOffset = this.mainLayer.offset;
      Renderer[] renderers = this.cloudFog.Renderers;
      if ((double) num > (double) this.thickness && Application.isPlaying)
      {
        this.cloudFog.enabled = false;
        for (int index = 0; index < renderers.Length; ++index)
          renderers[index].enabled = false;
      }
      else
      {
        this.cloudFog.enabled = true;
        for (int index = 0; index < renderers.Length; ++index)
          renderers[index].enabled = true;
        foreach (Material material in this.cloudFog.Materials)
          this.ApplyMaterialValues(material, this.quality[this.qualityLevelUsed], this.quality[this.qualityLevelUsed].fogShader);
      }
    }
    if (!(bool) (UnityEngine.Object) this.cloudParticles)
      return;
    this.cloudParticles.altitude = this.altitude;
    this.cloudParticles.thickness = this.thickness;
    this.cloudParticles.width = this.cloudParticles.thickness;
    this.cloudParticles.range = this.particleRange;
    Renderer[] renderers1 = this.cloudParticles.Renderers;
    if ((double) num > (double) this.cloudParticles.range + (double) this.cloudParticles.thickness && Application.isPlaying)
    {
      this.cloudParticles.enabled = false;
      for (int index = 0; index < renderers1.Length; ++index)
        renderers1[index].enabled = false;
    }
    else
    {
      this.cloudParticles.enabled = true;
      for (int index = 0; index < renderers1.Length; ++index)
        renderers1[index].enabled = true;
      foreach (Material material in this.cloudParticles.Materials)
      {
        this.ApplyMaterialValues(material, this.quality[this.qualityLevelUsed], this.quality[this.qualityLevelUsed].particleShader);
        material.SetFloat("_SamplingScale", 1f / this.tileSize);
        material.SetVector("_SamplingOffset", new Vector4(this.mainLayer.offset.x, this.mainLayer.offset.y, 0.0f, 0.0f));
        material.SetFloat("_Thickness", this.thickness * 2f);
        material.SetFloat("_Altitude", this.altitude);
        material.SetFloat("_RangeFar", this.particleRange);
        material.SetFloat("_AlphaRange", this.AlphaRange / 3f);
      }
    }
  }

  public void SetCloudColor(Color color)
  {
    if ((bool) (UnityEngine.Object) this.skyDome)
      this.skyDome.GetComponent<Renderer>().material.SetColor("_Color", color);
    if (!(bool) (UnityEngine.Object) this.cloudParticles)
      return;
    this.cloudParticles.GetComponent<Renderer>().material.SetColor("_Color", color);
  }

  [Serializable]
  public class Quality
  {
    public int parallaxSteps;
    public int layers;
    public Shader domeShader;
    public Shader fogShader;
    public Shader particleShader;
  }

  [Serializable]
  public class Layer
  {
    public float rotation;
    public float scale;
    public Vector2 offset;
    public float opacity;
    public Vector2 wind;
    public CloudLayer cloudLayer;

    public void Init(CloudLayer _cloudLayer)
    {
      this.cloudLayer = _cloudLayer;
      this.offset.x = UnityEngine.Random.value;
      this.offset.y = UnityEngine.Random.value;
    }

    public Vector4 GetMatrix()
    {
      float f = this.rotation * ((float) Math.PI / 180f);
      float num1 = Mathf.Sin(f) / this.scale;
      float num2 = Mathf.Cos(f) / this.scale;
      Vector4 matrix;
      matrix.x = num2;
      matrix.y = num1;
      matrix.z = -num1;
      matrix.w = num2;
      return matrix;
    }

    public Vector4 GetInvRotMatrix()
    {
      float f = this.rotation * ((float) Math.PI / 180f);
      float num1 = Mathf.Sin(f);
      float num2 = Mathf.Cos(f);
      Vector4 invRotMatrix;
      invRotMatrix.x = num2;
      invRotMatrix.y = -num1;
      invRotMatrix.z = num1;
      invRotMatrix.w = num2;
      return invRotMatrix;
    }

    public Vector4 GetOffsetAlpha(CloudLayer.Layer mainLayer)
    {
      this.opacity = Mathf.Pow(this.scale, this.cloudLayer.smoothness + this.cloudLayer.smoothness_2);
      Vector4 offsetAlpha;
      offsetAlpha.x = this.offset.x - mainLayer.offset.x;
      offsetAlpha.y = this.offset.y - mainLayer.offset.y;
      offsetAlpha.z = 1f;
      offsetAlpha.w = this.opacity;
      return offsetAlpha;
    }

    public void UpdateWindOffset()
    {
      this.offset += this.wind * this.cloudLayer.windMultiplier * Time.deltaTime / this.cloudLayer.tileSize;
    }
  }
}
