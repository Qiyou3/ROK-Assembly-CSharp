// Decompiled with JetBrains decompiler
// Type: AtmosphereTexture
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.IO;
using UnityEngine;

#nullable disable
public class AtmosphereTexture : MonoBehaviour
{
  public float ratioPower = 2f;
  public float altitudeMax = 1.2f;
  public int xAngleResolution = 256;
  public int yAltitudeResolution = 512;
  public float densityMultiplier = 0.25f;
  public float earthDensityMultiplier = 3f;
  public float atmosphereRadius = 1.1f;
  public Texture2D texture2D;
  public float planetRadius = 6400000f;
  public Renderer atmosphereRenderer;
  public float renderDistanceMultiplier = 0.9f;

  public float AltitudeFromTexcoord(float v)
  {
    return (float) (((double) this.altitudeMax - 1.0) * (double) Mathf.Pow(v, this.ratioPower) + 1.0);
  }

  public float TexcoordFromAltitude(float altitude)
  {
    return Mathf.Pow((float) (((double) altitude - 1.0) / ((double) this.altitudeMax - 1.0)), 1f / this.ratioPower);
  }

  public static float CalculateDensity(
    float atmosphereRadius,
    float earthDensityMultiplier,
    float densityMultiplier,
    int earthNumSolutions,
    float earthDist1,
    float earthDist2,
    float dot,
    float scaledDot,
    float angle,
    float altitude)
  {
    float f1 = (float) ((double) scaledDot * (double) scaledDot - (double) altitude * (double) altitude + (double) atmosphereRadius * (double) atmosphereRadius);
    float num1 = 0.0f;
    float num2 = 0.0f;
    if ((double) f1 >= 0.0)
    {
      if ((double) f1 == 0.0)
      {
        num1 = scaledDot;
        num2 = scaledDot;
      }
      else
      {
        num1 = scaledDot - Mathf.Sqrt(f1);
        num2 = scaledDot + Mathf.Sqrt(f1);
        if ((double) num2 < (double) num1)
        {
          float num3 = num2;
          num2 = num1;
          num1 = num3;
        }
        if ((double) num1 < 0.0)
          num1 = 0.0f;
        if ((double) num2 < 0.0)
          num2 = 0.0f;
      }
    }
    float num4 = num2 - num1;
    if (earthNumSolutions > 0 && (double) num2 > (double) earthDist1 && (double) earthDist1 < (double) earthDist2)
    {
      num2 = earthDist1;
      num4 = (num2 - num1) * earthDensityMultiplier;
    }
    Vector2 vector2_1 = new Vector2(dot, Mathf.Cos(angle));
    Vector2 vector2_2 = vector2_1 * num1;
    Vector2 vector2_3 = vector2_1 * num2;
    Vector2 vector2_4 = (Vector2) new Vector3(altitude, 0.0f);
    float num5 = Vector2.Dot(vector2_2 - vector2_4, vector2_2 - vector2_4);
    float num6 = 2f * Vector2.Dot(vector2_3 - vector2_2, vector2_2 - vector2_4);
    float f2 = Vector2.Dot(vector2_3 - vector2_2, vector2_3 - vector2_2);
    float num7 = Mathf.Sqrt(f2 * (num5 + num6 + f2));
    float num8 = Mathf.Sqrt(f2 * num5);
    float num9 = 1f - Mathf.Clamp01((float) (((double) ((float) (4.0 * (double) f2 * (double) num7 + 2.0 * (double) num6 * ((double) num7 - (double) num8) + ((double) num6 * (double) num6 - 4.0 * (double) num5 * (double) f2) * (double) Mathf.Log((float) (((double) num6 + 2.0 * (double) num8) / (2.0 * (double) f2 + (double) num6 + 2.0 * (double) num7)))) / (8f * Mathf.Pow(f2, 1.5f))) - 1.0) / ((double) atmosphereRadius - 1.0)));
    return num4 * (densityMultiplier * num9 * num9);
  }

  [ContextMenu("Create")]
  public void Create()
  {
    if ((Object) this.texture2D == (Object) null)
      this.texture2D = new Texture2D(this.xAngleResolution, this.yAltitudeResolution);
    else
      this.texture2D.Resize(this.xAngleResolution, this.yAltitudeResolution);
    Color[] pixels = this.texture2D.GetPixels();
    for (int index1 = 0; index1 < this.xAngleResolution; ++index1)
    {
      float angle = (float) (3.1415927410125732 * (double) index1 / (double) (this.xAngleResolution - 1) - 1.5707963705062866);
      float dot = Mathf.Sin(-angle);
      for (int index2 = 0; index2 < this.yAltitudeResolution; ++index2)
      {
        float altitude = this.AltitudeFromTexcoord((float) index2 / (float) (this.yAltitudeResolution - 1));
        float scaledDot = dot * altitude;
        float f = (float) ((double) scaledDot * (double) scaledDot - (double) altitude * (double) altitude + 1.0);
        float earthDist1 = 0.0f;
        float earthDist2 = 0.0f;
        int earthNumSolutions;
        if ((double) f < 0.0)
          earthNumSolutions = 0;
        else if ((double) f == 0.0)
        {
          earthNumSolutions = 1;
          earthDist1 = scaledDot;
          earthDist2 = scaledDot;
        }
        else
        {
          earthNumSolutions = 2;
          earthDist1 = scaledDot - Mathf.Sqrt(f);
          earthDist2 = scaledDot + Mathf.Sqrt(f);
          if ((double) earthDist2 < (double) earthDist1)
          {
            float num = earthDist2;
            earthDist2 = earthDist1;
            earthDist1 = num;
          }
          if ((double) earthDist1 < 0.0)
            earthDist1 = 0.0f;
          if ((double) earthDist2 < 0.0)
            earthDist2 = 0.0f;
        }
        float density = AtmosphereTexture.CalculateDensity(this.atmosphereRadius, this.earthDensityMultiplier, this.densityMultiplier, earthNumSolutions, earthDist1, earthDist2, dot, scaledDot, angle, altitude);
        int index3 = index1 + this.xAngleResolution * index2;
        pixels[index3] = Color.Lerp(Color.black, Color.white, density);
      }
    }
    this.texture2D.SetPixels(pixels);
    this.texture2D.Apply();
    byte[] png = this.texture2D.EncodeToPNG();
    using (StreamWriter streamWriter = new StreamWriter("Assets/AtmosphereTexture" + (object) Random.value + ".png", true))
      streamWriter.BaseStream.Write(png, 0, png.Length);
  }

  public void Start()
  {
  }

  public void OnWillRenderObject()
  {
    this.transform.position = Camera.current.transform.position;
    this.transform.localScale = Vector3.one * (float) ((double) Camera.current.farClipPlane * (double) this.renderDistanceMultiplier * 2.0);
    float y = this.TexcoordFromAltitude((Camera.current.transform.position.y + this.planetRadius) / this.planetRadius);
    if (Application.isPlaying)
      this.atmosphereRenderer.material.mainTextureOffset = new Vector2(0.0f, y);
    else
      this.atmosphereRenderer.sharedMaterial.mainTextureOffset = new Vector2(0.0f, y);
  }
}
