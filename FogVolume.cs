// Decompiled with JetBrains decompiler
// Type: FogVolume
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using SmartAssembly.Attributes;
using System;
using UnityEngine;

#nullable disable
[DoNotObfuscate]
[DoNotObfuscateType]
[ExecuteInEditMode]
public class FogVolume : MonoBehaviour
{
  public bool EnableInscattering;
  public bool EnableNoise;
  public Color FogColor = new Color(0.5f, 0.6f, 0.7f, 1f);
  public Color DeepColor = new Color(0.5f, 0.6f, 0.7f, 1f);
  public bool UseDeepColor;
  [HideInInspector]
  public Material FogMaterial;
  private GameObject FogVolumeGameObject;
  public bool HideWireframe = true;
  public float InscateringExponent = 15f;
  public Color InscatteringColor = Color.white;
  public float InscatteringIntensity = 2f;
  public float InscatteringStartDistance = 400f;
  public float InscatteringTransitionWideness = 1f;
  public float TransitionHeight;
  [Range(0.0f, 1f)]
  public float NoiseContrast;
  [Range(0.0f, 10f)]
  public float NoiseIntensity = 1f;
  [Range(1f, 3f)]
  public int Quality = 1;
  public int QueuePosition;
  public Vector4 Speed = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
  public Vector4 Stretch = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
  public Light Sun;
  public Vector3 volumeSize;
  public float Visibility = 5f;
  public float _3DNoiseScale = 300f;
  public float _3DNoiseStepSize = 50f;
  public Texture3D NoiseVolume;
  public Shader shader;

  public float GetVisibility() => this.Visibility;

  public void OnEnable()
  {
    if (!(bool) (UnityEngine.Object) this.FogMaterial)
    {
      this.FogMaterial = new Material(this.shader);
      this.FogMaterial.name = "Fog Material";
      this.FogMaterial.hideFlags = HideFlags.HideAndDontSave;
    }
    this.FogVolumeGameObject = this.gameObject;
    this.FogVolumeGameObject.GetComponent<Renderer>().sharedMaterial = this.FogMaterial;
    this.ToggleKeywords();
    if (!((UnityEngine.Object) Camera.main != (UnityEngine.Object) null))
      return;
    Camera.main.depthTextureMode |= DepthTextureMode.Depth;
  }

  public static void Wireframe(GameObject obj, bool enable)
  {
  }

  public void Update()
  {
  }

  public void OnWillRenderObject()
  {
    this.ToggleKeywords();
    this.FogMaterial.SetColor("_Color", this.FogColor);
    this.FogMaterial.SetColor("_DeepColor", !this.UseDeepColor ? this.FogColor : this.DeepColor);
    this.FogMaterial.SetColor("_InscatteringColor", this.InscatteringColor);
    this.FogMaterial.SetFloat("_TransitionHeight", this.TransitionHeight);
    if ((bool) (UnityEngine.Object) this.Sun)
    {
      this.FogMaterial.SetFloat("_InscatteringIntensity", this.InscatteringIntensity);
      this.FogMaterial.SetVector("L", (Vector4) -this.Sun.transform.forward);
      this.FogMaterial.SetFloat("_InscateringExponent", this.InscateringExponent);
      this.FogMaterial.SetFloat("InscatteringTransitionWideness", this.InscatteringTransitionWideness);
    }
    if (this.EnableNoise && (bool) (UnityEngine.Object) this.NoiseVolume)
    {
      Shader.SetGlobalTexture("_NoiseVolume", (Texture) this.NoiseVolume);
      this.FogMaterial.SetFloat("gain", this.NoiseIntensity);
      this.FogMaterial.SetFloat("threshold", this.NoiseContrast * 0.5f);
      this.FogMaterial.SetFloat("_3DNoiseScale", this._3DNoiseScale * (1f / 1000f));
      this.FogMaterial.SetFloat("_3DNoiseStepSize", this._3DNoiseStepSize * (1f / 1000f) / (float) this.Quality);
      this.FogMaterial.SetVector("Speed", this.Speed);
      this.FogMaterial.SetVector("Stretch", new Vector4(1f, 1f, 1f, 1f) + this.Stretch * 0.01f);
    }
    this.FogMaterial.SetFloat("InscatteringStartDistance", this.InscatteringStartDistance);
    this.volumeSize = this.FogVolumeGameObject.transform.localScale;
    this.transform.localScale = new Vector3((float) Decimal.Round((Decimal) this.volumeSize.x, 2), this.volumeSize.y, this.volumeSize.z);
    this.volumeSize = this.FogVolumeGameObject.transform.lossyScale;
    this.FogMaterial.SetVector("_BoxMin", (Vector4) (this.volumeSize * -0.5f));
    this.FogMaterial.SetVector("_BoxMax", (Vector4) (this.volumeSize * 0.5f));
    this.FogMaterial.SetFloat("_Visibility", this.Visibility);
    this.FogMaterial.renderQueue = this.QueuePosition;
  }

  private void ToggleKeywords()
  {
    this.FogMaterial.EnableKeyword("_FOG_VOLUME_NOISE", this.EnableNoise);
    this.FogMaterial.EnableKeyword("_FOG_VOLUME_INSCATTERING", this.EnableInscattering && (bool) (UnityEngine.Object) this.Sun);
    this.FogMaterial.EnableKeyword("_LQ", this.Quality == 1);
    this.FogMaterial.EnableKeyword("_MQ", this.Quality == 2);
    this.FogMaterial.EnableKeyword("_HQ", this.Quality == 3);
  }
}
