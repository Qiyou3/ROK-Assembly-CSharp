// Decompiled with JetBrains decompiler
// Type: AudioCategory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class AudioCategory
{
  public string Name;
  private AudioCategory _parentCategory;
  [SerializeField]
  private string _parentCategoryName;
  public GameObject AudioObjectPrefab;
  public AudioItem[] AudioItems;
  [SerializeField]
  private float _volume = 1f;

  public AudioCategory(AudioController audioController) => this.audioController = audioController;

  public float Volume
  {
    get => this._volume;
    set
    {
      this._volume = value;
      this._ApplyVolumeChange();
    }
  }

  public float VolumeTotal
  {
    get
    {
      return this.parentCategory != null ? this.parentCategory.VolumeTotal * this._volume : this._volume;
    }
  }

  public AudioCategory parentCategory
  {
    set
    {
      this._parentCategory = value;
      if (value != null)
        this._parentCategoryName = this._parentCategory.Name;
      else
        this._parentCategoryName = (string) null;
    }
    get
    {
      if (string.IsNullOrEmpty(this._parentCategoryName))
        return (AudioCategory) null;
      if (this._parentCategory == null)
      {
        if ((UnityEngine.Object) this.audioController != (UnityEngine.Object) null)
          this._parentCategory = this.audioController._GetCategory(this._parentCategoryName);
        else
          this.LogWarning<AudioCategory>("_audioController == null");
      }
      return this._parentCategory;
    }
  }

  public AudioController audioController { get; set; }

  internal void _AnalyseAudioItems(Dictionary<string, AudioItem> audioItemsDict)
  {
    if (this.AudioItems == null)
      return;
    foreach (AudioItem audioItem in this.AudioItems)
    {
      if (audioItem != null)
      {
        audioItem._Initialize(this);
        if (audioItemsDict != null)
        {
          try
          {
            audioItemsDict.Add(audioItem.Name, audioItem);
          }
          catch (ArgumentException ex)
          {
            this.LogWarning<AudioCategory>("Multiple audio items with name '{0}'", (object) audioItem.Name);
          }
        }
      }
    }
  }

  internal int _GetIndexOf(AudioItem audioItem)
  {
    if (this.AudioItems == null)
      return -1;
    for (int indexOf = 0; indexOf < this.AudioItems.Length; ++indexOf)
    {
      if (audioItem == this.AudioItems[indexOf])
        return indexOf;
    }
    return -1;
  }

  private void _ApplyVolumeChange()
  {
    List<AudioObject> playingAudioObjects = AudioController.GetPlayingAudioObjects();
    for (int index = 0; index < playingAudioObjects.Count; ++index)
    {
      AudioObject audioObject = playingAudioObjects[index];
      if (this._IsCategoryParentOf(audioObject.category, this))
        audioObject._ApplyVolumeBoth();
    }
  }

  private bool _IsCategoryParentOf(AudioCategory toTest, AudioCategory parent)
  {
    for (AudioCategory audioCategory = toTest; audioCategory != null; audioCategory = audioCategory.parentCategory)
    {
      if (audioCategory == parent)
        return true;
    }
    return false;
  }

  public void UnloadAllAudioClips()
  {
    for (int index = 0; index < this.AudioItems.Length; ++index)
      this.AudioItems[index].UnloadAudioClip();
  }
}
