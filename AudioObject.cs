// Decompiled with JetBrains decompiler
// Type: AudioObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (AudioSource))]
[AddComponentMenu("ClockStone/Audio/AudioObject")]
public class AudioObject : RegisteredComponent
{
  private const float VOLUME_TRANSFORM_POWER = 1.6f;
  [NonSerialized]
  private AudioCategory _category;
  private AudioSubItem _subItemPrimary;
  private AudioSubItem _subItemSecondary;
  private AudioObject.AudioEventDelegate _completelyPlayedDelegate;
  private int _pauseCoroutineCounter;
  internal float _volumeExcludingCategory = 1f;
  private float _volumeFromPrimaryFade = 1f;
  private float _volumeFromSecondaryFade = 1f;
  internal float _volumeFromScriptCall = 1f;
  private bool _paused;
  private bool _applicationPaused;
  private AudioFader _primaryFader;
  private AudioFader _secondaryFader;
  private double _playTime = -1.0;
  private double _playStartTimeLocal = -1.0;
  private double _playStartTimeSystem = -1.0;
  private double _playScheduledTimeDsp = -1.0;
  private double _audioObjectTime;
  private bool _IsInactive = true;
  private bool _stopRequested;
  private bool _finishSequence;
  private int _loopSequenceCount;
  private bool _stopAfterFadeoutUserSetting;
  private bool _pauseWithFadeOutRequested;
  private double _dspTimeRemainingAtPause;
  private AudioController _audioController;
  internal bool _isCurrentPlaylistTrack;
  internal float _audioSource_MinDistance_Saved = 1f;
  internal float _audioSource_MaxDistance_Saved = 500f;
  internal int _lastChosenSubItemIndex = -1;
  private AudioSource _audioSource1;
  private AudioSource _audioSource2;
  private bool _primaryAudioSourcePaused;
  private bool _secondaryAudioSourcePaused;

  public string audioID { get; internal set; }

  public AudioCategory category
  {
    get => this._category;
    internal set => this._category = value;
  }

  public AudioSubItem subItem
  {
    get => this._subItemPrimary;
    internal set => this._subItemPrimary = value;
  }

  public bool isPlayedAsMusic { get; internal set; }

  public AudioItem audioItem => this.subItem != null ? this.subItem.item : (AudioItem) null;

  public AudioObject.AudioEventDelegate completelyPlayedDelegate
  {
    set => this._completelyPlayedDelegate = value;
    get => this._completelyPlayedDelegate;
  }

  public float volume
  {
    get => this._volumeWithCategory;
    set
    {
      float volumeFromCategory = this._volumeFromCategory;
      this._volumeExcludingCategory = (double) volumeFromCategory <= 0.0 ? value : value / volumeFromCategory;
      this._ApplyVolumeBoth();
    }
  }

  public float volumeItem
  {
    get
    {
      return (double) this._volumeFromScriptCall > 0.0 ? this._volumeExcludingCategory / this._volumeFromScriptCall : this._volumeExcludingCategory;
    }
    set
    {
      this._volumeExcludingCategory = value * this._volumeFromScriptCall;
      this._ApplyVolumeBoth();
    }
  }

  public float volumeTotal => this.volumeTotalWithoutFade * this._volumeFromPrimaryFade;

  public float volumeTotalWithoutFade
  {
    get
    {
      float totalWithoutFade = this._volumeWithCategory;
      AudioController audioController = this.category == null ? this._audioController : this.category.audioController;
      if ((UnityEngine.Object) audioController != (UnityEngine.Object) null)
      {
        totalWithoutFade *= audioController.Volume;
        if (audioController.soundMuted && !this.isPlayedAsMusic)
          totalWithoutFade = 0.0f;
      }
      return totalWithoutFade;
    }
  }

  public double playCalledAtTime => this._playTime;

  public double startedPlayingAtTime => this._playStartTimeSystem;

  public float timeUntilEnd => this.clipLength - this.audioTime;

  public double scheduledPlayingAtDspTime
  {
    get => this._playScheduledTimeDsp;
    set
    {
      this._playScheduledTimeDsp = value;
      this.primaryAudioSource.SetScheduledStartTime(this._playScheduledTimeDsp);
    }
  }

  public float clipLength
  {
    get
    {
      if ((double) this._stopClipAtTime > 0.0)
        return this._stopClipAtTime - this._startClipAtTime;
      return (UnityEngine.Object) this.primaryAudioSource.clip != (UnityEngine.Object) null ? this.primaryAudioSource.clip.length - this._startClipAtTime : 0.0f;
    }
  }

  public float audioTime
  {
    get => this.primaryAudioSource.time - this._startClipAtTime;
    set => this.primaryAudioSource.time = value + this._startClipAtTime;
  }

  public bool isFadingOut => this._primaryFader.isFadingOut;

  public bool isFadeOutComplete => this._primaryFader.isFadingOutComplete;

  public bool isFadingOutOrScheduled => this._primaryFader.isFadingOutOrScheduled;

  public bool isFadingIn => this._primaryFader.isFadingIn;

  public float pitch
  {
    get => this.primaryAudioSource.pitch;
    set => this.primaryAudioSource.pitch = value;
  }

  public float pan
  {
    get => this.primaryAudioSource.panStereo;
    set => this.primaryAudioSource.panStereo = value;
  }

  public double audioObjectTime => this._audioObjectTime;

  public bool stopAfterFadeOut
  {
    get => this._stopAfterFadeoutUserSetting;
    set => this._stopAfterFadeoutUserSetting = value;
  }

  public void FadeIn(float fadeInTime)
  {
    if (this._playStartTimeLocal > 0.0 && this._playStartTimeLocal - this.audioObjectTime > 0.0)
    {
      this._primaryFader.FadeIn(fadeInTime, this._playStartTimeLocal);
      this._UpdateFadeVolume();
    }
    else
    {
      this._primaryFader.FadeIn(fadeInTime, this.audioObjectTime, !this._shouldStopIfPrimaryFadedOut);
      this._UpdateFadeVolume();
    }
  }

  public void PlayScheduled(double dspTime) => this._PlayScheduled(dspTime);

  public void PlayAfter(string audioID, double deltaDspTime = 0, float volume = 1f, float startTime = 0)
  {
    AudioController.PlayAfter(audioID, this, deltaDspTime, volume, startTime);
  }

  public void PlayNow(string audioID, float delay = 0, float volume = 1f, float startTime = 0)
  {
    AudioItem audioItem = AudioController.GetAudioItem(audioID);
    if (audioItem == null)
      this.LogWarning<AudioObject>("Audio item with name '{0}' does not exist", (object) audioID);
    else
      this._audioController.PlayAudioItem(audioItem, volume, this.transform.position, this.transform.parent, delay, startTime, useExistingAudioObj: this, dspTime: 0.0);
  }

  public void Play(float delay = 0) => this._PlayDelayed(delay);

  public void Stop() => this.Stop(-1f);

  public void Stop(float fadeOutLength) => this.Stop(fadeOutLength, 0.0f);

  public void Stop(float fadeOutLength, float startToFadeTime)
  {
    if (this.IsPaused(false))
    {
      fadeOutLength = 0.0f;
      startToFadeTime = 0.0f;
    }
    if ((double) startToFadeTime > 0.0)
    {
      this.StartCoroutine(this._WaitForSecondsThenStop(startToFadeTime, fadeOutLength));
    }
    else
    {
      this._stopRequested = true;
      if ((double) fadeOutLength < 0.0)
        fadeOutLength = this.subItem == null ? 0.0f : this.subItem.FadeOut;
      if ((double) fadeOutLength == 0.0 && (double) startToFadeTime == 0.0)
      {
        this._Stop();
      }
      else
      {
        this.FadeOut(fadeOutLength, startToFadeTime);
        if (!this.IsSecondaryPlaying())
          return;
        this.SwitchAudioSources();
        this.FadeOut(fadeOutLength, startToFadeTime);
        this.SwitchAudioSources();
      }
    }
  }

  public void FinishSequence()
  {
    if (this._finishSequence)
      return;
    AudioItem audioItem = this.audioItem;
    if (audioItem == null)
      return;
    switch (audioItem.Loop)
    {
      case AudioItem.LoopMode.LoopSequence:
      case AudioItem.LoopMode.LoopSubitem | AudioItem.LoopMode.LoopSequence:
        this._finishSequence = true;
        break;
      case AudioItem.LoopMode.PlaySequenceAndLoopLast:
      case AudioItem.LoopMode.IntroLoopOutroSequence:
        this.primaryAudioSource.loop = false;
        this._finishSequence = true;
        break;
    }
  }

  [DebuggerHidden]
  private IEnumerator _WaitForSecondsThenStop(float startToFadeTime, float fadeOutLength)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new AudioObject.\u003C_WaitForSecondsThenStop\u003Ec__Iterator119()
    {
      startToFadeTime = startToFadeTime,
      fadeOutLength = fadeOutLength,
      \u003C\u0024\u003EstartToFadeTime = startToFadeTime,
      \u003C\u0024\u003EfadeOutLength = fadeOutLength,
      \u003C\u003Ef__this = this
    };
  }

  public void FadeOut(float fadeOutLength) => this.FadeOut(fadeOutLength, 0.0f);

  public void FadeOut(float fadeOutLength, float startToFadeTime)
  {
    if ((double) fadeOutLength < 0.0)
      fadeOutLength = this.subItem == null ? 0.0f : this.subItem.FadeOut;
    if ((double) fadeOutLength > 0.0 || (double) startToFadeTime > 0.0)
    {
      this._primaryFader.FadeOut(fadeOutLength, startToFadeTime);
    }
    else
    {
      if ((double) fadeOutLength != 0.0)
        return;
      if (this._shouldStopIfPrimaryFadedOut)
        this._Stop();
      else
        this._primaryFader.FadeOut(0.0f, startToFadeTime);
    }
  }

  public void Pause() => this.Pause(0.0f);

  public void Pause(float fadeOutTime)
  {
    if (this._paused)
      return;
    this._paused = true;
    if ((double) fadeOutTime > 0.0)
    {
      this._pauseWithFadeOutRequested = true;
      this.FadeOut(fadeOutTime);
      this.StartCoroutine(this._WaitThenPause(fadeOutTime, ++this._pauseCoroutineCounter));
    }
    else
      this._PauseNow();
  }

  private void _PauseNow()
  {
    if (this._playScheduledTimeDsp > 0.0)
    {
      this._dspTimeRemainingAtPause = this._playScheduledTimeDsp - AudioSettings.dspTime;
      this.scheduledPlayingAtDspTime = 9000000000.0;
    }
    this._PauseAudioSources();
    if (!this._pauseWithFadeOutRequested)
      return;
    this._pauseWithFadeOutRequested = false;
    this._primaryFader.Set0();
  }

  public void Unpause() => this.Unpause(0.0f);

  public void Unpause(float fadeInTime)
  {
    if (!this._paused)
      return;
    this._UnpauseNow();
    if ((double) fadeInTime > 0.0)
      this.FadeIn(fadeInTime);
    this._pauseWithFadeOutRequested = false;
  }

  private void _UnpauseNow()
  {
    this._paused = false;
    if ((bool) (UnityEngine.Object) this.secondaryAudioSource && this._secondaryAudioSourcePaused)
      this.secondaryAudioSource.Play();
    if (this._dspTimeRemainingAtPause > 0.0 && this._primaryAudioSourcePaused)
    {
      double time = AudioSettings.dspTime + this._dspTimeRemainingAtPause;
      this._playStartTimeSystem = AudioController.systemTime + this._dspTimeRemainingAtPause;
      this.primaryAudioSource.PlayScheduled(time);
      this.scheduledPlayingAtDspTime = time;
      this._dspTimeRemainingAtPause = -1.0;
    }
    else
    {
      if (!this._primaryAudioSourcePaused)
        return;
      this.primaryAudioSource.Play();
    }
  }

  [DebuggerHidden]
  private IEnumerator _WaitThenPause(float waitTime, int counter)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new AudioObject.\u003C_WaitThenPause\u003Ec__Iterator11A()
    {
      waitTime = waitTime,
      counter = counter,
      \u003C\u0024\u003EwaitTime = waitTime,
      \u003C\u0024\u003Ecounter = counter,
      \u003C\u003Ef__this = this
    };
  }

  private void _PauseAudioSources()
  {
    if (this.primaryAudioSource.isPlaying)
    {
      this._primaryAudioSourcePaused = true;
      this.primaryAudioSource.Pause();
    }
    else
      this._primaryAudioSourcePaused = false;
    if ((bool) (UnityEngine.Object) this.secondaryAudioSource && this.secondaryAudioSource.isPlaying)
    {
      this._secondaryAudioSourcePaused = true;
      this.secondaryAudioSource.Pause();
    }
    else
      this._secondaryAudioSourcePaused = false;
  }

  public bool IsPaused(bool returnTrueIfStillFadingOut = true)
  {
    return (returnTrueIfStillFadingOut || !this._pauseWithFadeOutRequested) && this._paused;
  }

  public bool IsPlaying() => this.IsPrimaryPlaying() || this.IsSecondaryPlaying();

  public bool IsPrimaryPlaying() => this.primaryAudioSource.isPlaying;

  public bool IsSecondaryPlaying()
  {
    return (UnityEngine.Object) this.secondaryAudioSource != (UnityEngine.Object) null && this.secondaryAudioSource.isPlaying;
  }

  public AudioSource primaryAudioSource => this._audioSource1;

  public AudioSource secondaryAudioSource => this._audioSource2;

  public void SwitchAudioSources()
  {
    if ((UnityEngine.Object) this._audioSource2 == (UnityEngine.Object) null)
      this._CreateSecondAudioSource();
    this._SwitchValues<AudioSource>(ref this._audioSource1, ref this._audioSource2);
    this._SwitchValues<AudioFader>(ref this._primaryFader, ref this._secondaryFader);
    this._SwitchValues<AudioSubItem>(ref this._subItemPrimary, ref this._subItemSecondary);
    this._SwitchValues<float>(ref this._volumeFromPrimaryFade, ref this._volumeFromSecondaryFade);
  }

  private void _SwitchValues<T>(ref T v1, ref T v2)
  {
    T obj = v1;
    v1 = v2;
    v2 = obj;
  }

  internal float _volumeFromCategory => this.category != null ? this.category.VolumeTotal : 1f;

  internal float _volumeWithCategory => this._volumeFromCategory * this._volumeExcludingCategory;

  private float _stopClipAtTime => this.subItem != null ? this.subItem.ClipStopTime : 0.0f;

  private float _startClipAtTime => this.subItem != null ? this.subItem.ClipStartTime : 0.0f;

  public override void Awake()
  {
    base.Awake();
    if (this._primaryFader == null)
      this._primaryFader = new AudioFader();
    else
      this._primaryFader.Set0();
    if (this._secondaryFader == null)
      this._secondaryFader = new AudioFader();
    else
      this._secondaryFader.Set0();
    if ((UnityEngine.Object) this._audioSource1 == (UnityEngine.Object) null)
      this._audioSource1 = this.GetComponent<AudioSource>();
    else if ((bool) (UnityEngine.Object) this._audioSource2 && (UnityEngine.Object) this._audioSource1 != (UnityEngine.Object) this.GetComponent<AudioSource>())
      this.SwitchAudioSources();
    this._Set0();
    this._audioController = SingletonMonoBehaviour<AudioController>.Instance;
  }

  private void _CreateSecondAudioSource()
  {
    this._audioSource2 = this.gameObject.AddComponent<AudioSource>();
    this._audioSource2.rolloffMode = this._audioSource1.rolloffMode;
    this._audioSource2.minDistance = this._audioSource1.minDistance;
    this._audioSource2.maxDistance = this._audioSource1.maxDistance;
    this._audioSource2.dopplerLevel = this._audioSource1.dopplerLevel;
    this._audioSource2.spread = this._audioSource1.spread;
    this._audioSource2.spatialBlend = this._audioSource1.spatialBlend;
    this._audioSource2.velocityUpdateMode = this._audioSource1.velocityUpdateMode;
    this._audioSource2.ignoreListenerVolume = this._audioSource1.ignoreListenerVolume;
    this._audioSource2.playOnAwake = false;
    this._audioSource2.priority = this._audioSource1.priority;
    this._audioSource2.bypassEffects = this._audioSource1.bypassEffects;
    this._audioSource2.ignoreListenerPause = this._audioSource1.ignoreListenerPause;
  }

  private void _Set0()
  {
    this._SetReferences0();
    this._audioObjectTime = 0.0;
    this.primaryAudioSource.playOnAwake = false;
    if ((bool) (UnityEngine.Object) this.secondaryAudioSource)
      this.secondaryAudioSource.playOnAwake = false;
    this._lastChosenSubItemIndex = -1;
    this._primaryFader.Set0();
    this._secondaryFader.Set0();
    this._playTime = -1.0;
    this._playStartTimeLocal = -1.0;
    this._playStartTimeSystem = -1.0;
    this._playScheduledTimeDsp = -1.0;
    this._volumeFromPrimaryFade = 1f;
    this._volumeFromSecondaryFade = 1f;
    this._volumeFromScriptCall = 1f;
    this._IsInactive = true;
    this._stopRequested = false;
    this._finishSequence = false;
    this._volumeExcludingCategory = 1f;
    this._paused = false;
    this._applicationPaused = false;
    this._isCurrentPlaylistTrack = false;
    this._loopSequenceCount = 0;
    this._stopAfterFadeoutUserSetting = false;
    this._pauseWithFadeOutRequested = false;
    this._dspTimeRemainingAtPause = -1.0;
    this._primaryAudioSourcePaused = false;
    this._secondaryAudioSourcePaused = false;
  }

  private void _SetReferences0()
  {
    this._audioController = (AudioController) null;
    this.primaryAudioSource.clip = (AudioClip) null;
    if ((UnityEngine.Object) this.secondaryAudioSource != (UnityEngine.Object) null)
    {
      this.secondaryAudioSource.playOnAwake = false;
      this.secondaryAudioSource.clip = (AudioClip) null;
    }
    this.subItem = (AudioSubItem) null;
    this.category = (AudioCategory) null;
    this._completelyPlayedDelegate = (AudioObject.AudioEventDelegate) null;
  }

  private void _PlayScheduled(double dspTime)
  {
    if (!(bool) (UnityEngine.Object) this.primaryAudioSource.clip)
    {
      this.LogError<AudioObject>("audio.clip == null in {0}", (object) this.gameObject.name);
    }
    else
    {
      this._playScheduledTimeDsp = dspTime;
      double num = dspTime - AudioSettings.dspTime;
      this._playStartTimeLocal = num + this.audioObjectTime;
      this._playStartTimeSystem = num + AudioController.systemTime;
      this.primaryAudioSource.PlayScheduled(dspTime);
      this._OnPlay();
    }
  }

  private void _PlayDelayed(float delay)
  {
    if (!this.primaryAudioSource.isActiveAndEnabled)
      return;
    if (!(bool) (UnityEngine.Object) this.primaryAudioSource.clip)
    {
      this.LogError<AudioObject>("audio.clip == null in {0}", (object) this.gameObject.name);
    }
    else
    {
      if ((double) delay <= 1.0 / 1000.0)
        this.primaryAudioSource.Play();
      else
        this.primaryAudioSource.PlayDelayed(delay);
      this._playScheduledTimeDsp = -1.0;
      this._playStartTimeLocal = this.audioObjectTime + (double) delay;
      this._playStartTimeSystem = AudioController.systemTime + (double) delay;
      this._OnPlay();
    }
  }

  private void _OnPlay()
  {
    this._IsInactive = false;
    this._playTime = this.audioObjectTime;
    this._paused = false;
    this._primaryAudioSourcePaused = false;
    this._secondaryAudioSourcePaused = false;
    this._primaryFader.Set0();
  }

  private void _Stop()
  {
    this._primaryFader.Set0();
    this._secondaryFader.Set0();
    this.primaryAudioSource.Stop();
    if ((bool) (UnityEngine.Object) this.secondaryAudioSource)
      this.secondaryAudioSource.Stop();
    this._paused = false;
    this._primaryAudioSourcePaused = false;
    this._secondaryAudioSourcePaused = false;
  }

  public void Update()
  {
    if (this._IsInactive)
      return;
    if (!this.IsPaused(false))
    {
      this._audioObjectTime += AudioController.systemDeltaTime;
      this._primaryFader.time = this._audioObjectTime;
      this._secondaryFader.time = this._audioObjectTime;
    }
    if (this._playScheduledTimeDsp > 0.0 && this._audioObjectTime > this._playStartTimeLocal)
      this._playScheduledTimeDsp = -1.0;
    if (!this._paused && !this._applicationPaused)
    {
      bool flag1 = this.IsPrimaryPlaying();
      bool flag2 = this.IsSecondaryPlaying();
      if (!flag1 && !flag2)
      {
        bool flag3 = true;
        if (!this._stopRequested && flag3 && this.completelyPlayedDelegate != null)
        {
          this.completelyPlayedDelegate(this);
          flag3 = !this.IsPlaying();
        }
        if (this._isCurrentPlaylistTrack && (bool) (UnityEngine.Object) SingletonMonoBehaviour<AudioController>.DoesInstanceExist())
          SingletonMonoBehaviour<AudioController>.Instance._NotifyPlaylistTrackCompleteleyPlayed(this);
        if (flag3)
        {
          this.DestroyAudioObject();
          return;
        }
      }
      else
      {
        if (!this._stopRequested && this._IsAudioLoopSequenceMode() && !this.IsSecondaryPlaying() && (double) this.timeUntilEnd < 1.0 + (double) Mathf.Max(0.0f, this.audioItem.loopSequenceOverlap) && this._playScheduledTimeDsp < 0.0)
          this._ScheduleNextInLoopSequence();
        if (!this.primaryAudioSource.loop)
        {
          if (this._isCurrentPlaylistTrack && (bool) (UnityEngine.Object) this._audioController && this._audioController.crossfadePlaylist && (double) this.audioTime > (double) this.clipLength - (double) this._audioController.musicCrossFadeTime_Out)
          {
            if ((bool) (UnityEngine.Object) SingletonMonoBehaviour<AudioController>.DoesInstanceExist())
              SingletonMonoBehaviour<AudioController>.Instance._NotifyPlaylistTrackCompleteleyPlayed(this);
          }
          else
          {
            this._StartFadeOutIfNecessary();
            if (flag2)
            {
              this.SwitchAudioSources();
              this._StartFadeOutIfNecessary();
              this.SwitchAudioSources();
            }
          }
        }
      }
    }
    this._UpdateFadeVolume();
  }

  private void _StartFadeOutIfNecessary()
  {
    if (this.subItem == null)
    {
      this.LogWarning<AudioObject>("subItem == null");
    }
    else
    {
      float audioTime = this.audioTime;
      if (this.isFadingOutOrScheduled || (double) this.subItem.FadeOut <= 0.0 || (double) audioTime <= (double) this.clipLength - (double) this.subItem.FadeOut)
        return;
      this.FadeOut(this.subItem.FadeOut);
    }
  }

  private bool _IsAudioLoopSequenceMode()
  {
    AudioItem audioItem = this.audioItem;
    if (audioItem != null)
    {
      switch (audioItem.Loop)
      {
        case AudioItem.LoopMode.LoopSequence:
        case AudioItem.LoopMode.LoopSubitem | AudioItem.LoopMode.LoopSequence:
          return true;
        case AudioItem.LoopMode.PlaySequenceAndLoopLast:
        case AudioItem.LoopMode.IntroLoopOutroSequence:
          return !this.primaryAudioSource.loop;
      }
    }
    return false;
  }

  private bool _ScheduleNextInLoopSequence()
  {
    if (this.audioItem == null)
      return false;
    int num = this.audioItem.loopSequenceCount <= 0 ? this.audioItem.subItems.Length : this.audioItem.loopSequenceCount;
    if (this._finishSequence && (this.audioItem.Loop != AudioItem.LoopMode.IntroLoopOutroSequence || this._loopSequenceCount <= num - 3 || this._loopSequenceCount >= num - 1) || this.audioItem.loopSequenceCount > 0 && this.audioItem.loopSequenceCount <= this._loopSequenceCount + 1)
      return false;
    double dspTime = AudioSettings.dspTime + (double) this.timeUntilEnd + (double) this._GetRandomLoopSequenceDelay(this.audioItem);
    AudioItem audioItem = this.audioItem;
    this.SwitchAudioSources();
    this._audioController.PlayAudioItem(audioItem, this._volumeFromScriptCall, Vector3.zero, delay: 0.0f, startTime: 0.0f, useExistingAudioObj: this, dspTime: dspTime);
    ++this._loopSequenceCount;
    if (this.audioItem.Loop == AudioItem.LoopMode.PlaySequenceAndLoopLast || this.audioItem.Loop == AudioItem.LoopMode.IntroLoopOutroSequence)
    {
      if (this.audioItem.Loop == AudioItem.LoopMode.IntroLoopOutroSequence)
      {
        if (!this._finishSequence && num <= this._loopSequenceCount + 2)
          this.primaryAudioSource.loop = true;
      }
      else if (num <= this._loopSequenceCount + 1)
        this.primaryAudioSource.loop = true;
    }
    return true;
  }

  private void _UpdateFadeVolume()
  {
    bool finishedFadeOut;
    float num1 = this._EqualizePowerForCrossfading(this._primaryFader.Get(out finishedFadeOut));
    if (finishedFadeOut)
    {
      if (this._stopRequested)
      {
        this._Stop();
        return;
      }
      if (!this._IsAudioLoopSequenceMode())
      {
        if (!this._shouldStopIfPrimaryFadedOut)
          return;
        this._Stop();
        return;
      }
    }
    if ((double) num1 != (double) this._volumeFromPrimaryFade)
    {
      this._volumeFromPrimaryFade = num1;
      this._ApplyVolumePrimary();
    }
    if (!((UnityEngine.Object) this._audioSource2 != (UnityEngine.Object) null))
      return;
    float num2 = this._EqualizePowerForCrossfading(this._secondaryFader.Get(out finishedFadeOut));
    if (finishedFadeOut)
    {
      this._audioSource2.Stop();
    }
    else
    {
      if ((double) num2 == (double) this._volumeFromSecondaryFade)
        return;
      this._volumeFromSecondaryFade = num2;
      this._ApplyVolumeSecondary();
    }
  }

  private float _EqualizePowerForCrossfading(float v)
  {
    return !this._audioController.EqualPowerCrossfade ? v : AudioObject.InverseTransformVolume(Mathf.Sin((float) ((double) v * 3.1415927410125732 * 0.5)));
  }

  private bool _shouldStopIfPrimaryFadedOut
  {
    get => this._stopAfterFadeoutUserSetting && !this._pauseWithFadeOutRequested;
  }

  public void OnApplicationPause(bool b) => this.SetApplicationPaused(b);

  private void SetApplicationPaused(bool isPaused) => this._applicationPaused = isPaused;

  public void DestroyAudioObject()
  {
    if (this.IsPlaying())
      this._Stop();
    ObjectPoolController.Destroy(this.gameObject);
    this._IsInactive = true;
  }

  public static float TransformVolume(float volume) => Mathf.Pow(volume, 1.6f);

  public static float InverseTransformVolume(float volume) => Mathf.Pow(volume, 0.625f);

  public static float TransformPitch(float pitchSemiTones) => Mathf.Pow(2f, pitchSemiTones / 12f);

  public static float InverseTransformPitch(float pitch)
  {
    return (float) ((double) Mathf.Log(pitch) / (double) Mathf.Log(2f) * 12.0);
  }

  internal void _ApplyVolumeBoth()
  {
    float totalWithoutFade = this.volumeTotalWithoutFade;
    this.primaryAudioSource.volume = AudioObject.TransformVolume(totalWithoutFade * this._volumeFromPrimaryFade);
    if (!(bool) (UnityEngine.Object) this.secondaryAudioSource)
      return;
    this.secondaryAudioSource.volume = AudioObject.TransformVolume(totalWithoutFade * this._volumeFromSecondaryFade);
  }

  internal void _ApplyVolumePrimary()
  {
    this.primaryAudioSource.volume = AudioObject.TransformVolume(this.volumeTotalWithoutFade * this._volumeFromPrimaryFade);
  }

  internal void _ApplyVolumeSecondary()
  {
    if (!(bool) (UnityEngine.Object) this.secondaryAudioSource)
      return;
    this.secondaryAudioSource.volume = AudioObject.TransformVolume(this.volumeTotalWithoutFade * this._volumeFromSecondaryFade);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    AudioItem audioItem = this.audioItem;
    if (audioItem != null && audioItem.overrideAudioSourceSettings)
      this._RestoreAudioSourceSettings();
    this._SetReferences0();
  }

  private void _RestoreAudioSourceSettings()
  {
    this.primaryAudioSource.minDistance = this._audioSource_MinDistance_Saved;
    this.primaryAudioSource.maxDistance = this._audioSource_MaxDistance_Saved;
    if (!((UnityEngine.Object) this.secondaryAudioSource != (UnityEngine.Object) null))
      return;
    this.secondaryAudioSource.minDistance = this._audioSource_MinDistance_Saved;
    this.secondaryAudioSource.maxDistance = this._audioSource_MaxDistance_Saved;
  }

  public bool DoesBelongToCategory(string categoryName)
  {
    for (AudioCategory audioCategory = this.category; audioCategory != null; audioCategory = audioCategory.parentCategory)
    {
      if (audioCategory.Name == categoryName)
        return true;
    }
    return false;
  }

  private float _GetRandomLoopSequenceDelay(AudioItem audioItem)
  {
    float loopSequenceDelay = -audioItem.loopSequenceOverlap;
    if ((double) audioItem.loopSequenceRandomDelay > 0.0)
      loopSequenceDelay += UnityEngine.Random.Range(0.0f, audioItem.loopSequenceRandomDelay);
    return loopSequenceDelay;
  }

  public delegate void AudioEventDelegate(AudioObject audioObject);
}
