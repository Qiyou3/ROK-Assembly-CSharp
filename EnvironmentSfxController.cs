// Decompiled with JetBrains decompiler
// Type: EnvironmentSfxController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Networking;
using UnityEngine;

#nullable disable
public class EnvironmentSfxController : MonoBehaviour
{
  public string LightWind = nameof (LightWind);
  public string HeavyWind = "StrongWind";
  public string LightRain = nameof (LightRain);
  public float LightRainThreshold = 0.1f;
  public string MediumRain = nameof (MediumRain);
  public float MediumRainThreshold = 0.5f;
  public string HeavyRain = nameof (HeavyRain);
  public float HeavyRainThreshold = 0.95f;
  public string MainCameraTag = "MainCamera";
  private Transform _mainCameraTransform;
  private Weather _weather;
  private PrecipitationController _precipitationController;
  private bool _runUpdate;

  private PrecipitationController precipitationController
  {
    get
    {
      if ((Object) this._precipitationController == (Object) null)
        this._precipitationController = this._weather.GetComponentInChildren<PrecipitationController>();
      return this._precipitationController;
    }
  }

  public void OnLevelWasLoaded(int level) => this.Start();

  public void Start()
  {
    this._weather = Weather.Instance;
    if ((Object) this._weather == (Object) null)
    {
      this.LogError<EnvironmentSfxController>("Couldn't not find weather object, disabling EnvironmentSfxController");
      this._runUpdate = false;
    }
    else
      this._runUpdate = true;
    this._mainCameraTransform = GameObject.FindGameObjectWithTag(this.MainCameraTag).transform;
  }

  public void Update()
  {
    if (!this._runUpdate || Player.IsLocalDedi)
      return;
    switch (this._weather.CurrentWeather)
    {
      case Weather.WeatherType.Clear:
        if (AudioController.IsPlaying(this.HeavyWind))
          AudioController.Stop(this.HeavyWind);
        if (AudioController.IsPlaying(this.LightWind))
        {
          AudioController.Stop(this.LightWind);
          break;
        }
        break;
      case Weather.WeatherType.Cloudy:
        if (AudioController.IsPlaying(this.HeavyWind))
          AudioController.Stop(this.HeavyWind);
        if (!AudioController.IsPlaying(this.LightWind))
        {
          AudioController.Play(this.LightWind, this._mainCameraTransform);
          break;
        }
        break;
      case Weather.WeatherType.PrecipitateLow:
        if (AudioController.IsPlaying(this.HeavyWind))
          AudioController.Stop(this.HeavyWind);
        if (!AudioController.IsPlaying(this.LightWind))
        {
          AudioController.Play(this.LightWind, this._mainCameraTransform);
          break;
        }
        break;
      case Weather.WeatherType.PrecipitateMedium:
        if (AudioController.IsPlaying(this.HeavyWind))
          AudioController.Stop(this.HeavyWind);
        if (!AudioController.IsPlaying(this.LightWind))
        {
          AudioController.Play(this.LightWind, this._mainCameraTransform);
          break;
        }
        break;
      case Weather.WeatherType.PrecipitateHeavy:
        if (AudioController.IsPlaying(this.LightWind))
          AudioController.Stop(this.LightWind);
        if (!AudioController.IsPlaying(this.HeavyWind))
        {
          AudioController.Play(this.HeavyWind, this._mainCameraTransform);
          break;
        }
        break;
    }
    if ((double) this._weather.PrecipitationVolume < (double) this.LightRainThreshold || (Object) this.precipitationController != (Object) null && this.precipitationController.CurrentPrecipitationType != PrecipitationController.PrecipitationType.Rain)
    {
      if (AudioController.IsPlaying(this.LightRain))
        AudioController.Stop(this.LightRain);
      if (AudioController.IsPlaying(this.MediumRain))
        AudioController.Stop(this.MediumRain);
      if (!AudioController.IsPlaying(this.HeavyRain))
        return;
      AudioController.Stop(this.HeavyRain);
    }
    else if ((double) this._weather.PrecipitationVolume >= (double) this.LightRainThreshold && (double) this._weather.PrecipitationVolume < (double) this.MediumRainThreshold)
    {
      if (AudioController.IsPlaying(this.MediumRain))
        AudioController.Stop(this.MediumRain);
      if (AudioController.IsPlaying(this.HeavyRain))
        AudioController.Stop(this.HeavyRain);
      if (AudioController.IsPlaying(this.LightRain))
        return;
      AudioController.Play(this.LightRain, this._mainCameraTransform);
    }
    else if ((double) this._weather.PrecipitationVolume >= (double) this.MediumRainThreshold && (double) this._weather.PrecipitationVolume < (double) this.HeavyRainThreshold)
    {
      if (AudioController.IsPlaying(this.LightRain))
        AudioController.Stop(this.LightRain);
      if (AudioController.IsPlaying(this.HeavyRain))
        AudioController.Stop(this.HeavyRain);
      if (AudioController.IsPlaying(this.MediumRain))
        return;
      AudioController.Play(this.MediumRain, this._mainCameraTransform);
    }
    else
    {
      if ((double) this._weather.PrecipitationVolume < (double) this.HeavyRainThreshold)
        return;
      if (AudioController.IsPlaying(this.LightRain))
        AudioController.Stop(this.LightRain);
      if (AudioController.IsPlaying(this.MediumRain))
        AudioController.Stop(this.MediumRain);
      if (AudioController.IsPlaying(this.HeavyRain))
        return;
      AudioController.Play(this.HeavyRain, this._mainCameraTransform);
    }
  }
}
