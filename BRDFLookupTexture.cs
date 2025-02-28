// Decompiled with JetBrains decompiler
// Type: BRDFLookupTexture
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class BRDFLookupTexture : MonoBehaviour
{
  public float intensity = 1f;
  public float diffuseIntensity = 1f;
  public Color keyColor = BRDFLookupTexture.ColorRGB(188, 158, 118);
  public Color fillColor = BRDFLookupTexture.ColorRGB(86, 91, 108);
  public Color backColor = BRDFLookupTexture.ColorRGB(44, 54, 57);
  public float wrapAround;
  public float metalic;
  public float specularIntensity = 1f;
  public float specularShininess = 5f / 64f;
  public float translucency;
  public Color translucentColor = BRDFLookupTexture.ColorRGB((int) byte.MaxValue, 82, 82);
  public int lookupTextureWidth = 128;
  public int lookupTextureHeight = 128;
  public bool fastPreview = true;
  public Texture2D lookupTexture;

  private void Awake()
  {
    if ((bool) (Object) this.lookupTexture)
      return;
    this.Bake();
  }

  private static Color ColorRGB(int r, int g, int b)
  {
    return new Color((float) r / (float) byte.MaxValue, (float) g / (float) byte.MaxValue, (float) b / (float) byte.MaxValue, 0.0f);
  }

  private void CheckConsistency()
  {
    this.intensity = Mathf.Max(0.0f, this.intensity);
    this.wrapAround = Mathf.Clamp(this.wrapAround, -1f, 1f);
    this.metalic = Mathf.Clamp(this.metalic, 0.0f, 12f);
    this.diffuseIntensity = Mathf.Max(0.0f, this.diffuseIntensity);
    this.specularIntensity = Mathf.Max(0.0f, this.specularIntensity);
    this.specularShininess = Mathf.Clamp(this.specularShininess, 0.01f, 1f);
    this.translucency = Mathf.Clamp01(this.translucency);
  }

  private Color PixelFunc(float ndotl, float ndoth)
  {
    ndotl *= Mathf.Pow(ndoth, this.metalic);
    float num1 = (float) (1.0 + (double) this.metalic * 0.25) * Mathf.Max(0.0f, this.diffuseIntensity - (1f - ndoth) * this.metalic);
    float t1 = Mathf.Clamp01(Mathf.InverseLerp(-this.wrapAround, 1f, (float) ((double) ndotl * 2.0 - 1.0)));
    float t2 = Mathf.Clamp01(Mathf.InverseLerp(-1f, Mathf.Max(-0.99f, -this.wrapAround), (float) ((double) ndotl * 2.0 - 1.0)));
    Color color = num1 * Color.Lerp(this.backColor, Color.Lerp(this.fillColor, this.keyColor, t1), t2) + this.backColor * (1f - num1) * Mathf.Clamp01(this.diffuseIntensity);
    float p = this.specularShininess * 128f;
    float a = this.specularIntensity * (float) (((double) p + 2.0) * ((double) p + 4.0) / (25.132741928100586 * ((double) Mathf.Pow(2f, (float) (-(double) p / 2.0)) + (double) p))) * Mathf.Pow(ndoth, p);
    float num2 = 0.5f * this.translucency * Mathf.Clamp01((float) (1.0 - (double) (ndotl + 0.1f) * (double) ndoth)) * Mathf.Clamp01(1f - ndotl);
    return (color * this.intensity + this.translucentColor * num2 + new Color(0.0f, 0.0f, 0.0f, a)) * this.intensity;
  }

  private void TextureFunc(Texture2D tex)
  {
    for (int y = 0; y < tex.height; ++y)
    {
      for (int x = 0; x < tex.width; ++x)
      {
        float width = (float) tex.width;
        float height = (float) tex.height;
        Color color = this.PixelFunc((float) x / width, (float) y / height);
        tex.SetPixel(x, y, color);
      }
    }
  }

  private void GenerateLookupTexture(int width, int height)
  {
    Texture2D tex = !(bool) (Object) this.lookupTexture || this.lookupTexture.width != width || this.lookupTexture.height != height ? new Texture2D(width, height, TextureFormat.ARGB32, false) : this.lookupTexture;
    this.CheckConsistency();
    this.TextureFunc(tex);
    tex.Apply();
    tex.wrapMode = TextureWrapMode.Clamp;
    if ((Object) this.lookupTexture != (Object) tex)
      Object.DestroyImmediate((Object) this.lookupTexture);
    this.lookupTexture = tex;
  }

  public void Preview() => this.GenerateLookupTexture(32, 64);

  public void Bake()
  {
    this.GenerateLookupTexture(this.lookupTextureWidth, this.lookupTextureHeight);
  }
}
