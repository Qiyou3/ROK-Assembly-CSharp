// Decompiled with JetBrains decompiler
// Type: CodeHatch.Audio.CrossfadeClipInstance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Audio;
using UnityEngine;

#nullable disable
namespace CodeHatch.Audio
{
  public class CrossfadeClipInstance : IVolumeAware
  {
    public readonly AudioSource audioSource;
    public CrossfadeClip clip;
    public float beginFadeInTime;
    public float beginFadeOutTime;
    public float endFadeInTime;
    public float endFadeOutTime;

    public CrossfadeClipInstance(AudioSource audioSource) => this.audioSource = audioSource;

    public float BGMVolume => AudioVolumeController.BGMVolume;

    public float SFXVolume => AudioVolumeController.SFXVolume;

    public float AFXVolume => AudioVolumeController.AFXVolume;

    public float VFXVolume => AudioVolumeController.VFXVolume;

    public bool IsEmpty
    {
      get
      {
        return this.clip == null || !this.audioSource.isPlaying || this.clip.type == CrossfadeClip.Type.Loop && (double) this.endFadeOutTime < (double) Time.time;
      }
    }

    public float Volume
    {
      get
      {
        return Mathf.Clamp01(Mathf.InverseLerp(this.beginFadeInTime, this.endFadeInTime, Time.time)) * (1f - Mathf.Clamp01(Mathf.InverseLerp(this.beginFadeOutTime, this.endFadeOutTime, Time.time))) * this.SFXVolume;
      }
    }

    public void Update()
    {
      if (!this.IsEmpty)
      {
        this.audioSource.volume = this.Volume;
      }
      else
      {
        if (!(bool) (Object) this.audioSource)
          return;
        this.audioSource.Stop();
      }
    }

    public void Play(CrossfadeClip clipToPlay)
    {
      this.clip = clipToPlay;
      this.FadeInNow();
      this.PlayCurrentClip();
    }

    public void PlayCurrentClip()
    {
      this.beginFadeOutTime = float.PositiveInfinity;
      this.endFadeOutTime = float.PositiveInfinity;
      this.audioSource.clip = this.clip.clip;
      this.audioSource.volume = this.Volume * this.clip.volume;
      this.audioSource.minDistance = this.clip.minDistance;
      this.audioSource.maxDistance = this.clip.maxDistance;
      this.audioSource.loop = this.clip.type == CrossfadeClip.Type.Loop;
      this.audioSource.Play();
    }

    public void FadeInNow() => this.FadeInAtTime(Time.time);

    public void FadeInAtTime(float time)
    {
      this.beginFadeInTime = time;
      this.endFadeInTime = this.beginFadeInTime + this.clip.crossfadeInTime;
    }

    public void FadeOutNow() => this.FadeOutAtTime(Time.time);

    public void FadeOutAtTime(float time)
    {
      this.beginFadeOutTime = time;
      this.endFadeOutTime = this.beginFadeOutTime + this.clip.crossfadeOutTime;
    }
  }
}
