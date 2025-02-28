// Decompiled with JetBrains decompiler
// Type: BrakeLightController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;
using UnityEngine;

#nullable disable
public class BrakeLightController : EntityBehaviour
{
  private CarController carController;
  private bool lastOn;
  public Renderer[] renderers;
  public Light[] lights;
  public float materialOffIntensity = 1f;
  public float materialOnIntensity = 7f;
  public EmissiveLightTexture.Channel materialChannel;
  public float lightOffIntensity;
  public float lightOnIntensity = 1f;
  private float intensity;

  public void Start() => this.carController = this.Entity.Get<CarController>();

  public void Update()
  {
    bool brakeKey = this.carController.brakeKey;
    if (brakeKey)
      this.intensity += Time.deltaTime / 0.1f;
    else
      this.intensity -= Time.deltaTime / 0.1f;
    this.intensity = Mathf.Clamp01(this.intensity);
    for (int index = 0; index < this.lights.Length; ++index)
      this.lights[index].intensity = Mathf.Lerp(this.lightOffIntensity, this.lightOnIntensity, this.intensity);
    if (this.lastOn != brakeKey)
    {
      string propertyName = "_ColorEmissiveR";
      if (this.materialChannel == EmissiveLightTexture.Channel.G)
        propertyName = "_ColorEmissiveG";
      else if (this.materialChannel == EmissiveLightTexture.Channel.B)
        propertyName = "_ColorEmissiveB";
      for (int index = 0; index < this.renderers.Length; ++index)
      {
        foreach (Material material in this.renderers[index].materials)
          material.SetFloat(propertyName, !brakeKey ? this.materialOffIntensity : this.materialOnIntensity);
      }
    }
    this.lastOn = this.carController.brakeKey;
  }
}
