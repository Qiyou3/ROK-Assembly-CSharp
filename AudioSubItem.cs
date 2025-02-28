// Decompiled with JetBrains decompiler
// Type: AudioSubItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7AE521BE-DDCA-4BB1-9869-8132CF2A08FD
// Assembly location: C:\Users\85206\Desktop\Oxide.ReignOfKings-develop\src\Dependencies\windows\ROK_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class AudioSubItem
{
  public AudioSubItemType SubItemType;
  public float Probability = 1f;
  public bool DisableOtherSubitems;
  public string ItemModeAudioID;
  public AudioClip Clip;
  public float Volume = 1f;
  public float PitchShift;
  public float Pan2D;
  public float Delay;
  public float RandomPitch;
  public float RandomVolume;
  public float RandomDelay;
  public float ClipStopTime;
  public float ClipStartTime;
  public float FadeIn;
  public float FadeOut;
  public bool RandomStartPosition;
  private float _summedProbability = -1f;
  internal int _subItemID;
  [NonSerialized]
  private AudioItem _item;

  internal float _SummedProbability
  {
    get => this._summedProbability;
    set => this._summedProbability = value;
  }

  public AudioItem item
  {
    internal set => this._item = value;
    get => this._item;
  }

  public override string ToString()
  {
    return this.SubItemType == AudioSubItemType.Clip ? "CLIP: " + this.Clip.name : "ITEM: " + this.ItemModeAudioID;
  }
}
