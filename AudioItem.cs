// Decompiled with JetBrains decompiler
// Type: AudioItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class AudioItem
{
  public string Name;
  public AudioItem.LoopMode Loop;
  public int loopSequenceCount;
  public float loopSequenceOverlap;
  public float loopSequenceRandomDelay;
  public float loopSequenceRandomPitch;
  public float loopSequenceRandomVolume;
  public bool DestroyOnLoad = true;
  public float Volume = 1f;
  public AudioPickSubItemMode SubItemPickMode = AudioPickSubItemMode.RandomNotSameTwice;
  public float MinTimeBetweenPlayCalls = 0.1f;
  public int MaxInstanceCount;
  public float Delay;
  public bool overrideAudioSourceSettings;
  public float audioSource_MinDistance = 1f;
  public float audioSource_MaxDistance = 500f;
  public float spatialBlend = 1f;
  public AudioSubItem[] subItems;
  internal int _lastChosen = -1;
  internal double _lastPlayedTime = -1.0;
  [NonSerialized]
  private AudioCategory _category;

  public AudioCategory category
  {
    private set => this._category = value;
    get => this._category;
  }

  public void Awake()
  {
    if (this.Loop == (AudioItem.LoopMode.LoopSubitem | AudioItem.LoopMode.LoopSequence))
      this.Loop = AudioItem.LoopMode.LoopSequence;
    this._lastChosen = -1;
  }

  internal void _Initialize(AudioCategory categ)
  {
    this.category = categ;
    this._NormalizeSubItems();
  }

  private void _NormalizeSubItems()
  {
    float num1 = 0.0f;
    int num2 = 0;
    bool flag = false;
    foreach (AudioSubItem subItem in this.subItems)
    {
      if (AudioItem._IsValidSubItem(subItem) && subItem.DisableOtherSubitems)
      {
        flag = true;
        break;
      }
    }
    foreach (AudioSubItem subItem in this.subItems)
    {
      subItem.item = this;
      if (AudioItem._IsValidSubItem(subItem) && (subItem.DisableOtherSubitems || !flag))
        num1 += subItem.Probability;
      subItem._subItemID = num2;
      ++num2;
    }
    if ((double) num1 <= 0.0)
      return;
    float num3 = 0.0f;
    foreach (AudioSubItem subItem in this.subItems)
    {
      if (AudioItem._IsValidSubItem(subItem))
      {
        if (subItem.DisableOtherSubitems || !flag)
          num3 += subItem.Probability / num1;
        subItem._SummedProbability = num3;
      }
    }
  }

  private static bool _IsValidSubItem(AudioSubItem item)
  {
    switch (item.SubItemType)
    {
      case AudioSubItemType.Clip:
        return (UnityEngine.Object) item.Clip != (UnityEngine.Object) null;
      case AudioSubItemType.Item:
        return item.ItemModeAudioID != null && item.ItemModeAudioID.Length > 0;
      default:
        return false;
    }
  }

  public void UnloadAudioClip()
  {
    foreach (AudioSubItem subItem in this.subItems)
    {
      if ((bool) (UnityEngine.Object) subItem.Clip)
        Resources.UnloadAsset((UnityEngine.Object) subItem.Clip);
    }
  }

  [Serializable]
  public enum LoopMode
  {
    DoNotLoop = 0,
    LoopSubitem = 1,
    LoopSequence = 2,
    PlaySequenceAndLoopLast = 4,
    IntroLoopOutroSequence = 5,
  }
}
