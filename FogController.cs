// Decompiled with JetBrains decompiler
// Type: FogController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Modules.Environment;
using UnityEngine;

#nullable disable
public class FogController : MonoBehaviour
{
  public GlobalFog fog;
  public CloudLayer cloudLayer;
  public EnvironmentController environment;
  public float FogDensity;

  public void Start()
  {
    this.fog = (GlobalFog) Object.FindObjectOfType(typeof (GlobalFog));
    if ((Object) this.fog == (Object) null)
      this.LogError<FogController>("Could not find global fog shader (usually located on the main camera)");
    this.cloudLayer = (CloudLayer) Object.FindObjectOfType(typeof (CloudLayer));
    if ((Object) this.fog == (Object) null)
      this.LogError<FogController>("Could not find cloud layer (usually located on the cloud controller)");
    this.environment = (EnvironmentController) Object.FindObjectOfType(typeof (EnvironmentController));
    if (!((Object) this.environment == (Object) null))
      return;
    this.LogError<FogController>("Could not find environment controller");
  }

  public void Update()
  {
    float num1 = Mathf.Clamp(this.cloudLayer.coverage, 0.0f, 1f);
    float fogDensity = this.environment.currentEffectsAtElevation.fogDensity;
    float num2 = fogDensity + num1 * this.environment.currentEffectsAtElevation.cloudCoverageToFogDesntiyFactor * fogDensity;
    this.fog.globalFogColor = this.environment.currentEffectsAtElevation.fogColor;
    this.fog.globalDensity = num2;
  }
}
