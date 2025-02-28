// Decompiled with JetBrains decompiler
// Type: FogVolumeController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.TerrainAPI;
using SmartAssembly.Attributes;
using System;
using UnityEngine;

#nullable disable
[DoNotObfuscate]
[DoNotObfuscateType]
public class FogVolumeController : MonoBehaviour
{
  public static FogVolumeController Instance;
  public FogVolume fogVolume;
  public Transform trackingTransform;
  public Weather _Weather;
  public Renderer[] WaterFogRenderers;
  public FogVolumeController.BiomeFogVolumeSetting[] BiomeSettings;
  public Color GlobalFogColor = Color.white;
  public float GlobalFogDensity = 1f;
  public float FoggyWeatherVisibility = 300f;
  public float RainyWeatherVisibility = 500f;
  public float LerpUpTime = 5f;
  public float LerpDownTime = 5f;
  public float BiomeCheckRate = 4f;
  public float SaturationMod = 1f;
  public float BrightnessMod = 1f;
  private FogVolumeController.BiomeFogVolumeSetting currentSetting;
  private Color _currentFogColor;
  private float _currentFogVisibility = 300f;
  private float _currentFogHeight;
  private float _currentTerrainHeight;

  public FogVolume FogVolume
  {
    get
    {
      if ((UnityEngine.Object) this.fogVolume == (UnityEngine.Object) null)
      {
        GameObject gameObjectWithTag = Tags.FindGameObjectWithTag("Atmosphere Fog");
        if ((UnityEngine.Object) gameObjectWithTag != (UnityEngine.Object) null)
          this.fogVolume = gameObjectWithTag.GetComponent<FogVolume>();
      }
      return this.fogVolume;
    }
  }

  public Transform TrackingTransform
  {
    get
    {
      if ((UnityEngine.Object) Camera.main == (UnityEngine.Object) null)
        return (Transform) null;
      if ((UnityEngine.Object) this.trackingTransform == (UnityEngine.Object) null)
        this.trackingTransform = Camera.main.transform;
      return this.trackingTransform;
    }
  }

  public Weather Weather
  {
    get
    {
      if ((UnityEngine.Object) this._Weather == (UnityEngine.Object) null)
        this._Weather = UnityEngine.Object.FindObjectOfType<Weather>();
      return this._Weather;
    }
  }

  private Vector3 FogPosition
  {
    get
    {
      return new Vector3(this.TrackingTransform.position.x, this.FogHeight + this._currentTerrainHeight, this.TrackingTransform.position.z);
    }
  }

  private float FogHeight => this._currentFogHeight - this.FogVolume.transform.lossyScale.y / 2f;

  public void Awake() => FogVolumeController.Instance = this;

  private void CheckBiome()
  {
    this._currentTerrainHeight = HalfLife.GainLoss(this._currentTerrainHeight, TerrainAPIBase.GetTerrainHeightAt(this.TrackingTransform.position), this.LerpUpTime, this.LerpDownTime);
    foreach (FogZone fogZone in FogZone.FogZones)
    {
      if (fogZone.IsInZone(this.TrackingTransform.position))
      {
        this.currentSetting = fogZone.BiomeFogVolumeSetting;
        return;
      }
    }
    BiomeID biomeId = TerrainAPIBase.GetBiomeAt(this.TrackingTransform.position);
    this.currentSetting = (FogVolumeController.BiomeFogVolumeSetting) null;
    foreach (FogVolumeController.BiomeFogVolumeSetting biomeSetting in this.BiomeSettings)
    {
      if (biomeSetting.Biome == biomeId)
        this.currentSetting = biomeSetting;
      if (this.currentSetting == null && biomeSetting.Biome == BiomeID.Null)
        this.currentSetting = biomeSetting;
    }
  }

  public void LateUpdate()
  {
    if ((UnityEngine.Object) this.TrackingTransform == (UnityEngine.Object) null || (UnityEngine.Object) this.FogVolume == (UnityEngine.Object) null || (UnityEngine.Object) this.Weather == (UnityEngine.Object) null || TerrainAPIBase.TerrainState != InitState.Ready)
      return;
    this.CheckBiome();
    if (this.currentSetting != null)
    {
      this._currentFogColor = Color.Lerp(this._currentFogColor, this.currentSetting.GetFogColor(), HalfLife.GetRate(this.LerpUpTime));
      this._currentFogVisibility = 1f / HalfLife.GainLoss(1f / Mathf.Max(this._currentFogVisibility, 1f), 1f / Mathf.Max(this.currentSetting.FogVisibility, 1f), this.LerpUpTime, this.LerpDownTime);
      this._currentFogHeight = HalfLife.GainLoss(this._currentFogHeight, this.currentSetting.FogHeight, this.LerpUpTime, this.LerpDownTime);
    }
    float fogDensity = this.Weather.FogDensity;
    float precipitationVolume = this.Weather.PrecipitationVolume;
    float num1 = fogDensity + precipitationVolume;
    if ((double) num1 > 1.0)
    {
      fogDensity /= num1;
      precipitationVolume /= num1;
      num1 = 1f;
    }
    float num2 = 1f - num1;
    this.FogVolume.FogColor = this._currentFogColor * this.GlobalFogColor;
    this.FogVolume.Visibility = (float) ((double) this._currentFogVisibility * (double) num2 + (double) this.FoggyWeatherVisibility * (double) fogDensity + (double) this.RainyWeatherVisibility * (double) precipitationVolume) / this.GlobalFogDensity;
    this.FogVolume.transform.position = this.FogPosition;
    this.FogVolume.TransitionHeight = this._currentFogHeight * 2f;
    Shader.SetGlobalFloat("_GlobalFogVisibility", this.FogVolume.Visibility);
    Shader.SetGlobalColor("_GlobalFogColor", this.FogVolume.FogColor);
    Vector3 localScale = this.transform.localScale;
    float num3 = Camera.main.farClipPlane * 1.154f;
    localScale.x = num3;
    localScale.z = num3;
    this.FogVolume.transform.localScale = localScale;
  }

  [Serializable]
  public class BiomeFogVolumeSetting
  {
    public BiomeID Biome;
    public Color FogColor;
    public float FogVisibility;
    public float FogHeight;
    public bool IncludeAmbientColor;

    public Color GetFogColor() => this.FogColor;
  }
}
