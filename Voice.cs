// Decompiled with JetBrains decompiler
// Type: CodeHatch.Audio.Voice
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Engine.Core.Cache;
using UnityEngine;

#nullable disable
namespace CodeHatch.Audio
{
  public class Voice : EntityBehaviour
  {
    public CrossfadeClip[] clips;
    public AudioSource audioSourceA;
    public AudioSource audioSourceB;
    private CrossfadeClipInstance _current;
    private CrossfadeClipInstance _next;
    private CrossfadeClip _currentLoopClip;

    private CrossfadeClipInstance Current
    {
      get
      {
        if (this._current == null)
          this._current = new CrossfadeClipInstance(this.audioSourceA);
        return this._current;
      }
    }

    private CrossfadeClipInstance Next
    {
      get
      {
        if (this._next == null)
          this._next = new CrossfadeClipInstance(this.audioSourceB);
        return this._next;
      }
    }

    private void SwapCurrentAndNext()
    {
      CrossfadeClipInstance current = this._current;
      this._current = this._next;
      this._next = current;
    }

    public void PlayClip(AudioClip audioClip) => this.PlayClip(audioClip.name);

    public void PlayClip(string clipName)
    {
      CrossfadeClip clip = this.FindClip(clipName);
      if (clip == null)
        return;
      this.PlayClip(clip);
    }

    public CrossfadeClip FindClip(string clipName)
    {
      foreach (CrossfadeClip clip in this.clips)
      {
        if (clip.Name == clipName)
          return clip;
      }
      return (CrossfadeClip) null;
    }

    public void PlayClip(CrossfadeClip clip)
    {
      if (clip == null)
        return;
      this.enabled = true;
      this.CheckForSwap();
      if (this.Current.IsEmpty)
      {
        this.Current.Play(clip);
      }
      else
      {
        this.Current.FadeOutNow();
        this.Next.Play(clip);
      }
      if (clip.type != CrossfadeClip.Type.Loop)
        return;
      this._currentLoopClip = clip;
    }

    public void StopClips()
    {
      if (!this.Current.IsEmpty)
        this.Next.FadeOutNow();
      if (this.Next.IsEmpty)
        return;
      this.Next.FadeOutNow();
    }

    public void SetLoop(AudioClip audioClip) => this.SetLoop(audioClip.name);

    public void SetLoop(string loopName)
    {
      this.enabled = true;
      CrossfadeClip loop = this.FindLoop(loopName);
      if (loop == null)
        return;
      this._currentLoopClip = loop;
    }

    public CrossfadeClip FindLoop(string loopName)
    {
      foreach (CrossfadeClip clip in this.clips)
      {
        if (clip.type == CrossfadeClip.Type.Loop && !(clip.Name != loopName))
          return clip;
      }
      return (CrossfadeClip) null;
    }

    public void StopLoop()
    {
      if (this._currentLoopClip == null)
        return;
      if (!this.Current.IsEmpty && this.Current.clip.type == CrossfadeClip.Type.Loop)
        this.Next.FadeOutNow();
      if (!this.Next.IsEmpty && this.Next.clip.type == CrossfadeClip.Type.Loop)
        this.Next.FadeOutNow();
      this._currentLoopClip = (CrossfadeClip) null;
    }

    public void Start()
    {
      this.enabled = false;
      if (this.Entity.IsLocallyOwned)
        return;
      this.Current.audioSource.dopplerLevel = 0.0f;
      this.Next.audioSource.dopplerLevel = 0.0f;
    }

    public void Update()
    {
      this.CheckForSwap();
      this.Current.Update();
      this.Next.Update();
      if (this.Next.IsEmpty && this._currentLoopClip != null)
      {
        if (this.Current.IsEmpty)
          this.Current.Play(this._currentLoopClip);
        else if (this.Current.clip != this._currentLoopClip)
        {
          this.Next.clip = this._currentLoopClip;
          if (this.Current.clip.type == CrossfadeClip.Type.Once)
          {
            this.Next.FadeInAtTime(this.Current.beginFadeInTime + this.Current.clip.clip.length);
          }
          else
          {
            this.Current.FadeOutNow();
            this.Next.FadeInNow();
          }
          this.Next.PlayCurrentClip();
        }
      }
      this.DisableIfNothingIsPlaying();
    }

    private void CheckForSwap()
    {
      if (!this.Current.IsEmpty || this.Next.IsEmpty)
        return;
      this.SwapCurrentAndNext();
    }

    private void DisableIfNothingIsPlaying()
    {
      if (!this.Current.IsEmpty || !this.Next.IsEmpty)
        return;
      this.enabled = false;
    }
  }
}
