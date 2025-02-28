// Decompiled with JetBrains decompiler
// Type: AudioAtNight
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[RequireComponent(typeof (AudioSource))]
public class AudioAtNight : MonoBehaviour
{
  public TOD_Sky sky;
  public float fadeTime = 1f;
  private float lerpTime;
  private AudioSource audioComponent;
  private float audioVolume;

  protected void OnEnable()
  {
    if (!(bool) (Object) this.sky)
      this.sky = TOD_Sky.Instance;
    this.audioComponent = this.GetComponent<AudioSource>();
    this.audioVolume = this.audioComponent.volume;
    if (this.sky.IsNight)
      return;
    this.audioComponent.volume = 0.0f;
  }

  protected void Update()
  {
    this.lerpTime = Mathf.Clamp01(this.lerpTime + (!this.sky.IsNight ? -1f : 1f) * Time.deltaTime / this.fadeTime);
    this.audioComponent.volume = Mathf.Lerp(0.0f, this.audioVolume, this.lerpTime);
  }
}
