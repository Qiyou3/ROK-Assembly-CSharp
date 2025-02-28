// Decompiled with JetBrains decompiler
// Type: AtmosphereController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Modules.Environment;
using UnityEngine;

#nullable disable
public class AtmosphereController : MonoBehaviour
{
  public EnvironmentController environment;
  public Transform trackLight;
  public string atmosphereAddPropertyName;
  public string atmosphereBlendPropertyName;
  public string atmosphereAltitudePropertyName;
  public string lightDirectionPropertyName;

  public void Start()
  {
    this.environment = (EnvironmentController) Object.FindObjectOfType(typeof (EnvironmentController));
    if (!((Object) this.environment == (Object) null))
      return;
    this.LogError<AtmosphereController>("Could not find environment controller");
  }

  public void Update()
  {
    this.transform.position = Camera.main.transform.position;
    if (!((Object) this.GetComponent<Renderer>() != (Object) null))
      return;
    this.GetComponent<Renderer>().material.SetColor(this.atmosphereAddPropertyName, this.environment.currentEffectsAtElevation.atmosphereColorAdd);
    this.GetComponent<Renderer>().material.SetColor(this.atmosphereBlendPropertyName, this.environment.currentEffectsAtElevation.atmosphereColorBlend);
    float y = Camera.main.transform.position.y;
    this.GetComponent<Renderer>().material.SetFloat(this.atmosphereAltitudePropertyName, y);
    this.GetComponent<Renderer>().material.SetVector(this.lightDirectionPropertyName, (Vector4) this.trackLight.forward);
  }
}
