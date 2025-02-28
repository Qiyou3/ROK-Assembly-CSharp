// Decompiled with JetBrains decompiler
// Type: AltitudeEnvironment
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class AltitudeEnvironment : MonoBehaviour
{
  public Light[] affectedLights;
  public List<AltitudeEnvironment.ColorAltitude> colorAltitudes;
  public AltitudeEnvironment.ColorAltitude currentColorAltitude;
  private float[] _originalLightIntensities;

  public void Sample(float altitude)
  {
    AltitudeEnvironment.ColorAltitude other1 = this.colorAltitudes.First<AltitudeEnvironment.ColorAltitude>();
    if ((double) altitude <= (double) other1.altitude)
    {
      this.currentColorAltitude.Set(other1);
    }
    else
    {
      AltitudeEnvironment.ColorAltitude other2 = this.colorAltitudes.Last<AltitudeEnvironment.ColorAltitude>();
      if ((double) altitude >= (double) other2.altitude)
      {
        this.currentColorAltitude.Set(other2);
      }
      else
      {
        for (int index = 1; index < this.colorAltitudes.Count; ++index)
        {
          if ((double) altitude >= (double) this.colorAltitudes[index - 1].altitude && (double) altitude < (double) this.colorAltitudes[index].altitude)
            this.currentColorAltitude.LerpAltitude(this.colorAltitudes[index - 1], this.colorAltitudes[index], altitude);
        }
      }
    }
  }

  public void Start()
  {
    this.colorAltitudes = this.colorAltitudes.OrderBy<AltitudeEnvironment.ColorAltitude, float>((Func<AltitudeEnvironment.ColorAltitude, float>) (c => c.altitude)).ToList<AltitudeEnvironment.ColorAltitude>();
    this._originalLightIntensities = new float[this.affectedLights.Length];
    for (int index = 0; index < this.affectedLights.Length; ++index)
      this._originalLightIntensities[index] = this.affectedLights[index].intensity;
  }

  public void Update()
  {
    this.Sample(this.transform.position.y);
    RenderSettings.ambientLight = this.currentColorAltitude.ambient;
    RenderSettings.fogColor = this.currentColorAltitude.fogColor;
    RenderSettings.fogDensity = this.currentColorAltitude.fogDensity;
    for (int index = 0; index < this.affectedLights.Length; ++index)
    {
      this.affectedLights[index].intensity = this._originalLightIntensities[index] * this.currentColorAltitude.lightIntensity;
      this.affectedLights[index].enabled = (double) this.affectedLights[index].intensity > 0.0099999997764825821;
    }
  }

  [Serializable]
  public class ColorAltitude
  {
    public float altitude;
    public Color ambient;
    public Color fogColor;
    public float fogDensity;
    public float lightIntensity;

    public void Set(AltitudeEnvironment.ColorAltitude other)
    {
      this.ambient = other.ambient;
      this.fogColor = other.fogColor;
      this.fogDensity = other.fogDensity;
      this.lightIntensity = other.lightIntensity;
      this.altitude = other.altitude;
    }

    public void Lerp(
      AltitudeEnvironment.ColorAltitude a,
      AltitudeEnvironment.ColorAltitude b,
      float t)
    {
      this.ambient = Color.Lerp(a.ambient, b.ambient, t);
      this.fogColor = Color.Lerp(a.fogColor, b.fogColor, t);
      this.fogDensity = Mathf.Lerp(a.fogDensity, b.fogDensity, t);
      this.lightIntensity = Mathf.Lerp(a.lightIntensity, b.lightIntensity, t);
      this.altitude = Mathf.Lerp(a.altitude, b.altitude, t);
    }

    public void LerpAltitude(
      AltitudeEnvironment.ColorAltitude a,
      AltitudeEnvironment.ColorAltitude b,
      float altitude)
    {
      float t = Mathf.InverseLerp(a.altitude, b.altitude, altitude);
      this.ambient = Color.Lerp(a.ambient, b.ambient, t);
      this.fogColor = Color.Lerp(a.fogColor, b.fogColor, t);
      this.fogDensity = Mathf.Lerp(a.fogDensity, b.fogDensity, t);
      this.lightIntensity = Mathf.Lerp(a.lightIntensity, b.lightIntensity, t);
      this.altitude = altitude;
    }
  }
}
