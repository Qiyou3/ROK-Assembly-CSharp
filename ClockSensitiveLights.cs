// Decompiled with JetBrains decompiler
// Type: ClockSensitiveLights
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class ClockSensitiveLights : MonoBehaviour
{
  public Light[] Lights;
  public VLight[] VLights;
  public List<EmissiveLightTexture> lightTextures = new List<EmissiveLightTexture>();
  private GameClock gameClock;
  private bool lastLightsOn;

  public void Start()
  {
    this.Lights = this.gameObject.GetComponentsInChildren<Light>();
    this.VLights = this.gameObject.GetComponentsInChildren<VLight>();
    this.gameClock = GameClock.Instance;
  }

  public void Update()
  {
    bool flag = false;
    if ((Object) this.gameClock == (Object) null)
      return;
    switch (this.gameClock.CurrentTimeBlock)
    {
      case GameClock.TimeBlock.Dawn:
        flag = true;
        goto case GameClock.TimeBlock.Morning;
      case GameClock.TimeBlock.Morning:
      case GameClock.TimeBlock.Afternoon:
        if (flag != this.lastLightsOn)
        {
          for (int index = 0; index < this.Lights.Length; ++index)
            this.Lights[index].enabled = flag;
          for (int index = 0; index < this.VLights.Length; ++index)
            this.VLights[index].gameObject.SetActive(flag);
          for (int index = 0; index < this.lightTextures.Count; ++index)
          {
            string propertyName = "_ColorEmissiveR";
            if (this.lightTextures[index].channel == EmissiveLightTexture.Channel.G)
              propertyName = "_ColorEmissiveG";
            else if (this.lightTextures[index].channel == EmissiveLightTexture.Channel.B)
              propertyName = "_ColorEmissiveB";
            foreach (Material material in this.lightTextures[index].renderer.materials)
              material.SetFloat(propertyName, !flag ? this.lightTextures[index].offIntensity : this.lightTextures[index].onIntensity);
          }
        }
        this.lastLightsOn = flag;
        break;
      case GameClock.TimeBlock.Dusk:
        flag = true;
        goto case GameClock.TimeBlock.Morning;
      default:
        flag = true;
        goto case GameClock.TimeBlock.Morning;
    }
  }
}
