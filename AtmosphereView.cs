// Decompiled with JetBrains decompiler
// Type: AtmosphereView
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Engine.Modding.Abstract;
using CodeHatch.TerrainAPI;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class AtmosphereView : MonoBehaviour, IModable
{
  public static AtmosphereView Instance;
  public AnimationCurve CloudDensityToPrecipitationVolume;
  public AnimationCurve CloudSharpnessToCloudCover;
  public AnimationCurve OcclusionToCloudCover;
  public AnimationCurve SunUpdateMultiplierAtVertical;
  public Color ClearSkyAmbientColor;
  public float ClearSkyAmbientMultiplier = 1f;
  public Color CloudySkyAmbientColor;
  public float CloudySkyAmbientMultiplier = 1f;
  public float ProbeGroundMultiplier;
  public float ProbeSkyMultiplier;
  public float ProbeSunMultiplier;
  public float BaseGroundReflectionAmount = 0.1f;
  public AnimationCurve AmbientOcclusionCompensationForAOPower = new AnimationCurve(new Keyframe[2]
  {
    new Keyframe(0.0f, 1f),
    new Keyframe(5f, 1.5f)
  });
  public float[] SunUpdateIntervalAtUnityQuality = new float[3]
  {
    0.3333f,
    0.1f,
    0.033333f
  };
  private Color _LerpedGroundColor;
  private float _LerpedGroundHeightOffset;
  private float _LerpedGroundLightDirectionality;
  private Weather _Weather;
  private GameClock _GameClock;
  private LightProbeRenderer _LightProbeRenderer;
  private FastAO _FastAO;
  private TOD_Sky _TOD_Sky;
  private SunShafts _SunShafts;
  private Light _LightUsed;
  public float _AreaLatitude;
  public float _AreaLongitude;
  public Color _SunColor = Color.white;
  public Color _MoonColor = Color.white;
  private Color _defaultDayAmbientColor;
  private Color _defaultNightAmbientColor;
  private ChangeBlindVariable<float> _timeOfDayChangeBlind = new ChangeBlindVariable<float>(3f, 30f);
  private float _lastSunTimeSet;

  public Weather Weather
  {
    get
    {
      if ((Object) this._Weather == (Object) null)
        this._Weather = Weather.Instance;
      return this._Weather;
    }
  }

  public GameClock GameClock
  {
    get
    {
      if ((Object) this._GameClock == (Object) null)
        this._GameClock = GameClock.Instance;
      return this._GameClock;
    }
  }

  public LightProbeRenderer LightProbeRenderer
  {
    get
    {
      if ((Object) this._LightProbeRenderer == (Object) null)
      {
        if ((Object) this.TOD_Sky == (Object) null)
          return (LightProbeRenderer) null;
        this._LightProbeRenderer = this.TOD_Sky.GetComponentInChildren<LightProbeRenderer>();
      }
      return this._LightProbeRenderer;
    }
  }

  public FastAO FastAO
  {
    get
    {
      if ((Object) this._FastAO == (Object) null)
        this._FastAO = FastAO.Instance;
      return this._FastAO;
    }
  }

  public TOD_Sky TOD_Sky
  {
    get
    {
      if ((Object) this._TOD_Sky == (Object) null)
      {
        this._TOD_Sky = TOD_Sky.Instance;
        if ((Object) this._TOD_Sky == (Object) null)
          return (TOD_Sky) null;
        this._defaultDayAmbientColor = this._TOD_Sky.Day.AmbientColor;
        this._defaultNightAmbientColor = this._TOD_Sky.Night.AmbientColor;
        this._TOD_Sky.Atmosphere.GroundFunction = new TOD_Sky.GroundDelegate(this.GroundFunction);
        this.SunColor = this._SunColor;
        this.MoonColor = this._MoonColor;
        this.AreaLongitude = this._AreaLongitude;
        this.AreaLatitude = this._AreaLatitude;
      }
      return this._TOD_Sky;
    }
  }

  public SunShafts SunShafts
  {
    get
    {
      if ((Object) this._SunShafts == (Object) null)
        this._SunShafts = SunShafts.Instance;
      return this._SunShafts;
    }
  }

  public Light LightUsed
  {
    get
    {
      if ((Object) this._LightUsed == (Object) null)
      {
        if ((Object) this.TOD_Sky == (Object) null)
          return (Light) null;
        if ((Object) this.TOD_Sky.Components.Light == (Object) null)
          return (Light) null;
        this._LightUsed = this.TOD_Sky.Components.Light.GetComponent<Light>();
      }
      return this._LightUsed;
    }
  }

  public float AreaLatitude
  {
    get => this._AreaLatitude;
    set
    {
      this._AreaLatitude = value;
      if (!((Object) this.TOD_Sky != (Object) null))
        return;
      this.TOD_Sky.World.Latitude = value;
    }
  }

  public float AreaLongitude
  {
    get => this._AreaLongitude;
    set
    {
      this._AreaLongitude = value;
      if (!((Object) this.TOD_Sky != (Object) null))
        return;
      this.TOD_Sky.World.Longitude = value;
    }
  }

  public Color SunColor
  {
    get => this._SunColor;
    set
    {
      this._SunColor = value;
      if (!((Object) this.TOD_Sky != (Object) null))
        return;
      this.TOD_Sky.Sun.LightColor = value;
      this.TOD_Sky.Sun.MeshColor = value;
    }
  }

  public Color MoonColor
  {
    get => this._MoonColor;
    set
    {
      this._MoonColor = value;
      if (!((Object) this.TOD_Sky != (Object) null))
        return;
      this.TOD_Sky.Moon.LightColor = value;
      this.TOD_Sky.Moon.MeshColor = value;
    }
  }

  public float SunHeight
  {
    get
    {
      return (Object) this.TOD_Sky == (Object) null ? 0.0f : Vector3.Dot(this.TOD_Sky.Components.LightTransform.forward, Vector3.down);
    }
  }

  public Color GroundFunction(Vector3 dir)
  {
    Vector3 forward = this.LightUsed.transform.forward;
    Vector3 normalized = Vector3.Lerp(Vector3.up, -dir, this._LerpedGroundLightDirectionality).normalized;
    Color color = this.LightUsed.color * this.LightUsed.intensity;
    return ((Mathf.Clamp01(-Vector3.Dot(forward, normalized)) + this.BaseGroundReflectionAmount) * this.ProbeGroundMultiplier * color * this._LerpedGroundColor) with
    {
      a = Mathf.Clamp01(-dir.y + this._LerpedGroundHeightOffset)
    };
  }

  public void Awake() => AtmosphereView.Instance = this;

  public void Update()
  {
    if ((Object) this.TOD_Sky == (Object) null)
      return;
    Shader.SetGlobalVector("_LightDirection", (Vector4) this.TOD_Sky.Components.LightTransform.forward);
    float num1 = 0.03333f;
    if ((Object) CodeHatch.UnityQualitySettings.UnityQualitySettings.Instance != (Object) null)
      num1 = this.SunUpdateIntervalAtUnityQuality[(int) CodeHatch.UnityQualitySettings.UnityQualitySettings.Instance.CurrentUnityQuality];
    if ((double) Time.time - (double) this._lastSunTimeSet > (double) (num1 * this.SunUpdateMultiplierAtVertical.Evaluate(this.SunHeight)))
    {
      this._timeOfDayChangeBlind.Value = this.GameClock.TimeOfDay;
      this._lastSunTimeSet = Time.time;
    }
    this.TOD_Sky.Clouds.Density = this.CloudDensityToPrecipitationVolume.Evaluate(this.Weather.PrecipitationVolume);
    this.TOD_Sky.Clouds.Sharpness = this.CloudSharpnessToCloudCover.Evaluate(this.Weather.CloudCover);
    this.TOD_Sky.Cycle.Hour = this._timeOfDayChangeBlind.Value;
    float t = Mathf.Clamp01(this.OcclusionToCloudCover.Evaluate(this.Weather.CloudCover));
    Color color = t * this.CloudySkyAmbientColor + (1f - t) * this.ClearSkyAmbientColor;
    float a = Mathf.Lerp(this.ClearSkyAmbientMultiplier, this.CloudySkyAmbientMultiplier, t);
    if ((Object) this.FastAO != (Object) null)
      a *= this.AmbientOcclusionCompensationForAOPower.Evaluate(this.FastAO.enabled ? this.FastAO.power : 0.0f);
    float num2 = Mathf.Max(a, 0.0f);
    this.TOD_Sky.Sun.LightColor = this._SunColor * (float) (1.1200000047683716 - (double) t * 0.64999997615814209);
    this.TOD_Sky.Moon.LightColor = this._MoonColor * (float) (1.1200000047683716 - (double) t * 0.64999997615814209);
    this.TOD_Sky.Day.AmbientColor = this._defaultDayAmbientColor * color;
    this.TOD_Sky.Day.AmbientMultiplier = num2;
    this.TOD_Sky.Night.AmbientColor = this._defaultNightAmbientColor * color;
    this.TOD_Sky.Day.AmbientMultiplier = num2;
    if ((Object) this.SunShafts != (Object) null && (Object) this.TOD_Sky.Components.Light != (Object) null)
      this.SunShafts.sunColor = this.TOD_Sky.Components.Light.GetComponent<Light>().color;
    this.TOD_Sky.transform.localScale = Vector3.one * (Camera.main.farClipPlane * 0.98f);
    if (!((Object) this.LightProbeRenderer != (Object) null))
      return;
    Color b = Color.grey;
    float to1 = 0.5f;
    float to2 = 0.5f;
    if ((Object) HeightMapAPI.Instance != (Object) null)
    {
      b = TerrainAPIBase.TerrainState < InitState.Initialized ? this._LerpedGroundColor : HeightMapAPI.Instance.GetColorAt(Camera.main.transform.position);
      TerrainAmbientLight.BiomeAmbientSettings settingsForBiome = TerrainAPIBase.TerrainState < InitState.Initialized ? (TerrainAmbientLight.BiomeAmbientSettings) null : TerrainAmbientLight.GetSettingsForBiome(TerrainAPIBase.GetBiomeAt(Camera.main.transform.position));
      if (settingsForBiome != null)
      {
        to1 = settingsForBiome.GroundHeightOffset;
        to2 = settingsForBiome.GroundLightDirectionality;
      }
    }
    this._LerpedGroundColor = Color.Lerp(this._LerpedGroundColor, b, HalfLife.GetRate(2f));
    this._LerpedGroundHeightOffset = Mathf.Lerp(this._LerpedGroundHeightOffset, to1, HalfLife.GetRate(2f));
    this._LerpedGroundLightDirectionality = Mathf.Lerp(this._LerpedGroundLightDirectionality, to2, HalfLife.GetRate(2f));
  }

  public string ModHandlerName => "Atmosphere";

  public void GetModDefaults(IList<ModEntry> defaultModList)
  {
    defaultModList.Add(new ModEntry("IslandLatitude", (object) this.AreaLatitude));
    defaultModList.Add(new ModEntry("IslandLongitude", (object) this.AreaLongitude));
    defaultModList.Add(new ModEntry("SunColor", (object) new ColorModParseable()
    {
      Value = this.SunColor
    }));
    defaultModList.Add(new ModEntry("MoonColor", (object) new ColorModParseable()
    {
      Value = this.MoonColor
    }));
    if (!((Object) FogVolumeController.Instance != (Object) null))
      return;
    defaultModList.Add(new ModEntry("FogColor", (object) new ColorModParseable()
    {
      Value = FogVolumeController.Instance.GlobalFogColor
    }));
    defaultModList.Add(new ModEntry("FogDensity", (object) FogVolumeController.Instance.GlobalFogDensity));
  }

  public void ApplyMod(string key, object value)
  {
    string key1 = key;
    if (key1 == null)
      return;
    // ISSUE: reference to a compiler-generated field
    if (AtmosphereView.\u003C\u003Ef__switch\u0024map1D == null)
    {
      // ISSUE: reference to a compiler-generated field
      AtmosphereView.\u003C\u003Ef__switch\u0024map1D = new Dictionary<string, int>(6)
      {
        {
          "IslandLatitude",
          0
        },
        {
          "IslandLongitude",
          1
        },
        {
          "SunColor",
          2
        },
        {
          "MoonColor",
          3
        },
        {
          "FogColor",
          4
        },
        {
          "FogDensity",
          5
        }
      };
    }
    int num;
    // ISSUE: reference to a compiler-generated field
    if (!AtmosphereView.\u003C\u003Ef__switch\u0024map1D.TryGetValue(key1, out num))
      return;
    switch (num)
    {
      case 0:
        this.AreaLatitude = Mathf.Clamp((float) value, -90f, 90f);
        break;
      case 1:
        this.AreaLongitude = Mathf.Clamp((float) value, -90f, 90f);
        break;
      case 2:
        this.SunColor = ((ColorModParseable) value).Value;
        break;
      case 3:
        this.MoonColor = ((ColorModParseable) value).Value;
        break;
      case 4:
        if (!((Object) FogVolumeController.Instance != (Object) null))
          break;
        FogVolumeController.Instance.GlobalFogColor = ((ColorModParseable) value).Value;
        break;
      case 5:
        if (!((Object) FogVolumeController.Instance != (Object) null))
          break;
        FogVolumeController.Instance.GlobalFogDensity = (float) value;
        break;
    }
  }
}
