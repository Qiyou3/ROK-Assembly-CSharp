// Decompiled with JetBrains decompiler
// Type: AudioController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using CodeHatch.Common;
using CodeHatch.Engine.Audio;
using CodeHatch.Engine.Core.Cache;
using CodeHatch.Engine.Networking;
using CodeHatch.Networking;
using CodeHatch.Networking.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
[AddComponentMenu("ClockStone/Audio/AudioController")]
public class AudioController : SingletonMonoBehaviour<AudioController>
{
  public const string AUDIO_TOOLKIT_VERSION = "7.0";
  public GameObject AudioObjectPrefab;
  public bool Persistent;
  public bool UnloadAudioClipsOnDestroy;
  public bool UsePooledAudioObjects = true;
  public bool PlayWithZeroVolume;
  public bool EqualPowerCrossfade;
  public float musicCrossFadeTime;
  public bool specifyCrossFadeInAndOutSeperately;
  [SerializeField]
  private float _musicCrossFadeTime_In;
  [SerializeField]
  private float _musicCrossFadeTime_Out;
  public AudioCategory[] AudioCategories;
  public string[] musicPlaylist;
  public bool loopPlaylist;
  public bool shufflePlaylist;
  public bool crossfadePlaylist;
  public float delayBetweenPlaylistTracks = 1f;
  protected static PoolableReference<AudioObject> _currentMusicReference = new PoolableReference<AudioObject>();
  protected AudioListener _currentAudioListener;
  private bool _musicEnabled = true;
  private bool _soundMuted;
  private bool _categoriesValidated;
  [SerializeField]
  private bool _isAdditionalAudioController;
  [SerializeField]
  private bool _audioDisabled;
  private Dictionary<string, AudioItem> _audioItems;
  private static List<int> _playlistPlayed;
  private static bool _isPlaylistPlaying = false;
  [SerializeField]
  private float _volume = 1f;
  private static double _systemTime;
  private static double _lastSystemTime = -1.0;
  private static double _systemDeltaTime = -1.0;
  private List<AudioController> _additionalAudioControllers;
  public AudioController_CurrentInspectorSelection _currentInspectorSelection = new AudioController_CurrentInspectorSelection();

  public AudioController()
  {
    SingletonMonoBehaviour<AudioController>.SetSingletonType(typeof (AudioController));
  }

  public bool DisableAudio
  {
    set
    {
      if (value == this._audioDisabled)
        return;
      if (!value)
        ;
      this._audioDisabled = value;
    }
    get => this._audioDisabled;
  }

  public bool isAdditionalAudioController
  {
    get => this._isAdditionalAudioController;
    set => this._isAdditionalAudioController = value;
  }

  public float Volume
  {
    get => this._volume;
    set
    {
      if ((double) value == (double) this._volume)
        return;
      this._volume = value;
      this._ApplyVolumeChange();
    }
  }

  public bool musicEnabled
  {
    get => this._musicEnabled;
    set
    {
      if (this._musicEnabled == value)
        return;
      this._musicEnabled = value;
      if (!(bool) (UnityEngine.Object) AudioController._currentMusic)
        return;
      if (value)
      {
        if (!AudioController._currentMusic.IsPaused())
          return;
        AudioController._currentMusic.Play(0.0f);
      }
      else
        AudioController._currentMusic.Pause();
    }
  }

  public bool soundMuted
  {
    get => this._soundMuted;
    set
    {
      this._soundMuted = value;
      this._ApplyVolumeChange();
    }
  }

  public float musicCrossFadeTime_In
  {
    get
    {
      return this.specifyCrossFadeInAndOutSeperately ? this._musicCrossFadeTime_In : this.musicCrossFadeTime;
    }
    set => this._musicCrossFadeTime_In = value;
  }

  public float musicCrossFadeTime_Out
  {
    get
    {
      return this.specifyCrossFadeInAndOutSeperately ? this._musicCrossFadeTime_Out : this.musicCrossFadeTime;
    }
    set => this._musicCrossFadeTime_Out = value;
  }

  public static double systemTime => AudioController._systemTime;

  public static double systemDeltaTime => AudioController._systemDeltaTime;

  public static AudioObject PlayMusic(string audioID, float volume, float delay, float startTime = 0)
  {
    if (Player.IsLocalDedi)
      return (AudioObject) null;
    AudioController._isPlaylistPlaying = false;
    return SingletonMonoBehaviour<AudioController>.Instance._PlayMusic(audioID, volume, delay, startTime);
  }

  public static AudioObject PlayMusic(string audioID)
  {
    return AudioController.PlayMusic(audioID, 1f, 0.0f, 0.0f);
  }

  public static AudioObject PlayMusic(string audioID, float volume)
  {
    return AudioController.PlayMusic(audioID, volume, 0.0f, 0.0f);
  }

  public static AudioObject PlayMusic(
    string audioID,
    Vector3 worldPosition,
    Transform parentObj = null,
    float volume = 1,
    float delay = 0,
    float startTime = 0)
  {
    if (Player.IsLocalDedi)
      return (AudioObject) null;
    AudioController._isPlaylistPlaying = false;
    return SingletonMonoBehaviour<AudioController>.Instance._PlayMusic(audioID, worldPosition, parentObj, volume, delay, startTime);
  }

  public static AudioObject PlayMusic(
    string audioID,
    Transform parentObj,
    float volume = 1,
    float delay = 0,
    float startTime = 0)
  {
    if (Player.IsLocalDedi)
      return (AudioObject) null;
    AudioController._isPlaylistPlaying = false;
    return SingletonMonoBehaviour<AudioController>.Instance._PlayMusic(audioID, parentObj.position, parentObj, volume, delay, startTime);
  }

  public static bool StopMusic()
  {
    return SingletonMonoBehaviour<AudioController>.Instance._StopMusic(0.0f);
  }

  public static bool StopMusic(float fadeOut)
  {
    return SingletonMonoBehaviour<AudioController>.Instance._StopMusic(fadeOut);
  }

  public static bool PauseMusic(float fadeOut = 0)
  {
    return SingletonMonoBehaviour<AudioController>.Instance._PauseMusic(fadeOut);
  }

  public static bool IsMusicPaused()
  {
    return (UnityEngine.Object) AudioController._currentMusic != (UnityEngine.Object) null && AudioController._currentMusic.IsPaused();
  }

  public static bool UnpauseMusic(float fadeIn = 0)
  {
    if (!SingletonMonoBehaviour<AudioController>.Instance._musicEnabled || !((UnityEngine.Object) AudioController._currentMusic != (UnityEngine.Object) null) || !AudioController._currentMusic.IsPaused())
      return false;
    AudioController._currentMusic.Unpause(fadeIn);
    return true;
  }

  public static int EnqueueMusic(string audioID)
  {
    return SingletonMonoBehaviour<AudioController>.Instance._EnqueueMusic(audioID);
  }

  public static string[] GetMusicPlaylist()
  {
    string[] destinationArray = new string[SingletonMonoBehaviour<AudioController>.Instance.musicPlaylist == null ? 0 : SingletonMonoBehaviour<AudioController>.Instance.musicPlaylist.Length];
    if (destinationArray.Length > 0)
      Array.Copy((Array) SingletonMonoBehaviour<AudioController>.Instance.musicPlaylist, (Array) destinationArray, destinationArray.Length);
    return destinationArray;
  }

  public static void SetMusicPlaylist(string[] playlist)
  {
    string[] destinationArray = new string[playlist == null ? 0 : playlist.Length];
    if (destinationArray.Length > 0)
      Array.Copy((Array) playlist, (Array) destinationArray, destinationArray.Length);
    SingletonMonoBehaviour<AudioController>.Instance.musicPlaylist = destinationArray;
  }

  public static AudioObject PlayMusicPlaylist()
  {
    return SingletonMonoBehaviour<AudioController>.Instance._PlayMusicPlaylist();
  }

  public static AudioObject PlayNextMusicOnPlaylist()
  {
    return AudioController.IsPlaylistPlaying() ? SingletonMonoBehaviour<AudioController>.Instance._PlayNextMusicOnPlaylist(0.0f) : (AudioObject) null;
  }

  public static AudioObject PlayPreviousMusicOnPlaylist()
  {
    return AudioController.IsPlaylistPlaying() ? SingletonMonoBehaviour<AudioController>.Instance._PlayPreviousMusicOnPlaylist(0.0f) : (AudioObject) null;
  }

  public static bool IsPlaylistPlaying()
  {
    if (!AudioController._isPlaylistPlaying)
      return false;
    if ((bool) (UnityEngine.Object) AudioController._currentMusic)
      return true;
    AudioController._isPlaylistPlaying = false;
    return false;
  }

  public static void ClearPlaylist()
  {
    SingletonMonoBehaviour<AudioController>.Instance.musicPlaylist = (string[]) null;
  }

  public static AudioObject Play(string audioID)
  {
    AudioListener currentAudioListener = AudioController.GetCurrentAudioListener();
    if (!((UnityEngine.Object) currentAudioListener == (UnityEngine.Object) null))
      return AudioController.Play(audioID, currentAudioListener.transform.position + currentAudioListener.transform.forward, (Transform) null, 1f, 0.0f, 0.0f);
    Logger.Warning("No AudioListener found in the scene");
    return (AudioObject) null;
  }

  public static void Play(string audioId, Entity transform, params Player[] players)
  {
    if (audioId.IsNullEmptyOrWhite())
      return;
    if (players == null || players.Length == 0)
    {
      if ((UnityEngine.Object) transform != (UnityEngine.Object) null)
      {
        PlayAudioEvent theEvent = new PlayAudioEvent(audioId, transform);
        theEvent.Recipients = EventReceiverController.GetEventRecievers(transform);
        theEvent.SkipServerHandle = true;
        EventManager.CallEvent((BaseEvent) theEvent);
      }
      else
      {
        PlayAudioEvent theEvent = new PlayAudioEvent(audioId);
        theEvent.SkipServerHandle = true;
        EventManager.CallEvent((BaseEvent) theEvent);
      }
    }
    else
    {
      PlayAudioEvent theEvent = new PlayAudioEvent(audioId, transform);
      theEvent.Recipients = ((IEnumerable<Player>) players).ToList<Player>();
      theEvent.SkipServerHandle = true;
      EventManager.CallEvent((BaseEvent) theEvent);
    }
  }

  private void OnPlay(PlayAudioEvent theEvent)
  {
    if (theEvent.Cancelled)
      return;
    if (theEvent.HasEntity)
      AudioController.Play(theEvent.AudioItem, theEvent.TheEntity.MainTransform);
    else
      AudioController.Play(theEvent.AudioItem);
  }

  public static AudioObject Play(string audioID, float volume, float delay = 0, float startTime = 0)
  {
    AudioListener currentAudioListener = AudioController.GetCurrentAudioListener();
    if (!((UnityEngine.Object) currentAudioListener == (UnityEngine.Object) null))
      return AudioController.Play(audioID, currentAudioListener.transform.position + currentAudioListener.transform.forward, (Transform) null, volume, delay, startTime);
    Logger.Warning("No AudioListener found in the scene");
    return (AudioObject) null;
  }

  public static AudioObject Play(string audioID, Transform parentObj)
  {
    return AudioController.Play(audioID, parentObj.position, parentObj, 1f, 0.0f, 0.0f);
  }

  public static AudioObject Play(
    string audioID,
    Transform parentObj,
    float volume,
    float delay = 0,
    float startTime = 0)
  {
    return AudioController.Play(audioID, parentObj.position, parentObj, volume, delay, startTime);
  }

  public static void PlayIfNotPlaying(
    string audioID,
    Transform parentObj,
    float volume = 1,
    float delay = 0,
    float startTime = 0)
  {
    if (AudioController.IsPlaying(audioID))
      return;
    AudioController.Play(audioID, parentObj.position, parentObj, volume, delay, startTime);
  }

  public static AudioObject Play(string audioID, Vector3 worldPosition, Transform parentObj = null)
  {
    return SingletonMonoBehaviour<AudioController>.Instance._PlayAsSound(audioID, 1f, worldPosition, parentObj, 0.0f, 0.0f, false, 0.0);
  }

  public static AudioObject PlaySeeded(
    string audioID,
    int seed,
    Vector3 worldPosition,
    Transform parentObj = null)
  {
    return SingletonMonoBehaviour<AudioController>.Instance._PlayAsSound(audioID, 1f, worldPosition, parentObj, 0.0f, 0.0f, false, 0.0, seed: seed);
  }

  public static AudioObject Play(
    string audioID,
    Vector3 worldPosition,
    Transform parentObj,
    float volume,
    float delay = 0,
    float startTime = 0)
  {
    return Player.IsLocalDedi ? (AudioObject) null : SingletonMonoBehaviour<AudioController>.Instance._PlayAsSound(audioID, volume, worldPosition, parentObj, delay, startTime, false, 0.0);
  }

  public static AudioObject PlayScheduled(
    string audioID,
    double dspTime,
    Vector3 worldPosition,
    Transform parentObj = null,
    float volume = 1f,
    float startTime = 0)
  {
    return Player.IsLocalDedi ? (AudioObject) null : SingletonMonoBehaviour<AudioController>.Instance._PlayAsSound(audioID, volume, worldPosition, parentObj, 0.0f, startTime, false, dspTime);
  }

  public static AudioObject PlayAfter(
    string audioID,
    AudioObject playingAudio,
    double deltaDspTime = 0,
    float volume = 1f,
    float startTime = 0)
  {
    if (Player.IsLocalDedi)
      return (AudioObject) null;
    double dspTime1 = AudioSettings.dspTime;
    if (playingAudio.IsPlaying())
      dspTime1 += (double) playingAudio.timeUntilEnd;
    double dspTime2 = dspTime1 + deltaDspTime;
    return AudioController.PlayScheduled(audioID, dspTime2, playingAudio.transform.position, playingAudio.transform.parent, volume, startTime);
  }

  public static bool Stop(string audioID, float fadeOutLength)
  {
    if (Player.IsLocalDedi || (UnityEngine.Object) SingletonMonoBehaviour<AudioController>.Instance == (UnityEngine.Object) null)
      return false;
    if (SingletonMonoBehaviour<AudioController>.Instance._GetAudioItem(audioID) == null)
    {
      Logger.Warning<AudioController>("Audio item with name '" + audioID + "' does not exist");
      return false;
    }
    List<AudioObject> playingAudioObjects = AudioController.GetPlayingAudioObjects(audioID);
    for (int index = 0; index < playingAudioObjects.Count; ++index)
    {
      AudioObject audioObject = playingAudioObjects[index];
      if ((double) fadeOutLength < 0.0)
        audioObject.Stop();
      else
        audioObject.Stop(fadeOutLength);
    }
    return playingAudioObjects.Count > 0;
  }

  public static bool Stop(string audioID) => AudioController.Stop(audioID, -1f);

  public static void StopAll(float fadeOutLength)
  {
    if (Player.IsLocalDedi)
      return;
    SingletonMonoBehaviour<AudioController>.Instance._StopMusic(fadeOutLength);
    List<AudioObject> playingAudioObjects = AudioController.GetPlayingAudioObjects();
    for (int index = 0; index < playingAudioObjects.Count; ++index)
    {
      AudioObject audioObject = playingAudioObjects[index];
      if ((UnityEngine.Object) audioObject != (UnityEngine.Object) null)
        audioObject.Stop(fadeOutLength);
    }
  }

  public static void StopAll() => AudioController.StopAll(-1f);

  public static void PauseAll(float fadeOutLength = 0)
  {
    if (Player.IsLocalDedi)
      return;
    SingletonMonoBehaviour<AudioController>.Instance._PauseMusic(fadeOutLength);
    List<AudioObject> playingAudioObjects = AudioController.GetPlayingAudioObjects();
    for (int index = 0; index < playingAudioObjects.Count; ++index)
    {
      AudioObject audioObject = playingAudioObjects[index];
      if ((UnityEngine.Object) audioObject != (UnityEngine.Object) null)
        audioObject.Pause(fadeOutLength);
    }
  }

  public static void UnpauseAll(float fadeInLength = 0)
  {
    if (Player.IsLocalDedi)
      return;
    AudioController.UnpauseMusic(fadeInLength);
    List<AudioObject> playingAudioObjects = AudioController.GetPlayingAudioObjects(true);
    AudioController instance = SingletonMonoBehaviour<AudioController>.Instance;
    for (int index = 0; index < playingAudioObjects.Count; ++index)
    {
      AudioObject audioObject = playingAudioObjects[index];
      if ((UnityEngine.Object) audioObject != (UnityEngine.Object) null && audioObject.IsPaused() && (instance.musicEnabled || !((UnityEngine.Object) AudioController._currentMusic == (UnityEngine.Object) audioObject)))
        audioObject.Unpause(fadeInLength);
    }
  }

  public static void PauseCategory(string categoryName, float fadeOutLength = 0)
  {
    if (Player.IsLocalDedi)
      return;
    if ((UnityEngine.Object) AudioController._currentMusic != (UnityEngine.Object) null && AudioController._currentMusic.category.Name == categoryName)
      AudioController.PauseMusic(fadeOutLength);
    List<AudioObject> objectsInCategory = AudioController.GetPlayingAudioObjectsInCategory(categoryName);
    for (int index = 0; index < objectsInCategory.Count; ++index)
      objectsInCategory[index].Pause(fadeOutLength);
  }

  public static void UnpauseCategory(string categoryName, float fadeInLength = 0)
  {
    if (Player.IsLocalDedi)
      return;
    if ((UnityEngine.Object) AudioController._currentMusic != (UnityEngine.Object) null && AudioController._currentMusic.category.Name == categoryName)
      AudioController.UnpauseMusic(fadeInLength);
    List<AudioObject> objectsInCategory = AudioController.GetPlayingAudioObjectsInCategory(categoryName, true);
    for (int index = 0; index < objectsInCategory.Count; ++index)
    {
      AudioObject audioObject = objectsInCategory[index];
      if (audioObject.IsPaused())
        audioObject.Unpause(fadeInLength);
    }
  }

  public static void StopCategory(string categoryName, float fadeOutLength = 0)
  {
    if ((UnityEngine.Object) AudioController._currentMusic != (UnityEngine.Object) null && AudioController._currentMusic.category.Name == categoryName)
      AudioController.StopMusic(fadeOutLength);
    List<AudioObject> objectsInCategory = AudioController.GetPlayingAudioObjectsInCategory(categoryName);
    for (int index = 0; index < objectsInCategory.Count; ++index)
      objectsInCategory[index].Stop(fadeOutLength);
  }

  public static bool IsPlaying(string audioID)
  {
    return AudioController.GetPlayingAudioObjects(audioID).Count > 0;
  }

  public static List<AudioObject> GetPlayingAudioObjects(string audioID, bool includePausedAudio = false)
  {
    List<AudioObject> playingAudioObjects1 = AudioController.GetPlayingAudioObjects(includePausedAudio);
    List<AudioObject> playingAudioObjects2 = new List<AudioObject>(playingAudioObjects1.Count);
    for (int index = 0; index < playingAudioObjects1.Count; ++index)
    {
      AudioObject audioObject = playingAudioObjects1[index];
      if ((UnityEngine.Object) audioObject != (UnityEngine.Object) null && audioObject.audioID == audioID)
        playingAudioObjects2.Add(audioObject);
    }
    return playingAudioObjects2;
  }

  public static List<AudioObject> GetPlayingAudioObjectsInCategory(
    string categoryName,
    bool includePausedAudio = false)
  {
    List<AudioObject> playingAudioObjects = AudioController.GetPlayingAudioObjects(includePausedAudio);
    List<AudioObject> objectsInCategory = new List<AudioObject>(playingAudioObjects.Count);
    for (int index = 0; index < playingAudioObjects.Count; ++index)
    {
      AudioObject audioObject = playingAudioObjects[index];
      if ((UnityEngine.Object) audioObject != (UnityEngine.Object) null && audioObject.DoesBelongToCategory(categoryName))
        objectsInCategory.Add(audioObject);
    }
    return objectsInCategory;
  }

  public static List<AudioObject> GetPlayingAudioObjects(bool includePausedAudio = false)
  {
    object[] allOfType = RegisteredComponentController.GetAllOfType(typeof (AudioObject));
    List<AudioObject> playingAudioObjects = new List<AudioObject>(allOfType.Length);
    for (int index = 0; index < allOfType.Length; ++index)
    {
      AudioObject audioObject = (AudioObject) allOfType[index];
      if (audioObject.IsPlaying() || includePausedAudio && audioObject.IsPaused())
        playingAudioObjects.Add(audioObject);
    }
    return playingAudioObjects;
  }

  public static int GetPlayingAudioObjectsCount(string audioID, bool includePausedAudio = false)
  {
    List<AudioObject> playingAudioObjects = AudioController.GetPlayingAudioObjects(includePausedAudio);
    int audioObjectsCount = 0;
    for (int index = 0; index < playingAudioObjects.Count; ++index)
    {
      AudioObject audioObject = playingAudioObjects[index];
      if ((UnityEngine.Object) audioObject != (UnityEngine.Object) null && audioObject.audioID == audioID)
        ++audioObjectsCount;
    }
    return audioObjectsCount;
  }

  public static void EnableMusic(bool b)
  {
    SingletonMonoBehaviour<AudioController>.Instance.musicEnabled = b;
  }

  public static void MuteSound(bool b)
  {
    SingletonMonoBehaviour<AudioController>.Instance.soundMuted = b;
  }

  public static bool IsMusicEnabled()
  {
    return SingletonMonoBehaviour<AudioController>.Instance.musicEnabled;
  }

  public static bool IsSoundMuted() => SingletonMonoBehaviour<AudioController>.Instance.soundMuted;

  public static AudioListener GetCurrentAudioListener()
  {
    AudioController instance = SingletonMonoBehaviour<AudioController>.Instance;
    if ((UnityEngine.Object) instance._currentAudioListener != (UnityEngine.Object) null && (UnityEngine.Object) instance._currentAudioListener.gameObject == (UnityEngine.Object) null)
      instance._currentAudioListener = (AudioListener) null;
    if ((UnityEngine.Object) instance._currentAudioListener == (UnityEngine.Object) null)
      instance._currentAudioListener = (AudioListener) UnityEngine.Object.FindObjectOfType(typeof (AudioListener));
    return instance._currentAudioListener;
  }

  public static AudioObject GetCurrentMusic() => AudioController._currentMusic;

  public static AudioCategory GetCategory(string name)
  {
    AudioController instance = SingletonMonoBehaviour<AudioController>.Instance;
    AudioCategory category1 = instance._GetCategory(name);
    if (category1 != null)
      return category1;
    if (instance._additionalAudioControllers != null)
    {
      for (int index = 0; index < instance._additionalAudioControllers.Count; ++index)
      {
        AudioCategory category2 = instance._additionalAudioControllers[index]._GetCategory(name);
        if (category2 != null)
          return category2;
      }
    }
    return (AudioCategory) null;
  }

  public static void SetCategoryVolume(string name, float volume)
  {
    bool flag = false;
    AudioController instance = SingletonMonoBehaviour<AudioController>.Instance;
    AudioCategory category1 = instance._GetCategory(name);
    if (category1 != null)
    {
      category1.Volume = volume;
      flag = true;
    }
    if (instance._additionalAudioControllers != null)
    {
      for (int index = 0; index < instance._additionalAudioControllers.Count; ++index)
      {
        AudioCategory category2 = instance._additionalAudioControllers[index]._GetCategory(name);
        if (category2 != null)
        {
          category2.Volume = volume;
          flag = true;
        }
      }
    }
    if (flag)
      return;
    Logger.WarningFormat("No audio category with name {0}", (object) name);
  }

  public static float GetCategoryVolume(string name)
  {
    AudioCategory category = AudioController.GetCategory(name);
    if (category != null)
      return category.Volume;
    Logger.WarningFormat("No audio category with name {0}", (object) name);
    return 0.0f;
  }

  public static void SetGlobalVolume(float volume)
  {
    AudioController instance = SingletonMonoBehaviour<AudioController>.Instance;
    instance.Volume = volume;
    if (instance._additionalAudioControllers == null)
      return;
    for (int index = 0; index < instance._additionalAudioControllers.Count; ++index)
      instance._additionalAudioControllers[index].Volume = volume;
  }

  public static float GetGlobalVolume() => SingletonMonoBehaviour<AudioController>.Instance.Volume;

  public static AudioCategory NewCategory(string categoryName)
  {
    int length = SingletonMonoBehaviour<AudioController>.Instance.AudioCategories == null ? 0 : SingletonMonoBehaviour<AudioController>.Instance.AudioCategories.Length;
    AudioCategory[] audioCategories = SingletonMonoBehaviour<AudioController>.Instance.AudioCategories;
    SingletonMonoBehaviour<AudioController>.Instance.AudioCategories = new AudioCategory[length + 1];
    if (length > 0)
      audioCategories.CopyTo((Array) SingletonMonoBehaviour<AudioController>.Instance.AudioCategories, 0);
    AudioCategory audioCategory = new AudioCategory(SingletonMonoBehaviour<AudioController>.Instance);
    audioCategory.Name = categoryName;
    SingletonMonoBehaviour<AudioController>.Instance.AudioCategories[length] = audioCategory;
    SingletonMonoBehaviour<AudioController>.Instance._InvalidateCategories();
    return audioCategory;
  }

  public static void RemoveCategory(string categoryName)
  {
    int num = -1;
    int length = SingletonMonoBehaviour<AudioController>.Instance.AudioCategories == null ? 0 : SingletonMonoBehaviour<AudioController>.Instance.AudioCategories.Length;
    for (int index = 0; index < length; ++index)
    {
      if (SingletonMonoBehaviour<AudioController>.Instance.AudioCategories[index].Name == categoryName)
      {
        num = index;
        break;
      }
    }
    if (num == -1)
    {
      Logger.ErrorFormat("AudioCategory does not exist: {0}", (object) categoryName);
    }
    else
    {
      AudioCategory[] audioCategoryArray = new AudioCategory[SingletonMonoBehaviour<AudioController>.Instance.AudioCategories.Length - 1];
      for (int index = 0; index < num; ++index)
        audioCategoryArray[index] = SingletonMonoBehaviour<AudioController>.Instance.AudioCategories[index];
      for (int index = num + 1; index < SingletonMonoBehaviour<AudioController>.Instance.AudioCategories.Length; ++index)
        audioCategoryArray[index - 1] = SingletonMonoBehaviour<AudioController>.Instance.AudioCategories[index];
      SingletonMonoBehaviour<AudioController>.Instance.AudioCategories = audioCategoryArray;
      SingletonMonoBehaviour<AudioController>.Instance._InvalidateCategories();
    }
  }

  public static void AddToCategory(AudioCategory category, AudioItem audioItem)
  {
    int length = category.AudioItems == null ? 0 : category.AudioItems.Length;
    AudioItem[] audioItems = category.AudioItems;
    category.AudioItems = new AudioItem[length + 1];
    if (length > 0)
      audioItems.CopyTo((Array) category.AudioItems, 0);
    category.AudioItems[length] = audioItem;
    SingletonMonoBehaviour<AudioController>.Instance._InvalidateCategories();
  }

  public static AudioItem AddToCategory(
    AudioCategory category,
    AudioClip audioClip,
    string audioID)
  {
    AudioItem audioItem = new AudioItem();
    audioItem.Name = audioID;
    audioItem.subItems = new AudioSubItem[1];
    audioItem.subItems[0] = new AudioSubItem()
    {
      Clip = audioClip
    };
    AudioController.AddToCategory(category, audioItem);
    return audioItem;
  }

  public static bool RemoveAudioItem(string audioID)
  {
    AudioItem audioItem = SingletonMonoBehaviour<AudioController>.Instance._GetAudioItem(audioID);
    if (audioItem == null)
      return false;
    int indexOf = audioItem.category._GetIndexOf(audioItem);
    if (indexOf < 0)
      return false;
    AudioItem[] audioItems = audioItem.category.AudioItems;
    AudioItem[] audioItemArray = new AudioItem[audioItems.Length - 1];
    for (int index = 0; index < indexOf; ++index)
      audioItemArray[index] = audioItems[index];
    for (int index = indexOf + 1; index < audioItems.Length; ++index)
      audioItemArray[index - 1] = audioItems[index];
    audioItem.category.AudioItems = audioItemArray;
    if (SingletonMonoBehaviour<AudioController>.Instance._categoriesValidated)
      SingletonMonoBehaviour<AudioController>.Instance._audioItems.Remove(audioID);
    return true;
  }

  public static bool IsValidAudioID(string audioID)
  {
    return SingletonMonoBehaviour<AudioController>.Instance._GetAudioItem(audioID) != null;
  }

  public static AudioItem GetAudioItem(string audioID)
  {
    return SingletonMonoBehaviour<AudioController>.Instance._GetAudioItem(audioID);
  }

  public static void DetachAllAudios(GameObject gameObjectWithAudios)
  {
    foreach (Component componentsInChild in gameObjectWithAudios.GetComponentsInChildren<AudioObject>(true))
      componentsInChild.transform.parent = (Transform) null;
  }

  public static float GetAudioItemMaxDistance(string audioID)
  {
    AudioItem audioItem = AudioController.GetAudioItem(audioID);
    if (audioItem.overrideAudioSourceSettings)
      return audioItem.audioSource_MaxDistance;
    return (UnityEngine.Object) audioItem.category.AudioObjectPrefab != (UnityEngine.Object) null ? audioItem.category.AudioObjectPrefab.GetComponent<AudioSource>().maxDistance : audioItem.category.audioController.AudioObjectPrefab.GetComponent<AudioSource>().maxDistance;
  }

  public void UnloadAllAudioClips()
  {
    for (int index = 0; index < this.AudioCategories.Length; ++index)
      this.AudioCategories[index].UnloadAllAudioClips();
  }

  private static AudioObject _currentMusic
  {
    set => AudioController._currentMusicReference.Set(value, true);
    get => AudioController._currentMusicReference.Get();
  }

  private void _ApplyVolumeChange()
  {
    List<AudioObject> playingAudioObjects = AudioController.GetPlayingAudioObjects(true);
    for (int index = 0; index < playingAudioObjects.Count; ++index)
    {
      AudioObject audioObject = playingAudioObjects[index];
      if ((UnityEngine.Object) audioObject != (UnityEngine.Object) null)
        audioObject._ApplyVolumeBoth();
    }
  }

  internal AudioItem _GetAudioItem(string audioID)
  {
    this._ValidateCategories();
    AudioItem audioItem;
    return this._audioItems.TryGetValue(audioID, out audioItem) ? audioItem : (AudioItem) null;
  }

  protected AudioObject _PlayMusic(string audioID, float volume, float delay, float startTime)
  {
    AudioListener currentAudioListener = AudioController.GetCurrentAudioListener();
    if (!((UnityEngine.Object) currentAudioListener == (UnityEngine.Object) null))
      return this._PlayMusic(audioID, currentAudioListener.transform.position + currentAudioListener.transform.forward, (Transform) null, volume, delay, startTime);
    this.LogWarning<AudioController>("No AudioListener found in the scene");
    return (AudioObject) null;
  }

  protected bool _StopMusic(float fadeOutLength)
  {
    if (!((UnityEngine.Object) AudioController._currentMusic != (UnityEngine.Object) null))
      return false;
    AudioController._currentMusic.Stop(fadeOutLength);
    AudioController._currentMusic = (AudioObject) null;
    return true;
  }

  protected bool _PauseMusic(float fadeOut)
  {
    if (!((UnityEngine.Object) AudioController._currentMusic != (UnityEngine.Object) null))
      return false;
    AudioController._currentMusic.Pause(fadeOut);
    return true;
  }

  protected AudioObject _PlayMusic(
    string audioID,
    Vector3 position,
    Transform parentObj,
    float volume,
    float delay,
    float startTime)
  {
    if (Player.IsLocalDedi)
      return (AudioObject) null;
    if (!AudioController.IsMusicEnabled())
      return (AudioObject) null;
    bool flag;
    if ((UnityEngine.Object) AudioController._currentMusic != (UnityEngine.Object) null && AudioController._currentMusic.IsPlaying())
    {
      flag = true;
      AudioController._currentMusic.Stop(this.musicCrossFadeTime_Out);
    }
    else
      flag = false;
    AudioController._currentMusic = this._PlayAsMusic(audioID, volume, position, parentObj, delay, startTime, false, 0.0);
    if ((bool) (UnityEngine.Object) AudioController._currentMusic && flag && (double) this.musicCrossFadeTime_In > 0.0)
      AudioController._currentMusic.FadeIn(this.musicCrossFadeTime_In);
    return AudioController._currentMusic;
  }

  protected int _EnqueueMusic(string audioID)
  {
    int length = this.musicPlaylist != null ? this.musicPlaylist.Length + 1 : 1;
    string[] strArray = new string[length];
    if (this.musicPlaylist != null)
      this.musicPlaylist.CopyTo((Array) strArray, 0);
    strArray[length - 1] = audioID;
    this.musicPlaylist = strArray;
    return length;
  }

  protected AudioObject _PlayMusicPlaylist()
  {
    this._ResetLastPlayedList();
    return this._PlayNextMusicOnPlaylist(0.0f);
  }

  private AudioObject _PlayMusicTrackWithID(int nextTrack, float delay, bool addToPlayedList)
  {
    if (nextTrack < 0)
      return (AudioObject) null;
    AudioController._playlistPlayed.Add(nextTrack);
    AudioController._isPlaylistPlaying = true;
    AudioObject audioObject = this._PlayMusic(this.musicPlaylist[nextTrack], 1f, delay, 0.0f);
    if ((UnityEngine.Object) audioObject != (UnityEngine.Object) null)
    {
      audioObject._isCurrentPlaylistTrack = true;
      audioObject.primaryAudioSource.loop = false;
    }
    return audioObject;
  }

  internal AudioObject _PlayNextMusicOnPlaylist(float delay)
  {
    return this._PlayMusicTrackWithID(this._GetNextMusicTrack(), delay, true);
  }

  internal AudioObject _PlayPreviousMusicOnPlaylist(float delay)
  {
    return this._PlayMusicTrackWithID(this._GetPreviousMusicTrack(), delay, false);
  }

  private void _ResetLastPlayedList() => AudioController._playlistPlayed.Clear();

  protected int _GetNextMusicTrack()
  {
    if (this.musicPlaylist == null || this.musicPlaylist.Length == 0)
      return -1;
    if (this.musicPlaylist.Length == 1)
      return 0;
    return this.shufflePlaylist ? this._GetNextMusicTrackShuffled() : this._GetNextMusicTrackInOrder();
  }

  protected int _GetPreviousMusicTrack()
  {
    if (this.musicPlaylist == null || this.musicPlaylist.Length == 0)
      return -1;
    if (this.musicPlaylist.Length == 1)
      return 0;
    return this.shufflePlaylist ? this._GetPreviousMusicTrackShuffled() : this._GetPreviousMusicTrackInOrder();
  }

  private int _GetPreviousMusicTrackShuffled()
  {
    if (AudioController._playlistPlayed.Count < 2)
      return -1;
    int musicTrackShuffled = AudioController._playlistPlayed[AudioController._playlistPlayed.Count - 2];
    this._RemoveLastPlayedOnList();
    this._RemoveLastPlayedOnList();
    return musicTrackShuffled;
  }

  private void _RemoveLastPlayedOnList()
  {
    AudioController._playlistPlayed.RemoveAt(AudioController._playlistPlayed.Count - 1);
  }

  private int _GetNextMusicTrackShuffled()
  {
    HashSet_Flash<int> hashSetFlash = new HashSet_Flash<int>();
    int num1 = AudioController._playlistPlayed.Count;
    if (this.loopPlaylist)
    {
      int num2 = Mathf.Clamp(this.musicPlaylist.Length / 4, 2, 10);
      if (num1 > this.musicPlaylist.Length - num2)
      {
        num1 = this.musicPlaylist.Length - num2;
        if (num1 < 1)
          num1 = 1;
      }
    }
    else if (num1 >= this.musicPlaylist.Length)
      return -1;
    for (int index = 0; index < num1; ++index)
      hashSetFlash.Add(AudioController._playlistPlayed[AudioController._playlistPlayed.Count - 1 - index]);
    List<int> intList = new List<int>();
    for (int index = 0; index < this.musicPlaylist.Length; ++index)
    {
      if (!hashSetFlash.Contains(index))
        intList.Add(index);
    }
    return intList[UnityEngine.Random.Range(0, intList.Count)];
  }

  private int _GetNextMusicTrackInOrder()
  {
    if (AudioController._playlistPlayed.Count == 0)
      return 0;
    int musicTrackInOrder = AudioController._playlistPlayed[AudioController._playlistPlayed.Count - 1] + 1;
    if (musicTrackInOrder >= this.musicPlaylist.Length)
    {
      if (!this.loopPlaylist)
        return -1;
      musicTrackInOrder = 0;
    }
    return musicTrackInOrder;
  }

  private int _GetPreviousMusicTrackInOrder()
  {
    if (AudioController._playlistPlayed.Count < 2)
      return this.loopPlaylist ? this.musicPlaylist.Length - 1 : -1;
    int musicTrackInOrder = AudioController._playlistPlayed[AudioController._playlistPlayed.Count - 1] - 1;
    this._RemoveLastPlayedOnList();
    this._RemoveLastPlayedOnList();
    if (musicTrackInOrder < 0)
    {
      if (!this.loopPlaylist)
        return -1;
      musicTrackInOrder = this.musicPlaylist.Length - 1;
    }
    return musicTrackInOrder;
  }

  protected AudioObject _PlayAsSound(
    string audioID,
    float volume,
    Vector3 worldPosition,
    Transform parentObj,
    float delay,
    float startTime,
    bool playWithoutAudioObject,
    double dspTime = 0,
    AudioObject useExistingAudioObject = null,
    int seed = 0)
  {
    return this._PlayEx(audioID, volume, worldPosition, parentObj, delay, startTime, playWithoutAudioObject, dspTime, useExistingAudioObject, seed: seed);
  }

  protected AudioObject _PlayAsMusic(
    string audioID,
    float volume,
    Vector3 worldPosition,
    Transform parentObj,
    float delay,
    float startTime,
    bool playWithoutAudioObject,
    double dspTime = 0,
    AudioObject useExistingAudioObject = null)
  {
    return this._PlayEx(audioID, volume, worldPosition, parentObj, delay, startTime, playWithoutAudioObject, dspTime, useExistingAudioObject, true);
  }

  protected AudioObject _PlayEx(
    string audioID,
    float volume,
    Vector3 worldPosition,
    Transform parentObj,
    float delay,
    float startTime,
    bool playWithoutAudioObject,
    double dspTime = 0,
    AudioObject useExistingAudioObject = null,
    bool playAsMusic = false,
    int seed = 0)
  {
    if (Player.IsLocalDedi)
      return (AudioObject) null;
    if (this._audioDisabled)
      return (AudioObject) null;
    AudioItem audioItem = this._GetAudioItem(audioID);
    if (audioItem == null)
    {
      if (audioID != string.Empty)
        this.LogWarning<AudioController>("Audio item with name '{0}' does not exist", (object) audioID);
      return (AudioObject) null;
    }
    if (audioItem._lastPlayedTime > 0.0 && dspTime == 0.0 && AudioController.systemTime - audioItem._lastPlayedTime < (double) audioItem.MinTimeBetweenPlayCalls)
      return (AudioObject) null;
    if (audioItem.MaxInstanceCount > 0)
    {
      List<AudioObject> playingAudioObjects = AudioController.GetPlayingAudioObjects(audioID);
      if (playingAudioObjects.Count >= audioItem.MaxInstanceCount)
      {
        bool flag = playingAudioObjects.Count > audioItem.MaxInstanceCount;
        AudioObject audioObject = (AudioObject) null;
        for (int index = 0; index < playingAudioObjects.Count; ++index)
        {
          if ((flag || !playingAudioObjects[index].isFadingOut) && ((UnityEngine.Object) audioObject == (UnityEngine.Object) null || playingAudioObjects[index].startedPlayingAtTime < audioObject.startedPlayingAtTime))
            audioObject = playingAudioObjects[index];
        }
        if ((UnityEngine.Object) audioObject != (UnityEngine.Object) null)
          audioObject.Stop(!flag ? 0.2f : 0.0f);
      }
    }
    return this.PlayAudioItem(audioItem, volume, worldPosition, parentObj, delay, startTime, playWithoutAudioObject, useExistingAudioObject, dspTime, playAsMusic, seed);
  }

  public AudioObject PlayAudioItem(
    AudioItem sndItem,
    float volume,
    Vector3 worldPosition,
    Transform parentObj = null,
    float delay = 0,
    float startTime = 0,
    bool playWithoutAudioObject = false,
    AudioObject useExistingAudioObj = null,
    double dspTime = 0,
    bool playAsMusic = false,
    int seed = 0)
  {
    AudioObject audioObject1 = (AudioObject) null;
    sndItem._lastPlayedTime = AudioController.systemTime;
    AudioSubItem[] audioSubItemArray = AudioController._ChooseSubItems(sndItem, useExistingAudioObj, seed);
    if (audioSubItemArray == null || audioSubItemArray.Length == 0)
      return (AudioObject) null;
    for (int index = 0; index < audioSubItemArray.Length; ++index)
    {
      AudioSubItem subItem = audioSubItemArray[index];
      if (subItem != null)
      {
        AudioObject audioObject2 = this.PlayAudioSubItem(subItem, volume, worldPosition, parentObj, delay, startTime, playWithoutAudioObject, useExistingAudioObj, dspTime, playAsMusic);
        if ((bool) (UnityEngine.Object) audioObject2)
        {
          audioObject1 = audioObject2;
          audioObject1.audioID = sndItem.Name;
          if (sndItem.overrideAudioSourceSettings)
          {
            audioObject2._audioSource_MinDistance_Saved = audioObject2.primaryAudioSource.minDistance;
            audioObject2._audioSource_MaxDistance_Saved = audioObject2.primaryAudioSource.maxDistance;
            audioObject2.primaryAudioSource.minDistance = sndItem.audioSource_MinDistance;
            audioObject2.primaryAudioSource.maxDistance = sndItem.audioSource_MaxDistance;
            if ((UnityEngine.Object) audioObject2.secondaryAudioSource != (UnityEngine.Object) null)
            {
              audioObject2.secondaryAudioSource.minDistance = sndItem.audioSource_MinDistance;
              audioObject2.secondaryAudioSource.maxDistance = sndItem.audioSource_MaxDistance;
            }
          }
        }
      }
    }
    return audioObject1;
  }

  internal AudioCategory _GetCategory(string name)
  {
    for (int index = 0; index < this.AudioCategories.Length; ++index)
    {
      AudioCategory audioCategory = this.AudioCategories[index];
      if (audioCategory.Name == name)
        return audioCategory;
    }
    return (AudioCategory) null;
  }

  public void Update()
  {
    if (this._isAdditionalAudioController)
      return;
    AudioController._UpdateSystemTime();
  }

  private static void _UpdateSystemTime()
  {
    double timeSinceLaunch = SystemTime.timeSinceLaunch;
    if (AudioController._lastSystemTime >= 0.0)
    {
      AudioController._systemDeltaTime = timeSinceLaunch - AudioController._lastSystemTime;
      if (AudioController._systemDeltaTime <= (double) Time.maximumDeltaTime + 0.0099999997764825821)
        AudioController._systemTime += AudioController._systemDeltaTime;
      else
        AudioController._systemDeltaTime = 0.0;
    }
    else
    {
      AudioController._systemDeltaTime = 0.0;
      AudioController._systemTime = 0.0;
    }
    AudioController._lastSystemTime = timeSinceLaunch;
  }

  public override void Awake()
  {
    base.Awake();
    if (this.isAdditionalAudioController)
      return;
    this.AwakeSingleton();
  }

  public void OnEnable()
  {
    if (this.isAdditionalAudioController)
      SingletonMonoBehaviour<AudioController>.Instance._RegisterAdditionalAudioController(this);
    else
      this.AwakeSingleton();
    EventManager.Subscribe<PlayAudioEvent>(new EventSubscriber<PlayAudioEvent>(this.OnPlay));
  }

  public override bool isSingletonObject => !this._isAdditionalAudioController;

  public override void OnDestroy()
  {
    if (this.UnloadAudioClipsOnDestroy)
      this.UnloadAllAudioClips();
    base.OnDestroy();
  }

  private void OnDisable()
  {
    if (this.isAdditionalAudioController && (bool) (UnityEngine.Object) SingletonMonoBehaviour<AudioController>.DoesInstanceExist())
      SingletonMonoBehaviour<AudioController>.Instance._UnregisterAdditionalAudioController(this);
    EventManager.Unsubscribe<PlayAudioEvent>(new EventSubscriber<PlayAudioEvent>(this.OnPlay));
  }

  private void AwakeSingleton()
  {
    AudioController._UpdateSystemTime();
    if (this.Persistent)
      UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) this.gameObject);
    if ((UnityEngine.Object) this.AudioObjectPrefab == (UnityEngine.Object) null)
      this.LogError<AudioController>("No AudioObject prefab specified in AudioController.");
    else
      this._ValidateAudioObjectPrefab(this.AudioObjectPrefab);
    this._ValidateCategories();
    if (AudioController._playlistPlayed != null)
      return;
    AudioController._playlistPlayed = new List<int>();
    AudioController._isPlaylistPlaying = false;
  }

  protected void _ValidateCategories()
  {
    if (this._categoriesValidated)
      return;
    this.InitializeAudioItems();
    this._categoriesValidated = true;
  }

  protected void _InvalidateCategories() => this._categoriesValidated = false;

  public void InitializeAudioItems()
  {
    if (this.isAdditionalAudioController)
      return;
    this._audioItems = new Dictionary<string, AudioItem>();
    this._InitializeAudioItems(this);
    if (this._additionalAudioControllers == null)
      return;
    for (int index = 0; index < this._additionalAudioControllers.Count; ++index)
    {
      AudioController additionalAudioController = this._additionalAudioControllers[index];
      if ((UnityEngine.Object) additionalAudioController != (UnityEngine.Object) null)
        this._InitializeAudioItems(additionalAudioController);
    }
  }

  private void _InitializeAudioItems(AudioController audioController)
  {
    for (int index = 0; index < audioController.AudioCategories.Length; ++index)
    {
      AudioCategory audioCategory = audioController.AudioCategories[index];
      audioCategory.audioController = audioController;
      audioCategory._AnalyseAudioItems(this._audioItems);
      if ((bool) (UnityEngine.Object) audioCategory.AudioObjectPrefab)
        this._ValidateAudioObjectPrefab(audioCategory.AudioObjectPrefab);
    }
  }

  private void _RegisterAdditionalAudioController(AudioController ac)
  {
    if (this._additionalAudioControllers == null)
      this._additionalAudioControllers = new List<AudioController>();
    this._additionalAudioControllers.Add(ac);
    this._InvalidateCategories();
    this._SyncCategoryVolumes(ac, this);
  }

  private void _SyncCategoryVolumes(AudioController toSync, AudioController syncWith)
  {
    for (int index = 0; index < toSync.AudioCategories.Length; ++index)
    {
      AudioCategory audioCategory = toSync.AudioCategories[index];
      AudioCategory category = syncWith._GetCategory(audioCategory.Name);
      if (category != null)
        audioCategory.Volume = category.Volume;
    }
  }

  private void _UnregisterAdditionalAudioController(AudioController ac)
  {
    if (this._additionalAudioControllers != null)
    {
      for (int index = 0; index < this._additionalAudioControllers.Count; ++index)
      {
        if ((UnityEngine.Object) this._additionalAudioControllers[index] == (UnityEngine.Object) ac)
        {
          this._additionalAudioControllers.RemoveAt(index);
          this._InvalidateCategories();
          break;
        }
      }
    }
    else
      this.LogWarning<AudioController>("_UnregisterAdditionalAudioController: AudioController {0} not found", (object) ac.name);
  }

  protected static AudioSubItem[] _ChooseSubItems(
    AudioItem audioItem,
    AudioObject useExistingAudioObj,
    int seed = 0)
  {
    return AudioController._ChooseSubItems(audioItem, audioItem.SubItemPickMode, useExistingAudioObj, seed);
  }

  internal static AudioSubItem _ChooseSingleSubItem(
    AudioItem audioItem,
    AudioPickSubItemMode pickMode,
    AudioObject useExistingAudioObj)
  {
    return AudioController._ChooseSubItems(audioItem, pickMode, useExistingAudioObj)[0];
  }

  protected static AudioSubItem[] _ChooseSubItems(
    AudioItem audioItem,
    AudioPickSubItemMode pickMode,
    AudioObject useExistingAudioObj,
    int seed = 0)
  {
    if (audioItem.subItems == null)
      return (AudioSubItem[]) null;
    int length = audioItem.subItems.Length;
    if (length == 0)
      return (AudioSubItem[]) null;
    int index1 = 0;
    bool flag = !object.ReferenceEquals((object) useExistingAudioObj, (object) null);
    int lastChosen = !flag ? audioItem._lastChosen : useExistingAudioObj._lastChosenSubItemIndex;
    if (length > 1)
    {
      switch (pickMode)
      {
        case AudioPickSubItemMode.Disabled:
          return (AudioSubItem[]) null;
        case AudioPickSubItemMode.Random:
          index1 = AudioController._ChooseRandomSubitem(audioItem, true, lastChosen, seed: seed);
          break;
        case AudioPickSubItemMode.RandomNotSameTwice:
          index1 = AudioController._ChooseRandomSubitem(audioItem, false, lastChosen, seed: seed);
          break;
        case AudioPickSubItemMode.Sequence:
          index1 = (lastChosen + 1) % length;
          break;
        case AudioPickSubItemMode.SequenceWithRandomStart:
          index1 = lastChosen != -1 ? (lastChosen + 1) % length : UnityEngine.Random.Range(0, length);
          break;
        case AudioPickSubItemMode.AllSimultaneously:
          AudioSubItem[] audioSubItemArray = new AudioSubItem[length];
          for (int index2 = 0; index2 < length; ++index2)
            audioSubItemArray[index2] = audioItem.subItems[index2];
          return audioSubItemArray;
        case AudioPickSubItemMode.TwoSimultaneously:
          return new AudioSubItem[2]
          {
            AudioController._ChooseSingleSubItem(audioItem, AudioPickSubItemMode.RandomNotSameTwice, useExistingAudioObj),
            AudioController._ChooseSingleSubItem(audioItem, AudioPickSubItemMode.RandomNotSameTwice, useExistingAudioObj)
          };
        case AudioPickSubItemMode.StartLoopSequenceWithFirst:
          index1 = !flag ? 0 : (lastChosen + 1) % length;
          break;
        case AudioPickSubItemMode.RandomNotSameTwiceOddsEvens:
          index1 = AudioController._ChooseRandomSubitem(audioItem, false, lastChosen, true);
          break;
      }
    }
    if (flag)
      useExistingAudioObj._lastChosenSubItemIndex = index1;
    else
      audioItem._lastChosen = index1;
    return new AudioSubItem[1]{ audioItem.subItems[index1] };
  }

  private static int _ChooseRandomSubitem(
    AudioItem audioItem,
    bool allowSameElementTwiceInRow,
    int lastChosen,
    bool switchOddsEvens = false,
    int seed = 0)
  {
    int length = audioItem.subItems.Length;
    int num1 = 0;
    float num2 = 0.0f;
    float num3;
    if (!allowSameElementTwiceInRow)
    {
      if (lastChosen >= 0)
      {
        num2 = audioItem.subItems[lastChosen]._SummedProbability;
        if (lastChosen >= 1)
          num2 -= audioItem.subItems[lastChosen - 1]._SummedProbability;
      }
      else
        num2 = 0.0f;
      num3 = 1f - num2;
    }
    else
      num3 = 1f;
    if (seed != 0)
      num3 = 1f;
    if (seed == 0)
      seed = (int) ((double) UnityEngine.Random.value * 100.0);
    float num4 = (float) new System.Random(seed).Next(0, (int) num3 * 1000) / 1000f;
    int i;
    for (i = 0; i < length - 1; ++i)
    {
      float summedProbability = audioItem.subItems[i]._SummedProbability;
      if (!switchOddsEvens || AudioController.isOdd(i) != AudioController.isOdd(lastChosen))
      {
        if (!allowSameElementTwiceInRow)
        {
          if (i != lastChosen || (double) summedProbability == 1.0 && audioItem.subItems[i].DisableOtherSubitems)
          {
            if (i > lastChosen)
              summedProbability -= num2;
          }
          else
            continue;
        }
        if ((double) summedProbability > (double) num4)
        {
          num1 = i;
          break;
        }
      }
    }
    if (i == length - 1)
      num1 = length - 1;
    return num1;
  }

  private static bool isOdd(int i) => i % 2 != 0;

  public AudioObject PlayAudioSubItem(
    AudioSubItem subItem,
    float volume,
    Vector3 worldPosition,
    Transform parentObj,
    float delay,
    float startTime,
    bool playWithoutAudioObject,
    AudioObject useExistingAudioObj,
    double dspTime = 0,
    bool playAsMusic = false)
  {
    if (subItem == null)
    {
      this.LogError<AudioController>("Trying to play a null subItem. please ensure no subitems are null");
      return (AudioObject) null;
    }
    this._ValidateCategories();
    AudioItem audioItem = subItem.item;
    switch (subItem.SubItemType)
    {
      case AudioSubItemType.Item:
        if (subItem.ItemModeAudioID.Length != 0)
          return this._PlayAsSound(subItem.ItemModeAudioID, volume, worldPosition, parentObj, delay, startTime, playWithoutAudioObject, dspTime, useExistingAudioObj);
        this.LogWarning<AudioController>("No item specified in audio sub-item with ITEM mode (audio item: '{0}')", (object) audioItem.Name);
        return (AudioObject) null;
      default:
        if ((UnityEngine.Object) subItem.Clip == (UnityEngine.Object) null)
          return (AudioObject) null;
        AudioCategory category = audioItem.category;
        float num = subItem.Volume * audioItem.Volume * volume;
        if ((double) subItem.RandomVolume != 0.0 || (double) audioItem.loopSequenceRandomVolume != 0.0)
        {
          float max = subItem.RandomVolume + audioItem.loopSequenceRandomVolume;
          num = Mathf.Clamp01(num + UnityEngine.Random.Range(-max, max));
        }
        float volume1 = num * category.VolumeTotal;
        AudioController audioController = this._GetAudioController(subItem);
        if (!audioController.PlayWithZeroVolume && ((double) volume1 <= 0.0 || (double) this.Volume <= 0.0))
          return (AudioObject) null;
        GameObject prefab = !((UnityEngine.Object) category.AudioObjectPrefab != (UnityEngine.Object) null) ? (!((UnityEngine.Object) audioController.AudioObjectPrefab != (UnityEngine.Object) null) ? this.AudioObjectPrefab : audioController.AudioObjectPrefab) : category.AudioObjectPrefab;
        if (playWithoutAudioObject)
        {
          prefab.GetComponent<AudioSource>().PlayOneShot(subItem.Clip, AudioObject.TransformVolume(volume1));
          return (AudioObject) null;
        }
        GameObject target;
        AudioObject audioObject;
        if ((UnityEngine.Object) useExistingAudioObj == (UnityEngine.Object) null)
        {
          if (audioItem.DestroyOnLoad)
          {
            target = !audioController.UsePooledAudioObjects ? ObjectPoolController.InstantiateWithoutPool(prefab, worldPosition, Quaternion.identity) : ObjectPoolController.Instantiate(prefab, worldPosition, Quaternion.identity);
          }
          else
          {
            target = ObjectPoolController.InstantiateWithoutPool(prefab, worldPosition, Quaternion.identity);
            UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) target);
          }
          if ((bool) (UnityEngine.Object) parentObj)
            target.transform.parent = parentObj;
          audioObject = target.gameObject.GetComponent<AudioObject>();
        }
        else
        {
          target = useExistingAudioObj.gameObject;
          audioObject = useExistingAudioObj;
        }
        audioObject.subItem = subItem;
        if (object.ReferenceEquals((object) useExistingAudioObj, (object) null))
          audioObject._lastChosenSubItemIndex = audioItem._lastChosen;
        audioObject.primaryAudioSource.clip = subItem.Clip;
        target.name = "AudioObject:" + audioObject.primaryAudioSource.clip.name;
        audioObject.primaryAudioSource.pitch = AudioObject.TransformPitch(subItem.PitchShift);
        audioObject.primaryAudioSource.panStereo = subItem.Pan2D;
        if (audioItem.overrideAudioSourceSettings)
          audioObject.primaryAudioSource.spatialBlend = audioItem.spatialBlend;
        if (subItem.RandomStartPosition)
          startTime = UnityEngine.Random.Range(0.0f, audioObject.clipLength);
        audioObject.primaryAudioSource.time = startTime + subItem.ClipStartTime;
        audioObject.primaryAudioSource.loop = audioItem.Loop == AudioItem.LoopMode.LoopSubitem || audioItem.Loop == (AudioItem.LoopMode.LoopSubitem | AudioItem.LoopMode.LoopSequence);
        audioObject._volumeExcludingCategory = num;
        audioObject._volumeFromScriptCall = volume;
        audioObject.category = category;
        audioObject.isPlayedAsMusic = playAsMusic;
        audioObject._ApplyVolumePrimary();
        if ((double) subItem.RandomPitch != 0.0 || (double) audioItem.loopSequenceRandomPitch != 0.0)
        {
          float max = subItem.RandomPitch + audioItem.loopSequenceRandomPitch;
          audioObject.primaryAudioSource.pitch *= AudioObject.TransformPitch(UnityEngine.Random.Range(-max, max));
        }
        if ((double) subItem.RandomDelay > 0.0)
          delay += UnityEngine.Random.Range(0.0f, subItem.RandomDelay);
        if (dspTime > 0.0)
          audioObject.PlayScheduled(dspTime + (double) delay + (double) subItem.Delay + (double) audioItem.Delay);
        else
          audioObject.Play(delay + subItem.Delay + audioItem.Delay);
        if ((double) subItem.FadeIn > 0.0)
          audioObject.FadeIn(subItem.FadeIn);
        return audioObject;
    }
  }

  private AudioController _GetAudioController(AudioSubItem subItem)
  {
    return subItem.item != null && subItem.item.category != null ? subItem.item.category.audioController : this;
  }

  internal void _NotifyPlaylistTrackCompleteleyPlayed(AudioObject audioObject)
  {
    audioObject._isCurrentPlaylistTrack = false;
    if (!AudioController.IsPlaylistPlaying() || !((UnityEngine.Object) AudioController._currentMusic == (UnityEngine.Object) audioObject) || !((UnityEngine.Object) this._PlayNextMusicOnPlaylist(this.delayBetweenPlaylistTracks) == (UnityEngine.Object) null))
      return;
    AudioController._isPlaylistPlaying = false;
  }

  private void _ValidateAudioObjectPrefab(GameObject audioPrefab)
  {
    if (this.UsePooledAudioObjects && !((UnityEngine.Object) audioPrefab.GetComponent<PoolableObject>() == (UnityEngine.Object) null))
      ObjectPoolController.Preload(audioPrefab);
    if (!((UnityEngine.Object) audioPrefab.GetComponent<AudioObject>() == (UnityEngine.Object) null))
      return;
    this.LogError<AudioController>("AudioObject prefab must have the AudioObject script component!");
  }
}
